using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ScriptableObject
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Checkpoint", order = 1)]
public class Checkpoint : ScriptableObject
{
    // opis 
    public string description;
    public bool isCompleted = false;
}
