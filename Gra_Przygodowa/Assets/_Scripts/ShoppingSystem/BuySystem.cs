using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuySystem : MonoBehaviour
{
    #region || -- Singelton -- ||
    public static BuySystem Instance { get; set; }
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

    [Header("ShopKeeper")]
    public ShopSystem ShopKeeper;

    [Header("UI")]
    public Transform contentTransform;
    public GameObject shopItemPrefab;
    public Button backButton;

    [Header("Current List")]
    public List<ShopItemData> currentShopList;

    // TO DO: uncomment this when we want to add event shop list
   // [Header("Optional Season Lists")]
    //public List<ShopItemData> springList = new List<ShopItemData>();

    void Start()
    {
        backButton.onClick.AddListener(ExitBuyMode);

        // Set list of items based on seasons / events
        //if (TimeManager.Instance.currentSeason == TimeManager.Season.Spring)
        //{
        //    // Set spring list as our list
        //    currentShopList = springList;

        //    // Initialize list
        //    InitializeBuyList(currentShopList);
        //}
        InitializeBuyList(currentShopList);
    }

    private void InitializeBuyList(List<ShopItemData> shopList)
    {
        foreach (ShopItemData listItem in shopList)
        {
            GameObject prefab = Instantiate(shopItemPrefab, contentTransform);

            ShopItemSlot shopItemSlot = prefab.GetComponent<ShopItemSlot>();

            InventoryItem inventoryItem = listItem.inventoryItem.GetComponent<InventoryItem>();

            Debug.Log("Shop item:" + inventoryItem.thisName);
            // Set the actual data
            shopItemSlot.shopItemData = listItem;
            // Setting the Name
            shopItemSlot.itemNameUI.text = inventoryItem.thisName;
            // Setting the Sprite
            shopItemSlot.itemImageUI.sprite = listItem.inventoryItem.GetComponent<Image>().sprite;
            // Setting the Price
            shopItemSlot.itemPriceUI.text = $"{listItem.itemPrice}";
        }
    }

    private void ExitBuyMode()
    {
        ShopKeeper.DialogMode();
    }

    [System.Serializable]
    public class ShopItemData
    {
        public GameObject inventoryItem;
        public int itemPrice;
    }
}
