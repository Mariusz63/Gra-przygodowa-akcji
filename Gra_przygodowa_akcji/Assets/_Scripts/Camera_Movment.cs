using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movment : MonoBehaviour
{
    public float mouseSensitivity = 200f;
    public Transform target;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        float mouseScroll = Input.GetAxis("Mouse ScrollWheel") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if ((Mathf.Round(transform.localPosition.z) <= -2 && mouseScroll > 0) || (Mathf.Round(transform.localPosition.z) >= -6 && mouseScroll < 0))
        {
            transform.localPosition += new Vector3 (mouseScroll * -0.5f, mouseScroll * -0.5f, mouseScroll * 2f);
        }
        
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        target.Rotate(Vector3.up * mouseX);
    }
}
