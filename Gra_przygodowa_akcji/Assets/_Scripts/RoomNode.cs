using UnityEngine;

public class RoomNode : Node
{
    // Constructor to initialize the RoomNode
    public RoomNode(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, Node parentNode, int index)
        : base(parentNode) // Call the base class constructor with parentNode
    {
        this.BottomLeftAreaCorner = bottomLeftAreaCorner; // Set the bottom left corner
        this.TopRightAreaCorner = topRightAreaCorner; // Set the top right corner
        this.BottomRightAreaCorner = new Vector2Int(topRightAreaCorner.x, bottomLeftAreaCorner.y); // Calculate and set the bottom right corner
        this.TopLeftAreaCorner = new Vector2Int(bottomLeftAreaCorner.x, TopRightAreaCorner.y); // Calculate and set the top left corner
        this.TreeLayerIndex = index; // Set the layer index in the tree
    }

    // Property to get the width of the room
    public int Width
    {
        get => (int)(TopRightAreaCorner.x - BottomLeftAreaCorner.x); // Calculate width as the difference between x-coordinates
    }

    // Property to get the length of the room
    public int Length
    {
        get => (int)(TopRightAreaCorner.y - BottomLeftAreaCorner.y); // Calculate length as the difference between y-coordinates
    }
}
