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

    public bool isPLayerDead;
    public RespawnLocation respawnLocation;
    public event Action OnRespawnRegistered;

    public GameObject playerBody;
    public GameObject deathscreenUI;

    // ------- Player Audio ----------
    public AudioSource playerAudioSource;
    public AudioClip playerPainSound;
    public AudioClip playerDeathSound;

    private float hurtSoundDelay = 2f;
    private float nextHurtTime = 1f;

    private void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;

        // Initialize lastPosition to the starting position of the player
        lastPosition = playerBody.transform.position;
        deathscreenUI.SetActive(false);
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
            if (MovementManager.Instance.canSprinting && currentStamina > staminaToDecreseSprinting)
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

        if (currentHealth <= 0 && !isPLayerDead)
        {
            Debug.Log("Player is dead");
            PlayerDead();
        }
        else
        {
            if (currentHealth > 0 && Time.time >= nextHurtTime)
            {
                Debug.Log("Player is hurt");
                playerAudioSource.PlayOneShot(playerPainSound);          
                nextHurtTime = Time.time + hurtSoundDelay;
            }
        }
    }

    public void PlayerDead()
    {
        isPLayerDead = true;
        playerAudioSource.PlayOneShot(playerDeathSound);
        //RespawnPlayer();
        OpenDeadScreen(true);
    }

    public void OpenDeadScreen(bool isOpen)
    {
        deathscreenUI.SetActive(isOpen);
        playerBody.GetComponent<PlayerMovement>().enabled = !isOpen;
        playerBody.GetComponent<MouseMovement>().enabled = !isOpen;
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOpen;
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCoroutine());
    }

    public IEnumerator RespawnCoroutine()
    { 
        if (respawnLocation != null)
        {
            Vector3 position = respawnLocation.transform.position;
            position.y += 7f; // go above the location
            position.z += 7f; // next too the location

            playerBody.transform.position = position;// actualy respawn the player

            currentHealth = maxHealth;
            currentStamina = maxStamina;
        }

        yield return new WaitForSeconds(0.3f);

        isPLayerDead = false;
        OpenDeadScreen(false);
    }

    public void SetRegisteredLocation(RespawnLocation _respawnLocation)
    {
        respawnLocation = _respawnLocation;
        OnRespawnRegistered?.Invoke();
    }
}
