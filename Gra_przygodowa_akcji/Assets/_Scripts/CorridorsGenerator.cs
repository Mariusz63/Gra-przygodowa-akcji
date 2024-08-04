using System;
using System.Collections.Generic;
using System.Linq;

public class CorridorsGenerator
{
    // Method to create corridors between nodes
    public List<Node> CreateCorridor(List<RoomNode> allNodesCollection, int corridorWidth)
    {
        List<Node> corridorList = new List<Node>();

        // Create a queue to process nodes, ordered by TreeLayerIndex in descending order
        Queue<RoomNode> structuresToCheck = new Queue<RoomNode>(
            allNodesCollection.OrderByDescending(node => node.TreeLayerIndex).ToList());

        // Process each node in the queue
        while (structuresToCheck.Count > 0)
        {
            var node = structuresToCheck.Dequeue();

            // If the node has no children
            if (node.ChildrenNodeList.Count == 0)
            {
                continue;
            }

            // Create a corridor between the first two children of the node
            CorridorNode corridor = new CorridorNode(node.ChildrenNodeList[0], node.ChildrenNodeList[1], corridorWidth);

            // Add the created corridor to the list
            corridorList.Add(corridor);
        }

        return corridorList;
    }
}
