using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
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

[System.Serializable]
public class TreeData
{
    public string name; // Tree or stump
    public Vector3 position;
    public Vector3 rotation;
}

//[Serializable]
//public class TreeData
//{
//    public string name;
//    public SerializableVector3 position;
//    public SerializableVector3 rotation;

//    public TreeData(Vector3 position, Vector3 rotation, string name)
//    {
//        this.position = new SerializableVector3(position);
//        this.rotation = new SerializableVector3(rotation);
//        this.name = name;
//    }
//}


[System.Serializable]
public class StorageData
{
    public List<string> items; // list of items inside storage
    public Vector3 position;
    public Vector3 rotation;
}

//[Serializable]
//public class SerializableVector3
//{
//    public float x, y, z;

//    public SerializableVector3(Vector3 vector)
//    {
//        x = vector.x;
//        y = vector.y;
//        z = vector.z;
//    }

//    public Vector3 ToVector3()
//    {
//        return new Vector3(x, y, z);
//    }
//}
