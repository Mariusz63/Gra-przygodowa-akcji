using UnityEngine;
// storing informations about lines
public class Line
{
    Orientation orientation;
    Vector2Int coordinates;

    public Line(Orientation orientaion, Vector2Int coordinations)
    {
        this.Orientation = orientaion;
        this.Coordinates = coordinations;
    }

    public Orientation Orientation { get => orientation; set => orientation = value; }
    public Vector2Int Coordinates { get => coordinates; set => coordinates = value; }
}

public enum Orientation
{
    Horizontal = 0,
    Vertical = 1
}