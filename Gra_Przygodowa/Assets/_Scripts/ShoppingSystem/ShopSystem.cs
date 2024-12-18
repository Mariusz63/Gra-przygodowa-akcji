using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    public bool playerInRange;
    public bool isTalkingWithPlayer;

    public GameObject shopkepperDialogUI;
    public Button buyBTN;
    public Button sellBTN;
    public Button exitBTN;

    public GameObject sellPanelUI;
    public GameObject buyPanelUI;

    public void Start()
    {
        shopkepperDialogUI.SetActive(false);

        buyBTN.onClick.AddListener(BuyMode);
        sellBTN.onClick.AddListener(SellMode);
        exitBTN.onClick.AddListener(StopTalking);
    }

    public void Talk()
    {
        isTalkingWithPlayer = true;
        DisplayDialogUI();

        MovementManager.Instance.EnableLook(false);
        MovementManager.Instance.EnableMovement(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StopTalking() 
    {
        isTalkingWithPlayer = false;
        HideDialogUI();

        MovementManager.Instance.EnableLook(true);
        MovementManager.Instance.EnableMovement(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void DisplayDialogUI()
    {
        shopkepperDialogUI.SetActive(true);
    }

    private void HideDialogUI()
    {
        shopkepperDialogUI.SetActive(false);
    }

    #region || ------------- Mode Events ----------- ||
    public void SellMode()
    {
        sellPanelUI.SetActive(true);
        buyPanelUI.SetActive(false);
        HideDialogUI();
    }

    public void BuyMode()
    {
        sellPanelUI.SetActive(false);
        buyPanelUI.SetActive(true);
        HideDialogUI();
    }

    public void DialogMode()
    {
        sellPanelUI.SetActive(false);
        buyPanelUI.SetActive(false);
        DisplayDialogUI();
    }
    #endregion

    #region || ------------ On Trigger ------------- ||
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
    #endregion
}
