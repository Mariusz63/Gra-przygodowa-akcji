using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Singleton
public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; set; }

    // ------ Player Health ------- //
    public float currentHealth;
    public float maxHealth;

    // ------ Player Calories ------- //
    public float currentCalories;
    public float maxCalories;

    float distanceTravelled = 0;
    Vector3 lastPosition;

    public GameObject playerBody;

    // ------ Player Hydration ------- //
    public float currentHydration;
    public float maxHydration;

    public bool isHydrationActive = true;

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

    private void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydration = maxHydration;

        StartCoroutine(decreaseHtdration());
    }

    IEnumerator decreaseHtdration()
    {
        while (isHydrationActive)
        {
            currentHydration -= 1;
            yield return new WaitForSeconds(10);
        }
    }

    // Update is called once per frame
    void Update()
    {

        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = transform.position; //lastPosition is now position

        if (distanceTravelled >= 6)
        {
            distanceTravelled = 0;
            currentCalories -= 1;
        }

    }

    public void setHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    public void setHydration(float newHydration)
    {
        currentHydration = newHydration;
    }

    public void setCalories(float newCalories)
    {
        currentCalories = newCalories;
    }
}
