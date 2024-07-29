using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BinarySpacePartitioner
{
    RoomNode rootNode;

    public BinarySpacePartitioner(int dungeonWidth, int dungeonLength)
    {
        this.rootNode = new RoomNode(new Vector2Int(0,0), new Vector2Int(dungeonWidth, dungeonLength), null, 0);
    }

    public RoomNode RootNode { get => rootNode;  }

    public List<RoomNode> PrepareNodesCollection(int maxIterations, int roomWidthMin, int roomLenghtMin)
    {
        Queue<RoomNode> graph = new Queue<RoomNode>();
        List<RoomNode> listToReturn = new List<RoomNode>();
        graph.Enqueue(rootNode);
        listToReturn.Add(rootNode);
        int iterations = 0;
        
        while(iterations < maxIterations && graph.Count > 0) 
        {
            iterations++;
            RoomNode currentNode = graph.Dequeue(); //takes first node of the graph

            if(currentNode.Width >= roomWidthMin*2 || currentNode.Lenght >= roomLenghtMin*2)
            {
                SplitTheSpace(currentNode, listToReturn, roomLenghtMin, roomWidthMin, graph);
            }
        }

        return listToReturn;
    }

    private void SplitTheSpace(RoomNode currentNode, List<RoomNode> listToReturn, int roomLenghtMin, int roomWidthMin, Queue<RoomNode> graph)
    {
        Line line = GetLineDividingSpace(currentNode.BottomLefAreaCorner, currentNode.TopRightAreaCorner,
            roomWidthMin, roomLenghtMin);

        RoomNode node1, node2;

        if(line.Orientation == Orientation.Horizontal)
        {
            node1 = new RoomNode(currentNode.BottomLefAreaCorner, 
                new Vector2Int(currentNode.TopRightAreaCorner.x, line.Coordinates.y),
                currentNode,
                currentNode.TreeLayerIndex + 1);

            node2 = new RoomNode(
                new Vector2Int(currentNode.BottomLefAreaCorner.x, line.Coordinates.y),
                currentNode.TopRightAreaCorner,
                currentNode,
                currentNode.TreeLayerIndex + 1);
        }
        else
        {
            node1 = new RoomNode(currentNode.BottomLefAreaCorner,
                new Vector2Int(line.Coordinates.x, currentNode.TopRightAreaCorner.y),
                currentNode,
                currentNode.TreeLayerIndex + 1);

            node2 = new RoomNode(
                new Vector2Int(line.Coordinates.x, currentNode.BottomLefAreaCorner.y),
                currentNode.TopRightAreaCorner,
                currentNode,
                currentNode.TreeLayerIndex + 1);
        }

        AddNewNodeToCollections(listToReturn, graph, node1);
        AddNewNodeToCollections(listToReturn, graph, node2);
    }

    private void AddNewNodeToCollections(List<RoomNode> listToReturn, Queue<RoomNode> graph, RoomNode node)
    {
        listToReturn.Add(node);
        graph.Enqueue(node);
    }

    private Line GetLineDividingSpace(Vector2Int bottomLefAreaCorner, Vector2Int topRightAreaCorner, int roomWidthMin, int roomLenghtMin)
    {
        Orientation orientation;
        bool lengthStatus = (topRightAreaCorner.y - bottomLefAreaCorner.y) >= 2 * roomLenghtMin;
        bool widthStatus = (topRightAreaCorner.x - bottomLefAreaCorner.x) >= 2 * roomWidthMin;

        if (lengthStatus && widthStatus)
        {
            orientation = (Orientation)(Random.Range(0,2));
        }else if ( widthStatus)
        {
            orientation = Orientation.Vertical;
        }
        else
        {
            orientation = Orientation.Horizontal;
        }

        return new Line(orientation, GetCoordinatesForOrientation(orientation, bottomLefAreaCorner, topRightAreaCorner, roomWidthMin, roomLenghtMin));
    }

    private Vector2Int GetCoordinatesForOrientation(Orientation orientation, Vector2Int bottomLefAreaCorner, Vector2Int topRightAreaCorner, int roomWidthMin, int roomLenghtMin)
    {
        Vector2Int coordinates = Vector2Int.zero;

        if(orientation == Orientation.Horizontal)
        {
            coordinates = new Vector2Int(0,
                Random.Range(
                (bottomLefAreaCorner.y + roomLenghtMin),
                (topRightAreaCorner.y - roomLenghtMin)));
        }
        else
        {
            coordinates = new Vector2Int( 
                Random.Range(
                (bottomLefAreaCorner.x + roomWidthMin),
                (topRightAreaCorner.x - roomWidthMin)), 0);
        }

        return coordinates;
    }
}