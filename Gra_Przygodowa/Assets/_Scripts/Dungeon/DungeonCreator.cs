using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using Random = UnityEngine.Random;

// Generowanie dungeona
public class DungeonCreator : MonoBehaviour
{
    public static DungeonCreator Instance { get; private set; }

    [SerializeField]
    private GameObject wejscie;

    [SerializeField] // pokkoje w dungeonie
    private List<GameObject> pokojeLista;

    [SerializeField] // Specjalne pokoje ( ex. special loot, interactions)
    private List<GameObject> specjalePokojeLista;

    [SerializeField] // Wejscia do dungeonu
    private List<GameObject> opcjonalneWejsciaLista;

    [SerializeField]
    private List<GameObject> korytarzeLista;

    [SerializeField]
    private GameObject drzwi;

    [SerializeField]
    private int liczbaPokoi = 10;

    [SerializeField]
    private LayerMask pokojeLayerMask;

    private List<DungeonPart> wygenerowanePokojeLista;

    private bool czyWygenerowany = false;

    private void Start()
    {
        Instance = this;
        wygenerowanePokojeLista = new List<DungeonPart>();
        // StartGeneration();
    }

    private void Awake()
    {
        Instance = this;
    }

    public void StartGeneration()
    {
        GenerujDungeon();
        GenerowanieAlternatywnychWejsc();
        UzupelnijPusteWejscia();
        czyWygenerowany = true;
    }

    public void GenerujDungeon()
    {
        //Dungeon gererator
        for (int i = 0; i < liczbaPokoi - opcjonalneWejsciaLista.Count; i++)
        {
            //if no rooms generated, generate an entrance
            if (wygenerowanePokojeLista.Count < 1)
            {
                GameObject generatedRoom = Instantiate(wejscie, transform.position, transform.rotation);
                generatedRoom.transform.SetParent(null);
                if (generatedRoom.TryGetComponent<DungeonPart>(out DungeonPart dungeonPart))
                {
                    wygenerowanePokojeLista.Add(dungeonPart);
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
                    int randomLinkRoomIndex = Random.Range(0, wygenerowanePokojeLista.Count);
                    DungeonPart roomToTest = wygenerowanePokojeLista[randomLinkRoomIndex];

                    if (roomToTest.CzyMaDostepnyPunktWejscia(out room1EntryPoint))
                    {
                        randomGeneratedRoom = roomToTest;
                        break;
                    }
                    retryIndex++;
                }

                GameObject doorToAlign = Instantiate(drzwi, transform.position, transform.rotation);

                if (shouldPlaceHallway)
                {
                    int randomIndex = Random.Range(0, korytarzeLista.Count);
                    GameObject generatedHallway = Instantiate(korytarzeLista[randomIndex], transform.position, transform.rotation);
                    generatedHallway.transform.SetParent(null);
                    if (generatedHallway.TryGetComponent<DungeonPart>(out DungeonPart dungeonPart))
                    {
                        if (dungeonPart.CzyMaDostepnyPunktWejscia(out Transform room2Entrypoint))
                        {
                            wygenerowanePokojeLista.Add(dungeonPart);
                            doorToAlign.transform.position = room1EntryPoint.transform.position;
                            doorToAlign.transform.rotation = room1EntryPoint.transform.rotation;
                            WyrownaniePomieszczen(randomGeneratedRoom.transform, generatedHallway.transform, room1EntryPoint, room2Entrypoint);

                            if (ObslugaSkrzyzowan(dungeonPart))
                            {
                                dungeonPart.NieuzywanyPunktWejscia(room2Entrypoint);
                                randomGeneratedRoom.NieuzywanyPunktWejscia(room1EntryPoint);
                                PonowProbeUmieszczenia(generatedHallway, doorToAlign);
                                continue;
                            }
                        }
                    }
                }
                else
                {
                    GameObject generatedRoom;
                    if (specjalePokojeLista.Count > 0)
                    {
                        bool shouldPlaceSpecialRoom = Random.Range(0f, 1f) > 0.9f;

                        if (shouldPlaceSpecialRoom)
                        {
                            int randomIndex = Random.Range(0, specjalePokojeLista.Count);
                            generatedRoom = Instantiate(specjalePokojeLista[randomIndex], transform.position, transform.rotation);
                        }
                        else
                        {
                            int randomIndex = Random.Range(0, pokojeLista.Count);
                            generatedRoom = Instantiate(pokojeLista[randomIndex], transform.position, transform.rotation);
                        }
                    }
                    else
                    {
                        int randomIndex = Random.Range(0, pokojeLista.Count);
                        generatedRoom = Instantiate(pokojeLista[randomIndex], transform.position, transform.rotation);
                    }
                    generatedRoom.transform.SetParent(null);

                    if (generatedRoom.TryGetComponent<DungeonPart>(out DungeonPart part))
                    {
                        if (part.CzyMaDostepnyPunktWejscia(out Transform room2Entrypoint))
                        {
                            wygenerowanePokojeLista.Add(part);
                            doorToAlign.transform.position = room1EntryPoint.transform.position;
                            doorToAlign.transform.rotation = room1EntryPoint.transform.rotation;
                            WyrownaniePomieszczen(randomGeneratedRoom.transform, generatedRoom.transform, room1EntryPoint, room2Entrypoint);

                            if (ObslugaSkrzyzowan(part))
                            {
                                part.NieuzywanyPunktWejscia(room2Entrypoint);
                                randomGeneratedRoom.NieuzywanyPunktWejscia(room1EntryPoint);
                                PonowProbeUmieszczenia(generatedRoom, doorToAlign);
                                continue;
                            }
                        }
                    }

                }
            }
        }
    }

