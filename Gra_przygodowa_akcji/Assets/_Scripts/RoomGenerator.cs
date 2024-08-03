using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator
{
    private int maxIterations;
    private int roomLenghtMin;
    private int roomWidthMin;

    public RoomGenerator(int maxIterations, int roomLenghtMin, int roomWidthMin)
    {
        this.maxIterations = maxIterations;
        this.roomLenghtMin = roomLenghtMin;
        this.roomWidthMin = roomWidthMin;
    }

    public List<RoomNode> GenerateRoomsInGivenSpaces(List<Node> roomSpaces, float roomBottomCornerModifier, float roomTopCornerModifier, int offset)
    {

        List<RoomNode> listToReturn = new List<RoomNode>();

        //creating corner points
        foreach (var space in roomSpaces)
        {
            Vector2Int newBottomLeftPoint = StructureHelper.GenerateBottomLeftCornerBetween(
                space.BottomLefAreaCorner, space.TopRightAreaCorner, roomBottomCornerModifier, offset);

            Vector2Int newTopRightPoint = StructureHelper.GenerateBottomLeftCornerBetween(
                space.BottomLefAreaCorner, space.TopRightAreaCorner, roomTopCornerModifier, offset);

            space.BottomLefAreaCorner = newBottomLeftPoint;
            space.TopRightAreaCorner = newTopRightPoint;

            space.BottomRightAreaCorner = new Vector2Int(newTopRightPoint.x, newBottomLeftPoint.y);
            space.TopLeftAreaCorner = new Vector2Int(newBottomLeftPoint.x, newTopRightPoint.y);

            listToReturn.Add((RoomNode)space);
        }

        return listToReturn;
    }
}