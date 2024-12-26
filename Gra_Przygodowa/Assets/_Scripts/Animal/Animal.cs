using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static Assets._Scripts.Enums.Enum;

public class Animal : MonoBehaviour
{
    public string animalName;
    public bool playerInRange;

    [Header("Animal statistics")]
    [SerializeField] float currentHealt;
    [SerializeField] float maxHealth;

    [Header("Sounds")]
    [SerializeField] AudioSource soundChannel;
    [SerializeField] AudioClip animalHit;
    [SerializeField] AudioClip animalHitAndDie;
    [SerializeField] AudioClip animalAttack;

    [SerializeField] AnimalType thisAnimalType;
    [SerializeField] ParticleSystem bloodSplashParticles;
    [SerializeField] GameObject spwaenBloodPuddle;

    public Slider healthBarSlider;

    private Animator animator;
    public bool isDead = false;

    public void Start()
    {
        currentHealt = maxHealth;
        animator = GetComponent<Animator>();
    }

    private void UpdateHealthBar()
    {
        healthBarSlider.value = currentHealt / maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            healthBarSlider.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            healthBarSlider.gameObject.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        UpdateHealthBar();
        if (!isDead)
        {
            currentHealt -= damage;
            bloodSplashParticles.Play();

            if (currentHealt <= 0)
            {
                PlayDieSound();
                animator.SetTrigger("DIE");

                StartCoroutine(PuddleDelay());
                isDead = true;
            }
            else
            {
                PlayHitSound();
                animator.SetTrigger("HIT");
            }
        }
    }

    IEnumerator PuddleDelay()
    {
        yield return new WaitForSeconds(1f);
        spwaenBloodPuddle.SetActive(true);
    }

    /// <summary>
    /// Playing hit sound by animal type
    /// </summary>
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

    /// <summary>
    /// Playing die sound by animal type
    /// </summary>
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

    /// <summary>
    /// Playing attack sound by animal type
    /// </summary>
    public void PlayAttackSound()
    {
        switch (thisAnimalType)
        {
            case AnimalType.Spider:
                soundChannel.PlayOneShot(animalAttack);
                break;
            default:
                break;
        }
    }
}
