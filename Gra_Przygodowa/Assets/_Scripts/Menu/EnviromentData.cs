using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class EnviromentData
{
    public List<string> pickedupItems;
    public List<TreeData> treeData;
    public List<string> animals;
    public List<StorageData> storage;

    public EnviromentData(List<string> _pickedupItems, List<TreeData> _treeData, List<string> _animals, List<StorageData> _storage)
    {
        pickedupItems = _pickedupItems;
        treeData = _treeData;
        animals = _animals;
        storage = _storage;
    }
}

[Serializable]
public class TreeData
{
    public string name; // Tree or stump
    public Vector3 position;
    public Vector3 rotation;
}

[Serializable]
public class StorageData
{
    public List<string> items; // list of items inside storage
    public Vector3 position;
    public Vector3 rotation;
}
