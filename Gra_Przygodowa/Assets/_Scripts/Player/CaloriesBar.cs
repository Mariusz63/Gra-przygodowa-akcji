using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaloriesBar : MonoBehaviour
{
    private Slider slider;
    public Text caloriesCounter;
    public GameObject playerState;
    private float currentCalories, maxCalories;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        currentCalories = playerState.GetComponent<PlayerState>().currentCalories;
        maxCalories = playerState.GetComponent<PlayerState>().maxHealth;

        float fillValue = currentCalories / maxCalories; // 0 - empty, 1 - full
        slider.value = fillValue;
        caloriesCounter.text = currentCalories + "/" + maxCalories; // e.g. 88/100

    }
}
