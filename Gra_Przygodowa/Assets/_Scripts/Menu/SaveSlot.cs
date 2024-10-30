using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    private Button button;
    private TextMeshProUGUI buttonText;
    public int slotNumber;

    public GameObject warningUI;
    Button yesBtn;
    Button noBtn;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        yesBtn = warningUI.transform.Find("YesBtn").GetComponent<Button>();
        noBtn = warningUI.transform.Find("NoBtn").GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (SaveManager.Instance.IsSlotEmpty(slotNumber))
            {
                SaveGameConfirmed();
            }
            else
            {
                DisplayOverrideWarning();
            }
        });
    }

    private void Update()
    {
        if (SaveManager.Instance.IsSlotEmpty(slotNumber))
        {
            buttonText.text = "Empty";
        }
        else
        {
            buttonText.text = PlayerPrefs.GetString("Slot" + slotNumber + "Description");
        }
    }

    public void DisplayOverrideWarning()
    {
        warningUI.SetActive(true);

        yesBtn.onClick.AddListener(() =>
        {
            SaveGameConfirmed();
            warningUI.SetActive(false);
        });

        noBtn.onClick.AddListener(() =>
        {
            warningUI.SetActive(false);
        });
    }

    private void SaveGameConfirmed()
    {

        SaveManager.Instance.SaveGame(slotNumber);

        DateTime dateTime = DateTime.Now;
        string time = dateTime.ToString("yyyy-MM-dd HH:mm:ss");

        string description = "Saved game" + slotNumber + " | " + time;

        buttonText.text = description;

        PlayerPrefs.SetString("Slot" + slotNumber + "Description", description);

        SaveManager.Instance.DeselectButton();
    }
}


