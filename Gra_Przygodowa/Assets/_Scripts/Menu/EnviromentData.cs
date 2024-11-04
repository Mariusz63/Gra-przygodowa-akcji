using System;
using System.Collections.Generic;
using Unity.VisualScripting;

[Serializable]
public class EnviromentData
{
    public List<string> pickedupItems;

    public EnviromentData(List<string> _pickedupItems) 
    {
        pickedupItems = _pickedupItems;

    }



}