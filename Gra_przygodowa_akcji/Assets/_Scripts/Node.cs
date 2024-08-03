using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    private List<Node> childrenNodeList;

    public List<Node> ChildrenNodeList { get => childrenNodeList; }

    public bool Visited { get; set; }
    public Vector2Int BottomLefAreaCorner { get; set; }
    public Vector2Int BottomRightAreaCorner { get; set; }
    public Vector2Int TopRightAreaCorner { get; set; }
    public Vector2Int TopLeftAreaCorner { get; set; }

    public Node Paretn { get; set; }

    public int TreeLayerIndex { get; set; }

    public Node(Node parentNode)
    {
        childrenNodeList = new List<Node>();
        this.Paretn = parentNode;

        if (parentNode != null)
        {
            parentNode.AddChild(this);
        }
    }

    private void AddChild(Node node)
    {
        childrenNodeList.Add(node);
    }

    private void RemoveChild(Node node)
    {
        childrenNodeList.Remove(node);
    }
}