    private void PonowProbeUmieszczenia(GameObject itemToPlace, GameObject doorToPlace)
    {
        DungeonPart randomGeneratedRoom = null;
        Transform room1Entrypoint = null;
        int totalRetries = 100;
        int retryIndex = 0;

        while (randomGeneratedRoom == null && retryIndex < totalRetries)
        {
            int randomLinkRoomIndex = Random.Range(0, wygenerowanePokojeLista.Count - 1);
            DungeonPart roomToTest = wygenerowanePokojeLista[randomLinkRoomIndex];

            if (roomToTest.CzyMaDostepnyPunktWejscia(out room1Entrypoint))
            {
                randomGeneratedRoom = roomToTest;
                break;
            }
            retryIndex++;
        }

        if (itemToPlace.TryGetComponent<DungeonPart>(out DungeonPart dungeonPart))
        {
            if (dungeonPart.CzyMaDostepnyPunktWejscia(out Transform room2Entrypoint))
            {
                doorToPlace.transform.position = room1Entrypoint.transform.position;
                doorToPlace.transform.rotation = room1Entrypoint.transform.rotation;
                WyrownaniePomieszczen(randomGeneratedRoom.transform, itemToPlace.transform, room1Entrypoint, room2Entrypoint);

                if (ObslugaSkrzyzowan(dungeonPart))
                {
                    dungeonPart.NieuzywanyPunktWejscia(room2Entrypoint);
                    randomGeneratedRoom.NieuzywanyPunktWejscia(room1Entrypoint);
                    PonowProbeUmieszczenia(itemToPlace, doorToPlace);
                }
            }
        }
    }

    private bool ObslugaSkrzyzowan(DungeonPart dungeonPart)
    {
        bool didIntersect = false;

        Collider[] hits = Physics.OverlapBox(dungeonPart.collider.bounds.center, dungeonPart.collider.bounds.size / 2, Quaternion.identity, pokojeLayerMask);

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

    private void WyrownaniePomieszczen(Transform transform1, Transform transform2, Transform room1EntryPoint, Transform room2Entrypoint)
    {    
        float angle = Vector3.Angle(room1EntryPoint.forward, room1EntryPoint.forward);

        transform2.TransformPoint(room2Entrypoint.position);
        transform2.eulerAngles = new Vector3(transform2.eulerAngles.x, transform2.eulerAngles.y + angle, transform2.eulerAngles.z);

        Vector3 offset = room1EntryPoint.position - room2Entrypoint.position;
        transform2.position += offset;
        Physics.SyncTransforms();
    }

    public void GenerowanieAlternatywnychWejsc()
    {
        if (opcjonalneWejsciaLista.Count < 1) return;
    }

    public void UzupelnijPusteWejscia()
    {
        wygenerowanePokojeLista.ForEach(roomList => roomList.UzupelnijPusteDrzwi());
    }

    public List<DungeonPart> PobierzWygenerowanePokoje() => wygenerowanePokojeLista;
    public bool CzyWygenerowany() => czyWygenerowany;
}
