using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public TextMeshProUGUI amountTXT;
    public InventoryItem itemInSlot;

    private void Update()
    {
        InventoryItem item = CheckInventoryItem();
        itemInSlot = item; // if there is not item then item == null

        if(itemInSlot != null)
        {
            amountTXT.gameObject.SetActive(true);
            amountTXT.text = $"{itemInSlot.amountInInventory}";
            amountTXT.gameObject.transform.SetAsLastSibling(); // Text is in fornt of item
        }
        else
        {
            amountTXT.gameObject.SetActive(false);
        }
    }

    private InventoryItem CheckInventoryItem()
    {
        foreach (Transform child  in transform)
        {
            if (child.GetComponent<InventoryItem>())
                return child.GetComponent<InventoryItem>();
        }
        return null;
    }

    public void UpdateItemInSlot()
    {
        itemInSlot = CheckInventoryItem();
    }
}
