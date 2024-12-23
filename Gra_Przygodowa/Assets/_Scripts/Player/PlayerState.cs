using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Singleton
public class PlayerState : MonoBehaviour
{
    #region || --------- Singleton ----------- ||
    public static PlayerState Instance { get; set; }
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

    // ------ Player Health ------- //
    public float currentHealth;
    public float maxHealth;

    // ------ Player Stamina ------- //
    public float currentStamina;
    public float maxStamina;
    public float staminaDecrese = 1;

    // Declare lastPosition and distanceTravelled variables
    private Vector3 lastPosition;
    private float distanceTravelled = 0;

    public GameObject playerBody;

    private void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;

        // Initialize lastPosition to the starting position of the player
        lastPosition = playerBody.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the distance from the previous position to the current position
        float frameDistance = Vector3.Distance(playerBody.transform.position, lastPosition);
        distanceTravelled += frameDistance;

        //Debug.Log("distanceTravelled: " + distanceTravelled.ToString("F2"));

        // Reset distanceTravelled and reduce stamina when the threshold is reached
        if (distanceTravelled >= 15)
        {
            distanceTravelled = 0;
            currentStamina -= staminaDecrese;

           // Debug.Log("Stamina decreased! Current Stamina: " + currentStamina);
        }

        // Update lastPosition to the current position at the end of the frame
        lastPosition = playerBody.transform.position;
    }

    public void setHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    public void setCalories(float newCalories)
    {
        currentStamina = newCalories;
    }
}
