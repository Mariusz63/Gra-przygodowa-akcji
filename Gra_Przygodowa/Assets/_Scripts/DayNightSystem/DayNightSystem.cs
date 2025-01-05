using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayNightSystem : MonoBehaviour
{
    public Light directionalLight;
    public TextMeshProUGUI timeUI;

    public float dayDurationInSeconds = 24.0f;
    public int currentHour;
    float currentTimeOfDay = 0.35f;

    public List<SkyBoxTimeMapping> timeMappings;
    float blendedValue = 0.0f;
    bool lockNextDayTrigger = false;

    // Update is called once per frame
    void Update()
    {
        // calculate the current time of day based on the time
        currentTimeOfDay += Time.deltaTime / dayDurationInSeconds;
        currentTimeOfDay %= 1; // Ensure it stays between 0 nad 1

        currentHour = Mathf.FloorToInt(currentTimeOfDay * 24);
        timeUI.text = $"{currentHour}:00";

        // Update the directional light's rotation
        directionalLight.transform.rotation = Quaternion.Euler((currentTimeOfDay * 360) - 90, 170, 0);

        // Update the skybox material based on ther time of day
        UpdateSkyBox();
    }
    /// <summary>
    /// Find the appropriate skubox material for the current day
    /// </summary>
    private void UpdateSkyBox()
    {
        Material currentSkybox = null;
        foreach (SkyBoxTimeMapping mapping in timeMappings)
        {
            if (currentHour == mapping.hour)
            {
                currentSkybox = mapping.skyboxMaterial;

                if (currentSkybox.shader != null)
                {
                    if (currentSkybox.shader.name == "Custom/SkyboxTransition")
                    {
                        blendedValue += Time.deltaTime;
                        blendedValue = Mathf.Clamp01(blendedValue);
                        currentSkybox.SetFloat("_TransitionFactor", blendedValue);
                    }
                    else
                    {
                        blendedValue = 0.0f;
                    }
                }

                break;
            }
        }

        if(currentHour == 0 && lockNextDayTrigger == false)
        {
            TimeManager.Instance.TriggerNextDay();
            lockNextDayTrigger = true;
        }

        if (currentHour != 0)
            lockNextDayTrigger = false;

        if (currentSkybox != null)
        {
            RenderSettings.skybox = currentSkybox;
        }
    }
}

[System.Serializable]
public class SkyBoxTimeMapping
{
    public string phaseName;
    public int hour; // the hour of the day (0 - 23)
    public Material skyboxMaterial;
}