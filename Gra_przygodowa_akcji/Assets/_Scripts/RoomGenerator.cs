using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator
{
    // Private fields for storing generation parameters
    private int maxIterations;
    private int roomLengthMin;
    private int roomWidthMin;

    // Constructor to initialize the room generator with given parameters
    public RoomGenerator(int maxIterations, int roomLengthMin, int roomWidthMin)
    {
        this.maxIterations = maxIterations;
        this.roomLengthMin = roomLengthMin;
        this.roomWidthMin = roomWidthMin;
    }

    // Method to generate rooms within given spaces
    public List<RoomNode> GenerateRoomsInGivenSpaces(List<Node> roomSpaces, float roomBottomCornerModifier, float roomTopCornerMidifier, int roomOffset)
    {
        List<RoomNode> listToReturn = new List<RoomNode>();

        // Iterate through each space provided
        foreach (var space in roomSpaces)
        {
            // Generate new bottom left point for the room within the space
            Vector2Int newBottomLeftPoint = StructureHelper.GenerateBottomLeftCornerBetween(
                space.BottomLeftAreaCorner, space.TopRightAreaCorner, roomBottomCornerModifier, roomOffset);

            // Generate new top right point for the room within the space
            Vector2Int newTopRightPoint = StructureHelper.GenerateTopRightCornerBetween(
                space.BottomLeftAreaCorner, space.TopRightAreaCorner, roomTopCornerMidifier, roomOffset);

            // Update space corners to reflect the new room dimensions
            space.BottomLeftAreaCorner = newBottomLeftPoint;
            space.TopRightAreaCorner = newTopRightPoint;
            space.BottomRightAreaCorner = new Vector2Int(newTopRightPoint.x, newBottomLeftPoint.y);
            space.TopLeftAreaCorner = new Vector2Int(newBottomLeftPoint.x, newTopRightPoint.y);

            // Add the updated space (now a RoomNode) to the return list
            listToReturn.Add((RoomNode)space);
        }

        return listToReturn;
    }
}
