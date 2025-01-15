using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Singleton
public class InventorySystem : MonoBehaviour
{
    #region || ---------------- Singleton --------------- ||
    public static InventorySystem Instance { get; set; }

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

    public GameObject inventoryScreenUI;
    public GameObject itemInfoUI;

    // Contain all slots
    public List<InventorySlot> slotList = new List<InventorySlot>();
    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;
    private InventorySlot whatSlotToEquip;

    public bool isFull;
    public bool isOpen;

    // Pickup Popup alert;
    [Header("Pickup Popup alert")]
    public GameObject pickupAlert;
    public Text pickupName;
    public Image pickupImage;

    public List<string> pickupItems;

    // TO DO: hardcoded, in future add upgrade
    public int stackLimit = 5;

    // For shopping, actual amount of coins
    [Header("Shopping")]
    public int currentCoins = 0;
    public TextMeshProUGUI currencyUI;

    private KeyCode inventoryKey;

    void Start()
    {  
        isOpen = false;
        //MovementManager.Instance.canLookAround = false;

        PopulateSlotList();
        //Cursor.visible = false;
        inventoryKey = SettingsManager.Instance.GetKeyCode("Inventory");
        Debug.Log("Inveointory key = " + inventoryKey);
    }

    private void Update()
    {
        if (Input.GetKeyDown(inventoryKey) && !isOpen)
        {
            MovementManager.Instance.EnableLook(false);
            MovementManager.Instance.EnableMovement(false);     
            OpenUI();
        }
        else if (Input.GetKeyDown(inventoryKey) && isOpen)
        {
            MovementManager.Instance.EnableLook(true);
            MovementManager.Instance.EnableMovement(true);            
            CloseUI();
        }
        currencyUI.text = $"{currentCoins}";
    }

