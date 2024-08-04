using UnityEngine;

public class Line
{
    // Private fields for orientation and coordinates
    private Orientation orientation;
    private Vector2Int coordinates;

    // Constructor to initialize the line with orientation and coordinates
    public Line(Orientation orientation, Vector2Int coordinates)
    {
        this.orientation = orientation;
        this.coordinates = coordinates;
    }

    // Public properties to get and set orientation and coordinates
    public Orientation Orientation
    {
        get => orientation;
        set => orientation = value;
    }

    public Vector2Int Coordinates
    {
        get => coordinates;
        set => coordinates = value;
    }
}

// Enum representing the orientation of a line
public enum Orientation
{
    Horizontal = 0, // Horizontal orientation
    Vertical = 1    // Vertical orientation
}