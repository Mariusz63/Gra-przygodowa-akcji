using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static Assets._Scripts.Enums.Enum;

public class Animal : MonoBehaviour
{
    public string animalName;
    public bool playerInRange;

    [Header("Animal statistics")]
    [SerializeField] int currentHealt;
    [SerializeField] int maxHealth;

    [Header("Sounds")]
    [SerializeField] AudioSource soundChannel;
    [SerializeField] AudioClip animalHit;
    [SerializeField] AudioClip animalHitAndDie;

    [SerializeField] AnimalType thisAnimalType;

    public void Start()
    {
        currentHealt = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealt -= damage;

        if(currentHealt <= 0)
        {
            PlayDieSound();
            Destroy(gameObject);
        }
        else
        {
            PlayHitSound();
        }
    }

    void PlayHitSound()
    {
        switch (thisAnimalType)
        {
            case AnimalType.Spider:
                soundChannel.PlayOneShot(animalHit);
                break;
            default:
                break;
        }
    }

    void PlayDieSound()
    {
        switch (thisAnimalType)
        {
            case AnimalType.Spider:
                soundChannel.PlayOneShot(animalHitAndDie);
                break;
            default:
                break;
        }
    }
}
