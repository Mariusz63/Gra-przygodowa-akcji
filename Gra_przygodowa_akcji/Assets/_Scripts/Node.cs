using System;
using System.Collections.Generic;
using UnityEngine;

// Abstract class Node, representing a node in a tree structure
public abstract class Node
{
    // Private list to hold child nodes
    private List<Node> childrenNodeList;

    // Public property to access the list of child nodes
    public List<Node> ChildrenNodeList { get => childrenNodeList; }

    // Public property to indicate if the node has been visited
    public bool Visted { get; set; }

    // Public properties for the corners of the area the node represents
    public Vector2Int BottomLeftAreaCorner { get; set; }
    public Vector2Int BottomRightAreaCorner { get; set; }
    public Vector2Int TopRightAreaCorner { get; set; }
    public Vector2Int TopLeftAreaCorner { get; set; }

    // Public property to hold the parent node
    public Node Parent { get; set; }

    // Public property to hold the index of the node in the tree layer
    public int TreeLayerIndex { get; set; }

    // Constructor to initialize a node with its parent
    public Node(Node parentNode)
    {
        // Initialize the list of child nodes
        childrenNodeList = new List<Node>();

        // Set the parent node
        this.Parent = parentNode;

        // If a parent node is provided, add this node as a child to the parent
        if (parentNode != null)
        {
            parentNode.AddChild(this);
        }
    }

    // Method to add a child node to this node
    public void AddChild(Node node)
    {
        childrenNodeList.Add(node);
    }

    // Method to remove a child node from this node
    public void RemoveChild(Node node)
    {
        childrenNodeList.Remove(node);
    }
}