    private void CloseUI()
    {
        //inventoryScreenUI.SetActive(false);

        if (!StorageManager.Instance.storageUIOpen && 
            !BuySystem.Instance.ShopKeeper.isTalkingWithPlayer)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            
        }
        isOpen = false;
    }

    private void OpenUI()
    {
        //inventoryScreenUI.SetActive(true);
        //inventoryScreenUI.GetComponent<Canvas>().sortingOrder = MenuManager.Instance.SetAsFront();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SelectionManager.Instance.DisableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
        isOpen = true;
        ReCalculateList();
    }

    private void PopulateSlotList()
    {
        // Pobierz wszystkie obiekty z tagiem "QuickSlot" w hierarchii InventoryScreenUI
        foreach (Transform child in inventoryScreenUI.GetComponentsInChildren<Transform>(true)) // true uwzglêdnia nieaktywne obiekty
        {
            if (child.CompareTag("QuickSlot") || child.CompareTag("Slot"))
            {
                InventorySlot slot = child.GetComponent<InventorySlot>();
                slotList.Add(slot);            
            }
        }
    }

    public void AddToInventory(string itemName, bool shouldStack)
    {
        InventorySlot stack = CheckIfStackExists(itemName);

        if (stack != null && shouldStack)
        {
            //Add one to existing items
            stack.itemInSlot.amountInInventory++;
            stack.UpdateItemInSlot();
        }
        else
        {      
            whatSlotToEquip = FindNextEmptySlot();

            itemToAdd = (GameObject)Instantiate(Resources.Load<GameObject>(itemName),
                whatSlotToEquip.transform.position,
                whatSlotToEquip.transform.rotation);
            itemToAdd.transform.SetParent(whatSlotToEquip.transform);

            itemList.Add(itemName);
        }

        //Sprite sprite = itemToAdd.GetComponent<Image>().sprite;
        TriggerPickupPopUp(itemName, itemToAdd.GetComponent<Image>().sprite);

        ReCalculateList();
        QuestManager.Instance.RefreshTrackerList();
    }

    private InventorySlot CheckIfStackExists(string itemName)
    {
        foreach (InventorySlot inventorySlot in slotList)
        {
            inventorySlot.UpdateItemInSlot();

            if (inventorySlot != null && inventorySlot.itemInSlot != null)
            {
                if (inventorySlot.itemInSlot.thisName == itemName &&
                    inventorySlot.itemInSlot.amountInInventory < stackLimit)
                    return inventorySlot;
            }
        }
        return null;
    }

    private InventorySlot FindNextEmptySlot()
    {
        Debug.Log("Szukanie wolnego slota");
        foreach (InventorySlot slot in slotList)
        {
            if (slot.transform.childCount <= 1)
            {
                return slot;
            }
        }
        return new InventorySlot();
    }

    void TriggerPickupPopUp(string itemName, Sprite itemSprite)
    {
        pickupAlert.SetActive(true);
        pickupImage.sprite = itemSprite;
        pickupName.text = itemName;

        Invoke("PopupDelay", 1.0f);
    }

    public void PopupDelay()
    {
        pickupAlert.SetActive(false);
    }

    //public bool CheckIfFull()
    //{
    //    int counter = 0;

    //    foreach (InventorySlot slot in slotList)
    //    {
    //        if (slot.transform.childCount > 0)
    //        {
    //            counter += 1;
    //        }
    //    }

    //    if (counter == slotList.Count)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    public bool CheckSlotsAvailable(int emptyNeeded)
    {
        int emptySlot = 0;

        foreach (InventorySlot slot in slotList)
        {
            if (slot.transform.childCount <= 1)
            {
                emptySlot += 1;
            }
        }
        return emptySlot >= emptyNeeded;

        //if (emptySlot >= emptyNeeded)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
    }


    public void RemoveItem(string itemName, int amountToRemove)
    {
        int remainingAmountToRemove = amountToRemove;

        // Iterate over the amount to remove
        while (remainingAmountToRemove != 0)
        {
            int previousRemainingAmount = remainingAmountToRemove;
            // Iterate over onventory slots
            foreach (InventorySlot slot in slotList)
            {
                //If the slot is not empty and contains the item we want to remove
                if (slot.itemInSlot != null && slot.itemInSlot.thisName == itemName)
                {
                    // Decrese the amount of the item in the slot
                    slot.itemInSlot.amountInInventory--;
                    remainingAmountToRemove--;

                    // Remove item from slot if its amount become zero
                    if (slot.itemInSlot.amountInInventory == 0)
                    {
                        Destroy(slot.itemInSlot.gameObject);
                        slot.itemInSlot = null;
                    }
                    break;
                }
            }

            // This should never happen, but we should check this to prevent an endless loop while in debug
            if (previousRemainingAmount == remainingAmountToRemove)
            {
                Debug.Log("Item not found or insufficient quantity in inventory");
                break;
            }

            ReCalculateList();
            QuestManager.Instance.RefreshTrackerList();
            QuestManager.Instance.RefreshQuestList();

        }
    }

    /// <summary>
    /// Usuniecie itemka z ekwipunku
    /// </summary>
    /// <param name="nameToRemove"></param>
    /// <param name="amountToRemove"></param>
    //public void RemoveItem(string nameToRemove, int amountToRemove)
    //{
    //    int counter = amountToRemove;

    //    for (var i = slotList.Count - 1; i >= 0; i--)
    //    {
    //        if (slotList[i].transform.childCount > 0)
    //        {
    //            // w ekwipunkyu mamy nazwe przedmiotu z dopiskiem "clone" 
    //            if (slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter != 0)
    //            {
    //                Destroy(slotList[i].transform.GetChild(0).gameObject);
    //                counter -= 1;
    //            }
    //        }
    //    }
    //    ReCalculateList();
    //    QuestManager.Instance.RefreshTrackerList();
    //}

    public void ReCalculateList()
    {
        itemList.Clear();

        foreach (InventorySlot slot in slotList)
        {
            InventoryItem item = slot.itemInSlot;
            if (item != null && item.amountInInventory > 0)
            {
                for (int i = 0; i < item.amountInInventory; i++)
                {
                    itemList.Add(item.thisName);
                }
            }
        }

        //itemList.Clear();
        //foreach (GameObject slot in slotList)
        //{
        //    if (slot.transform.childCount > 0)
        //    {
        //        string name = slot.transform.GetChild(0).name; //get item clone for ex: Stone (Clone)
        //        string str2 = "(Clone)";
        //        string result = name.Replace(str2, "");

        //        itemList.Add(result);
        //    }
        //}
    }

    /// <summary>
    /// Liczy ile przedmiotow mamy o tej samej nazwie
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public int CheckItemAmount(string name)
    {
        int itemCounter = 0;
        foreach (string item in itemList)
        {
            if (item == name)
            {
                itemCounter++;
            }
        }
        return itemCounter;
    }

}
