using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellSystem : MonoBehaviour
{
    #region || -- Singelton -- ||
    public static SellSystem Instance { get; set; }
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

    public Button sellBTN;
    public Button backBTN;
    public TextMeshProUGUI sellAmountTXT;

    public List<InventorySlot> sellSlots;
    public List<InventoryItem> itemsToBeSold;

    public GameObject sellPanel;

    [Header("ShopKeeper")]
    public ShopSystem ShopKeeper;

    private void Start()
    {
        GetAllSlots();

        sellBTN.onClick.AddListener(SellItems);
        backBTN.onClick.AddListener(ExitSellMode);
    }

    private void ExitSellMode()
    {
        if (SellPanelIsEmpty())
        {
            //Go back to main shop menu
            ShopKeeper.DialogMode();
        }
        
    }

    private bool SellPanelIsEmpty()
    {
        return itemsToBeSold.Count <= 0;
    }


    private void SellItems()
    {
        List<GameObject> itemsToDestroy = new List<GameObject>();
        int moneyToBeEarned = 0;
        foreach (InventoryItem item in itemsToBeSold)
        {
            itemsToDestroy.Add(item.gameObject);
            moneyToBeEarned += (item.amountInInventory * item.sellingPrice);
        }
        InventorySystem.Instance.currentCoins += moneyToBeEarned;
        foreach (GameObject ob in itemsToDestroy)
        {
            Destroy(ob);
        }

        itemsToDestroy.Clear();
        itemsToBeSold.Clear();
        UpdateSellAmountUI();
    }

    private void GetAllSlots()
    {
        sellSlots.Clear();
        foreach (Transform child in sellPanel.transform)
        {
            if(child.CompareTag("Slot"))
                sellSlots.Add(child.GetComponent<InventorySlot>());
        }
    }

    public void ScanItemsInSlots()
    {
        itemsToBeSold.Clear();
        foreach (InventorySlot slot in sellSlots)
        {
            if (slot.itemInSlot != null)
                itemsToBeSold.Add(slot.itemInSlot);
        }
    }

    public void UpdateSellAmountUI()
    {
        int totalPriceToDisplay = 0;
        foreach (InventoryItem item in itemsToBeSold)
        {
            totalPriceToDisplay += (item.amountInInventory * item.sellingPrice);
        }
        sellAmountTXT.text = totalPriceToDisplay.ToString();
    }
}
