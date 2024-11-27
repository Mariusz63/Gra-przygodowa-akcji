using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singleton
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    //Efekty
    public AudioSource dropItemSound;
    public AudioSource toolSwingSound;
    public AudioSource chopSound;
    public AudioSource pickupItemSound;
    public AudioSource grassWalkSound;

    // Muzyka
    public AudioSource startBGMusic;

    // G³osy
    public AudioSource voices;

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

    //Sprawdzamy czy podany dzwiek jest teraz uzywany
    public void PlaySound(AudioSource soundToPlay)
    {
        if (!soundToPlay.isPlaying)
        {
            soundToPlay.Play();
        }
    }

    public void PlayVoices(AudioClip voiceToPlay)
    {
        voices.clip = voiceToPlay;
        if(!voices.isPlaying)
        {
            voices.Play();
        }
        else
        {
            voices.Stop();
            voices.Play();
        }
    }

}
