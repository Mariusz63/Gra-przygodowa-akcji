using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    #region || ---------- Sigleton ---------- ||
    public static MovementManager Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    #endregion

    public bool canMove = true; // Keyboard Movement
    public bool canLookAround = true; // Mouse Movement

    public void EnableMovement(bool trigger)
    {
        canMove = trigger;
    }

    public void EnableLook(bool trigger)
    {
        canLookAround = trigger;
    }
}
