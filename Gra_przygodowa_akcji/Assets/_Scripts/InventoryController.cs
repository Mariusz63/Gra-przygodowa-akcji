using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController
{
    private List<Item> Itemlist;

    public InventoryController() {
        Itemlist = new List<Item>();
        
        Additem(new Item {});

        UnityEngine.Debug.Log(Itemlist.Count);
    }


    public void Additem (Item newitem){
        Itemlist.Add(newitem);
    }
}
