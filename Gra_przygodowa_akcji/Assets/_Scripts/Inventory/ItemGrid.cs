using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    const int tilesizeWidth = 150;
    const int tilesizeHeight = 150;

    RectTransform rectTransform;

    Vector2 positionOnGrid = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public Vector2Int GetTileGridPosition (Vector2 mousePosition)
    {
        positionOnGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)positionOnGrid.x / tilesizeWidth;
        tileGridPosition.y = (int)positionOnGrid.y / tilesizeHeight;

        return tileGridPosition;
    }

}
