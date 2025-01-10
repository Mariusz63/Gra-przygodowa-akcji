using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SaveManager;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;
    public Button saveButton;

    public Slider masterSlider;
    public GameObject masterVolume;

    public Slider musicSlider;
    public GameObject musicVolume;

    public Slider effectsSlider;
    public GameObject effectsVolume;

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
        saveButton.onClick.AddListener(() =>
        {
            SaveManager.Instance.SaveVolumeSettings(musicSlider.value, effectsSlider.value ,masterSlider.value);
        });

        StartCoroutine(LoadAndApplySettings());
    }

    private IEnumerator LoadAndApplySettings()
    {
        LoadAndSetVolume();
        //LoadGraphicsSettings
        //LoadKeyBinds();

        yield return new WaitForSeconds(0.1f);
    }

    private void LoadKeyBinds()
    {
        throw new NotImplementedException();
    }

    private void LoadAndSetVolume()
    {
        VolumeSettings volumeSettings = SaveManager.Instance.LoadVolumeSettings();
        masterSlider.value = volumeSettings.master;
        musicSlider.value = volumeSettings.music;
        effectsSlider.value = volumeSettings.effects;
    }

    private void Update()
    {
        masterVolume.GetComponent<TextMeshProUGUI>().text = "" + (masterSlider.value) + "";
        musicVolume.GetComponent<TextMeshProUGUI>().text = "" + (musicSlider.value) + "";
        effectsVolume.GetComponent<TextMeshProUGUI>().text = "" + (effectsSlider.value) + "";
    }

}
