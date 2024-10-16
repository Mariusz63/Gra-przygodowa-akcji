using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Singleton
public class GlobalState : MonoBehaviour
{
    public static GlobalState Instance { get; set; }

    public float resourceHealth, resourceMaxHealth;

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
}

