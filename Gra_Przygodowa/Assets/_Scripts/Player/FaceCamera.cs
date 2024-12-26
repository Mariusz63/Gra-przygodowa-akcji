using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotate canvas to the camera
/// </summary>
public class FaceCamera : MonoBehaviour
{
    private Transform localTrans;
    // Start is called before the first frame update
    void Start()
    {
        localTrans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Camera.main)
            localTrans.LookAt(2* localTrans.position - Camera.main.transform.position);
    }
}
