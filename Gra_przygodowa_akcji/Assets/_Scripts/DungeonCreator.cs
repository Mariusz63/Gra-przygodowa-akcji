using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    public int dungeonWidth, dungeonHeight;
    public int roomWidthMin, roomLenghtMin;
    public int maxIterations;
    public int corridorWidth;
    public Material material;

    [Range(0.0f, 0.3f)]
    public float roomBottomCornerModifier;

    [Range(0.7f, 1.0f)]
    public float roomTopCornerModifier;

    [Range(0.0f, 2.0f)]
    public int offset;

    // Start is called before the first frame update
    void Start()
    {
        CreateDungeon();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateDungeon()
    {
        DungeonGenerator generator = new DungeonGenerator(dungeonWidth, dungeonHeight);
        var listOfRooms = generator.CalculateDungeon(maxIterations, roomWidthMin, roomLenghtMin, roomBottomCornerModifier, roomTopCornerModifier, offset, corridorWidth);

        for (int i = 0; i < listOfRooms.Count; i++)
        {
            CreateMesh(listOfRooms[i].BottomLefAreaCorner, listOfRooms[i].TopRightAreaCorner);
        }
    }

    private void CreateMesh(Vector2Int bottomLeftCorner, Vector2Int topRightCorner)
    {
        Vector3 bottomLeftVertex = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
        Vector3 bottomRightVertex = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
        Vector3 topLeftVertex = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
        Vector3 topRightVertex = new Vector3(topRightCorner.x, 0, topRightCorner.y);

        Vector3[] vertices = new Vector3[]
        {
            topLeftVertex,
            topRightVertex,
            bottomLeftVertex,
            bottomRightVertex,
        };

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
        {
            0, 1,2,2,1,3
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        GameObject dungeonFloor = new GameObject("Mesh" + bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer));
        //set dungeon floor options
        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
    }
}
