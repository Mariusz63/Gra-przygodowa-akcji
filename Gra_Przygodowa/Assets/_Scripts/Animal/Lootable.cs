using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Set a list of items to be obtained and keep items already obtained and is calculated
/// </summary>
public class Lootable : MonoBehaviour
{
    public List<LootPossibility> possibleLoot;
    public List<LootRecived> finalLoot;

    public bool wasLootCalculated;
}

/// <summary>
/// Set possible loot
/// </summary>
[System.Serializable]
public class LootPossibility 
{
    public GameObject item;
    public int amountMin;
    public int amountMax;
}

/// <summary>
/// Stores information about the won lot
/// </summary>
[System.Serializable]
public class LootRecived
{
    public GameObject item;
    public int amount;
}