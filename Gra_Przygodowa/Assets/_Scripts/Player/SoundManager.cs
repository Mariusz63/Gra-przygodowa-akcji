using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singleton
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

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

    public void PlaySound(object grassWalkSound)
    {
        throw new NotImplementedException();
    }
}
