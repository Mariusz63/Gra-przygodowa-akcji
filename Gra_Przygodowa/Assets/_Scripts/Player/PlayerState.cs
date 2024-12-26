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
    public float staminaDecreseMultiplier = 1.5f;
    public float disntaceToDecreseStamina = 20f;

    // Declare lastPosition and distanceTravelled variables
    private Vector3 lastPosition;
    private float distanceTravelled = 0;

    // Player hand damage
    public float playerDamage = 5;

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
        if (distanceTravelled >= disntaceToDecreseStamina)
        {
            distanceTravelled = 0;
            float staminaToDecreseSprinting = staminaDecrese * staminaDecreseMultiplier;
            if (MovementManager.Instance.canSprinting && currentStamina> staminaToDecreseSprinting)
                currentStamina -= (staminaToDecreseSprinting);
            currentStamina -= staminaDecrese;
        }


        // Update lastPosition to the current position at the end of the frame
        lastPosition = playerBody.transform.position;
    }

    public void SetHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    /// <summary>
    /// Set 
    /// </summary>
    /// <param name="newStamina"></param>
    public void SetStamina(float newStamina)
    {
        currentStamina = newStamina;
    }

    /// <summary>
    /// Reduce health
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            Debug.Log("Player is dead");
        }
        else
        {
            Debug.Log("Player is hurt");
        }

    }


}
