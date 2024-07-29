using System;
using System.Collections.Generic;
using UnityEngine;

// helper methods to create rooms, coridors, space
public class DungeonGenerator
{
    RoomNode rootNode;
    List<RoomNode> allSpaceNodes = new List<RoomNode> ();
    private int dungeonWidth;
    private int dungeonLength;

    public DungeonGenerator(int dungeonWidth, int dungeonHeight)
    {
        this.dungeonWidth = dungeonWidth;
        this.dungeonLength = dungeonHeight;
    }

    public List<Node> CalculateRooms(int maxIterations, int roomWidthMin, int roomLenghtMin)
    {
        BinarySpacePartitioner bsp = new BinarySpacePartitioner(dungeonWidth, dungeonLength);
        allSpaceNodes = bsp.PrepareNodesCollection(maxIterations, roomWidthMin, roomLenghtMin);
        return new List<Node>( allSpaceNodes);
    }
}