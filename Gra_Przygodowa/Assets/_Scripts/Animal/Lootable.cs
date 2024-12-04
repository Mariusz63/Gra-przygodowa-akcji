using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Set a list of items to be obtained and keep items already obtained and is calculated
public class Lootable : MonoBehaviour
{
    public List<LootPossibility> possibleLoot;
    public List<LootRecived> finalLoot;

    public bool wasLootCalculated;
}

// Set possible loot
[System.Serializable]
public class LootPossibility 
{
    public GameObject item;
    public int amountMin;
    public int amountMax;
}

// Stores information about the won lot
[System.Serializable]
public class LootRecived
{
    public GameObject item;
    public int amount;
}