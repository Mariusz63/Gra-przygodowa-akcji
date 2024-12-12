using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private Slider slider;
    public Text staminaCounter;
    public GameObject playerState;
    private float currentStamina, maxStamina;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        currentStamina = playerState.GetComponent<PlayerState>().currentStamina;
        maxStamina = playerState.GetComponent<PlayerState>().maxStamina;

        float fillValue = currentStamina / maxStamina; // 0 - empty, 1 - full
        slider.value = fillValue;
        staminaCounter.text = currentStamina + "/" + maxStamina; // e.g. 88/100
    }
}
