using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonCreator : MonoBehaviour
{
    public static DungeonCreator Instance { get; private set; }

    [SerializeField]
    private GameObject entrance;

    [SerializeField] // Rooms in dungeon
    private List<GameObject> roomList;

    [SerializeField] // Special rooms ( ex. special loot, interactions)
    private List<GameObject> specialRoomList;

    [SerializeField] // Entrances to the dungeon
    private List<GameObject> altrenceEntrancesList;

    [SerializeField]
    private List<GameObject> hallwaysList;

    [SerializeField]
    private GameObject door;

    [SerializeField]
    private int noOfRooms = 10;

    [SerializeField]
    private LayerMask roomsLayerMask;

    private List<DungeonPart> generatedRoomsList;

    private bool isGenerated = false;

    private void Start()
    {
        Instance = this;
        generatedRoomsList = new List<DungeonPart>();
        // StartGeneration();
    }

    private void Awake()
    {
        Instance = this;
    }

    public void StartGeneration()
    {
        Generate();
        GenerateAlternateEntrances();
        FillEmptyEntrances();
        isGenerated = true;
    }

    public void Generate()
    {
        //Dungeon gererator
        for (int i = 0; i < noOfRooms - altrenceEntrancesList.Count; i++)
        {
            //if no rooms generated, generate an entrance
            if (generatedRoomsList.Count < 1)
            {
                GameObject generatedRoom = Instantiate(entrance, transform.position, transform.rotation);
                generatedRoom.transform.SetParent(null);
                if (generatedRoom.TryGetComponent<DungeonPart>(out DungeonPart dungeonPart))
                {
                    generatedRoomsList.Add(dungeonPart);
                }
            }
            else
            {
                bool shouldPlaceHallway = Random.Range(0f, 1f) > 0.5f;
                DungeonPart randomGeneratedRoom = null;
                Transform room1EntryPoint = null;
                int totalRetries = 100;
                int retryIndex = 0;

                while (randomGeneratedRoom == null && retryIndex < totalRetries)
                {
                    int randomLinkRoomIndex = Random.Range(0, generatedRoomsList.Count);
                    DungeonPart roomToTest = generatedRoomsList[randomLinkRoomIndex];

                    if (roomToTest.HasAvailableEntrypoint(out room1EntryPoint))
                    {
                        randomGeneratedRoom = roomToTest;
                        break;
                    }
                    retryIndex++;
                }

                GameObject doorToAlign = Instantiate(door, transform.position, transform.rotation);

                if (shouldPlaceHallway)
                {
                    int randomIndex = Random.Range(0, hallwaysList.Count);
                    GameObject generatedHallway = Instantiate(hallwaysList[randomIndex], transform.position, transform.rotation);
                    generatedHallway.transform.SetParent(null);
                    if (generatedHallway.TryGetComponent<DungeonPart>(out DungeonPart dungeonPart))
                    {
                        if (dungeonPart.HasAvailableEntrypoint(out Transform room2Entrypoint))
                        {
                            generatedRoomsList.Add(dungeonPart);
                            doorToAlign.transform.position = room1EntryPoint.transform.position;
                            doorToAlign.transform.rotation = room1EntryPoint.transform.rotation;
                            AlignRooms(randomGeneratedRoom.transform, generatedHallway.transform, room1EntryPoint, room2Entrypoint);

                            if (HandleIntersection(dungeonPart))
                            {
                                dungeonPart.UnuseEntrypoint(room2Entrypoint);
                                randomGeneratedRoom.UnuseEntrypoint(room1EntryPoint);
                                RetryPlacement(generatedHallway, doorToAlign);
                                continue;
                            }
                        }
                    }
                }
                else
                {
                    GameObject generatedRoom;
                    if (specialRoomList.Count > 0)
                    {
                        bool shouldPlaceSpecialRoom = Random.Range(0f, 1f) > 0.9f;

                        if (shouldPlaceSpecialRoom)
                        {
                            int randomIndex = Random.Range(0, specialRoomList.Count);
                            generatedRoom = Instantiate(specialRoomList[randomIndex], transform.position, transform.rotation);
                        }
                        else
                        {
                            int randomIndex = Random.Range(0, roomList.Count);
                            generatedRoom = Instantiate(roomList[randomIndex], transform.position, transform.rotation);
                        }
                    }
                    else
                    {
                        int randomIndex = Random.Range(0, roomList.Count);
                        generatedRoom = Instantiate(roomList[randomIndex], transform.position, transform.rotation);
                    }
                    generatedRoom.transform.SetParent(null);

                    if(generatedRoom.TryGetComponent<DungeonPart>(out DungeonPart part))
                    {
                        if(part.HasAvailableEntrypoint(out Transform room2Entrypoint))
                        {
                            generatedRoomsList.Add(part);
                            doorToAlign.transform.position = room1EntryPoint.transform.position;
                            doorToAlign.transform.rotation = room1EntryPoint.transform.rotation;
                            AlignRooms(randomGeneratedRoom.transform, generatedRoom.transform,room1EntryPoint, room2Entrypoint);

                            if(HandleIntersection(part))
                            {
                                part.UnuseEntrypoint(room2Entrypoint);
                                randomGeneratedRoom.UnuseEntrypoint(room1EntryPoint);
                                RetryPlacement(generatedRoom, doorToAlign);
                                continue;
                            }
                        }
                    }

                }
            }
        }
    }

    private void RetryPlacement(GameObject itemToPlace, GameObject doorToPlace)
    {
        DungeonPart randomGeneratedRoom = null;
        Transform room1Entrypoint = null;
        int totalRetries = 100;
        int retryIndex = 0;

        while (randomGeneratedRoom == null && retryIndex < totalRetries)
        {
            int randomLinkRoomIndex = Random.Range(0, generatedRoomsList.Count - 1);
            DungeonPart roomToTest = generatedRoomsList[randomLinkRoomIndex];

            if (roomToTest.HasAvailableEntrypoint(out room1Entrypoint))
            {
                randomGeneratedRoom = roomToTest;
                break;
            }
            retryIndex++;
        }

        if (itemToPlace.TryGetComponent<DungeonPart>(out DungeonPart dungeonPart))
        {
            if (dungeonPart.HasAvailableEntrypoint(out Transform room2Entrypoint))
            {
                doorToPlace.transform.position = room1Entrypoint.transform.position;
                doorToPlace.transform.rotation = room1Entrypoint.transform.rotation;
                AlignRooms(randomGeneratedRoom.transform, itemToPlace.transform, room1Entrypoint, room2Entrypoint);

                if (HandleIntersection(dungeonPart))
                {
                    dungeonPart.UnuseEntrypoint(room2Entrypoint);
                    randomGeneratedRoom.UnuseEntrypoint(room1Entrypoint);
                    RetryPlacement(itemToPlace, doorToPlace);
                }
            }
        }

    }

    private bool HandleIntersection(DungeonPart dungeonPart)
    {
        bool didIntersect = false;

        Collider[] hits = Physics.OverlapBox(dungeonPart.collider.bounds.center, dungeonPart.collider.bounds.size / 2, Quaternion.identity, roomsLayerMask);

        foreach (Collider hit in hits)
        {
            if (hit == dungeonPart.collider) continue;

            if (hit != dungeonPart.collider)
            {
                didIntersect = true;
                break;
            }
        }
        return didIntersect;
    }

    private void AlignRooms(Transform transform1, Transform transform2, Transform room1EntryPoint, Transform room2Entrypoint)
    {
        //Debug.Log($"room1 {transform1}");
        //Debug.Log($"room2 {transform2}");
        //Debug.Log($"room1Entry {room1EntryPoint}");
        //Debug.Log($"room2Entry {room2EntryPoint}");
        float angle = Vector3.Angle(room1EntryPoint.forward, room1EntryPoint.forward);

        transform2.TransformPoint(room2Entrypoint.position);
        transform2.eulerAngles = new Vector3(transform2.eulerAngles.x, transform2.eulerAngles.y + angle, transform2.eulerAngles.z);

        Vector3 offset = room1EntryPoint.position - room2Entrypoint.position;
        transform2.position += offset;
        Physics.SyncTransforms();
    }

    public void GenerateAlternateEntrances()
    {
        if (altrenceEntrancesList.Count < 1) return;
    }

    public void FillEmptyEntrances()
    {
        generatedRoomsList.ForEach(roomList => roomList.FillEmptyDoors());
    }

    public List<DungeonPart> GetGeneratedRooms() => generatedRoomsList;
    public bool IsGenerated() => isGenerated;
}
