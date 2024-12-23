using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    // --- Is this item trashable --- //
    [Header("Trashable")]
    public bool isTrashable;

    // --- Item Info UI --- //
    [Header("Item Info")]
    private GameObject itemInfoUI;

    private Text itemInfoUI_itemName;
    private Text itemInfoUI_itemDescription;
    private Text itemInfoUI_itemFunctionality;

    public string thisName, thisDescription, thisFunctionality;

    // --- Consumption --- //
    [Header("Consumption")]
    private GameObject itemPendingConsumption;
    public bool isConsumable;

    public float healthEffect;
    public float staminaEffect;

    // --- Equipping --- //
    [Header("Equipping")]
    public bool isEquippable;
    private GameObject itemPendingEquipping;
    public bool isInsideQuickSlot; // is inside the quickSlot
    public bool isSelected; // item we actually selected
    public bool isUsable; // is usable

    public int amountInInventory = 1;

    [Header("Shopping")]
    public int sellingPrice;

    private void Start()
    {
        itemInfoUI = InventorySystem.Instance.itemInfoUI;
        itemInfoUI_itemName = itemInfoUI.transform.Find("itemName").GetComponent<Text>();
        itemInfoUI_itemDescription = itemInfoUI.transform.Find("itemDescription").GetComponent<Text>();
        itemInfoUI_itemFunctionality = itemInfoUI.transform.Find("itemFunctionality").GetComponent<Text>();
    }

    void Update()
    {
        if (isSelected)
        {
            gameObject.GetComponent<DragDrop>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<DragDrop>().enabled = true;

        }
    }

    /// <summary>
    /// Triggered when the mouse enters into the area of the item that has this script.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        itemInfoUI.SetActive(true);
        itemInfoUI_itemName.text = thisName;
        itemInfoUI_itemDescription.text = thisDescription;
        itemInfoUI_itemFunctionality.text = thisFunctionality;
    }

    /// <summary>
    /// Triggered when the mouse exits the area of the item that has this script.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfoUI.SetActive(false);
    }

    /// <summary>
    /// Triggered when the mouse is clicked over the item that has this script.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        //Right Mouse Button Click on
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable)
            {
                // Setting this specific gameobject to be the item we want to destroy later
                itemPendingConsumption = gameObject;
                consumingFunction(healthEffect, staminaEffect);
            }

            if (isEquippable && isInsideQuickSlot == false && EquipSystem.Instance.CheckIfFull() == false)
            {
                EquipSystem.Instance.AddToQuickSlots(gameObject);
                isInsideQuickSlot = true;
            }

            if (isUsable)
            {
                UseItem();
            }
        }
    }

    private void UseItem()
    {
        // Closing all opened UI / Menus
        itemInfoUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SelectionManager.Instance.EnableSelection();
        SelectionManager.Instance.enabled = true;

        // we need to add (Clone) to name because after adding item to inventory become "clone"
        switch (gameObject.name)
        {
            case "StorageBox(Clone)":
                PlacementSystem.Instance.inventoryItemToDestory = gameObject;
                PlacementSystem.Instance.ActivatePlacementMode("StorageBox_Model");
                break;
            default: break; // do nothing;
        }
    }

    /// <summary>
    /// Triggered when the mouse button is released over the item that has this script.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable && itemPendingConsumption == gameObject)
            {
                DestroyImmediate(gameObject);
                InventorySystem.Instance.ReCalculateList();
                //CraftingSystem.Instance.RefreshNeededItems();
            }
        }
    }

    private void consumingFunction(float healthEffect, float staminaEffect)
    {
        itemInfoUI.SetActive(false);
        healthEffectCalculation(healthEffect);
        staminaEffectCalculation(staminaEffect);
    }

    /// <summary>
    /// Calcualte player health 
    /// </summary>
    /// <param name="healthEffect"></param>
    private static void healthEffectCalculation(float healthEffect)
    {
        float healthBeforeConsumption = PlayerState.Instance.currentHealth;
        float maxHealth = PlayerState.Instance.maxHealth;

        if (healthEffect != 0)
        {
            if ((healthBeforeConsumption + healthEffect) > maxHealth)
            {
                PlayerState.Instance.setHealth(maxHealth);
            }
            else
            {
                PlayerState.Instance.setHealth(healthBeforeConsumption + healthEffect);
            }
        }
    }

    /// <summary>
    /// Calculate player stamina
    /// </summary>
    /// <param name="caloriesEffect"></param>
    private static void staminaEffectCalculation(float caloriesEffect)
    {
        float staminaBeforeConsumption = PlayerState.Instance.currentStamina;
        float maxStamina = PlayerState.Instance.maxStamina;

        if (caloriesEffect != 0)
        {
            if ((staminaBeforeConsumption + caloriesEffect) > maxStamina)
            {
                PlayerState.Instance.setCalories(maxStamina);
            }
            else
            {
                PlayerState.Instance.setCalories(staminaBeforeConsumption + caloriesEffect);
            }
        }
    }
}
