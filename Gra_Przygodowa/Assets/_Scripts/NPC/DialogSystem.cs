using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Singleton
public class DialogSystem : MonoBehaviour
{
    public static DialogSystem Instance { get; set; }

    public Canvas dialogUI;
    public TextMeshProUGUI dialogText;
    public Button option1;
    public Button option2;

    public bool dialogUIActive;

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

    public void CloseDialogUI()
    {
        dialogUI.gameObject.SetActive(false);
        dialogUIActive = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenDialogUI()
    {
        dialogUI.gameObject.SetActive(true);
        dialogUIActive = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
