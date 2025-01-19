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

    /// <summary>
    /// Go back to main shop menu
    /// </summary>
    private void ExitSellMode()
    {
        if (SellPanelIsEmpty())
        {       
            ShopKeeper.DialogMode();
        }     
    }

    /// <summary>
    /// Zwraca wartoœæ true, jeœli lista przedmiotów do sprzeda¿y jest pusta
    /// </summary>
    /// <returns></returns>
    private bool SellPanelIsEmpty()
    {
        return itemsToBeSold.Count <= 0;
    }

    /// <summary>
    /// Oblicza wartoœæ wszystkich przedmiotów do sprzeda¿y, usuwa je z gry i aktualizuje interfejs
    /// </summary>
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

    /// <summary>
    /// Przegl¹da panel sprzeda¿y, aby zidentyfikowaæ wszystkie sloty
    /// </summary>
    private void GetAllSlots()
    {
        sellSlots.Clear();
        foreach (Transform child in sellPanel.transform)
        {
            if(child.CompareTag("Slot"))
            {
                sellSlots.Add(child.GetComponent<InventorySlot>());             
            }
                
        }
        Debug.Log("Sellpanel slots: "+ sellSlots.Count);
    }

    /// <summary>
    /// Przeszukuje sloty, aby zebraæ przedmioty gotowe do sprzeda¿y
    /// </summary>
    public void ScanItemsInSlots()
    {
        itemsToBeSold.Clear();

        foreach (InventorySlot slot in sellSlots)
        {
            if (slot.itemInSlot != null)
                itemsToBeSold.Add(slot.itemInSlot);
        }
    }

    /// <summary>
    /// Aktualizuje widoczn¹ kwotê sprzeda¿y w interfejsie u¿ytkownika
    /// </summary>
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
