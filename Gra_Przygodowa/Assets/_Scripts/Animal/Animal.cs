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
    [SerializeField] ParticleSystem bloodSplashParticles;
    [SerializeField] GameObject spwaenBloodPuddle;

    private Animator animator;
    public bool isDead = false;

    public void Start()
    {
        currentHealt = maxHealth;
        animator = GetComponent<Animator>();
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
        if (!isDead)
        {
            currentHealt -= damage;

            bloodSplashParticles.Play();

            if (currentHealt <= 0)
            {
                PlayDieSound();
                animator.SetTrigger("DIE");
                // GetComponent<AI_Movement>().enabled = false;
                //Destroy(gameObject);
                PuddleDelay();
                 isDead = true;
            }
            else
            {
                PlayHitSound();
            }
        }
    }

    IEnumerator PuddleDelay()
    {
        yield return new WaitForSeconds(1f);
        spwaenBloodPuddle.SetActive(true);
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
