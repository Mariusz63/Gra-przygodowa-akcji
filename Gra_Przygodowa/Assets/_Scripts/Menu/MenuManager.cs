using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// Singleton
public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; set; }

    public GameObject menuCanvas;
    public GameObject uiCanvas;

    public GameObject saveMenu;
    public GameObject settingsMenu;
    public GameObject menu;
    public Button returnBTN;

    public bool isMenuOpen;
    public int currentFrontLevel = 0;

    // Powoduje ze layout Menu bedzie pokazywany pierwszy (przed innymi UI)
    public int SetAsFront()
    {
        return currentFrontLevel++;
    }

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

    void Start()
    {
        // Przypisanie zdarzenia do przycisku UI
        returnBTN.onClick.AddListener(HandleReturnButton);
    }

    private void Update()
    {
        // TO DO: zmienic na escape aby dzia³a³o po buildzie
        if (Input.GetKeyDown(KeyCode.M) && !isMenuOpen)
        {
            MovementManager.Instance.EnableLook(false);
            MovementManager.Instance.EnableMovement(false);

            saveMenu.SetActive(false);
            settingsMenu.SetActive(false);
            menu.SetActive(true);

            uiCanvas.SetActive(false);
            menuCanvas.SetActive(true);
            IsMenuOpen(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            //SelectionManager jesli bedzie trzeba dodac
        }
        else if (Input.GetKeyDown(KeyCode.M) && isMenuOpen )
        {
            CloseMenu();
        }
    }


    void HandleReturnButton()
    {
        if (isMenuOpen)
        {
            CloseMenu(); // Zamkniêcie menu na klikniêcie przycisku
        }
    }

    void CloseMenu()
    {
        // Logika zamykania menu
        MovementManager.Instance.EnableLook(true);
        MovementManager.Instance.EnableMovement(true);

        uiCanvas.SetActive(true);
        menuCanvas.SetActive(false);
        isMenuOpen = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Menu zamkniête!");
    }

public void IsMenuOpen(bool ex)
    {
        isMenuOpen = ex;
    }

}
