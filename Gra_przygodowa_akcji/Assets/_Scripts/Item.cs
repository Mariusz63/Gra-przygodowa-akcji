using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType {
        Weapon,
        Armor,
        Collectible,
        Utility,
    }

    public ItemType itemType;
    public int amount;
    public bool stackable;
    public string itemName;
    public ItemShape itemShape;
}
