using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FixModelOrientation : MonoBehaviour
{
    void Start()
    {
        // Obr�t o 90 stopni w osi X
        transform.rotation = Quaternion.Euler(-90, 0, 0);
    }
}
