using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController2 : MonoBehaviour
{
    [SerializeField] ItemGrid selectedItemGrid;

    // Update is called once per frame
    void Update()
    {
        if (selectedItemGrid == null) {  return; }

        selectedItemGrid.GetTileGridPosition(Input.mousePosition);
    }
}
