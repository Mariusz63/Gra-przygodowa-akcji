using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class InventoryController
{
    private List<Item> Itemlist;
    public int inventoryLevel;

    public InventoryController() {
        Itemlist = new List<Item>();
        inventoryLevel = 0;
        
        Additem(new Item {});

        UnityEngine.Debug.Log(Itemlist.Count);
    }


    public void Additem (Item newitem){
        Itemlist.Add(newitem);
    }
}
