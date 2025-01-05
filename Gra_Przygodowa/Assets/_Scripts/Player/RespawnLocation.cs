using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnLocation : MonoBehaviour
{
    public bool isRegistered = false;
    public GameObject indicator; // something to show player when he register
    public KeyCode registerKeyCode = KeyCode.F;

    private void Start()
    {
        PlayerState.Instance.OnRespawnRegistered += UnRegisterLocation;
    }

    private void RegisterLocation()
    {
        PlayerState.Instance.SetRegisteredLocation(this);
        isRegistered = true;
        indicator.SetActive(true);
    }

    private void UnRegisterLocation()
    {
        if(PlayerState.Instance.respawnLocation != this)
        {
            isRegistered = false;
            indicator.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && Input.GetKeyDown(registerKeyCode))
        {
            RegisterLocation();
        }
    }
}
