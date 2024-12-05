using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Singleton
public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;
    public GameObject itemInfoUI;

    // Contain all slots
    public List<GameObject> slotList = new List<GameObject>();
    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;

    public bool isFull;

    // Pickup Popup alert;
    public GameObject pickupAlert;
    public Text pickupName;
    public Image pickupImage;

    public List<string> pickupItems;
    // TO DO: hardcoded, in future add upgrade
    public int stackLimit = 3;

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
        //isOpen = false;

        PopulateSlotList();
        Cursor.visible = false;
    }

    private void PopulateSlotList()
    {
        // Pobierz wszystkie obiekty z tagiem "QuickSlot" w hierarchii InventoryScreenUI
        foreach (Transform child in inventoryScreenUI.GetComponentsInChildren<Transform>(true)) // true uwzglêdnia nieaktywne obiekty
        {
            if (child.CompareTag("QuickSlot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    public void AddToInventory(string itemName)
    {
        Debug.Log("Dodanie do eq");

        GameObject stack = CheckIfStackExists(itemName);

        if (stack != null)
        {
            Debug.Log("Dodanie do eq - stack");
            //Add one to existing items
            stack.GetComponent<InventorySlot>().itemInSlot.amountInInventory++;
            stack.GetComponent<InventorySlot>().UpdateItemInSlot();
        }
        else
        {
            Debug.Log("Dodanie do eq - pojedynczy");
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
        Debug.Log("Dodanie do eq - " + itemName + "Koniec");
    }

    private GameObject CheckIfStackExists(string itemName)
    {
        foreach (GameObject slot in slotList)
        {
            InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
            inventorySlot.UpdateItemInSlot();

            if(inventorySlot != null && inventorySlot.itemInSlot != null)
            {
                if (inventorySlot.itemInSlot.thisName == itemName &&
                    inventorySlot.itemInSlot.amountInInventory < stackLimit)
                    return slot;
            }
        }
        return null;
    }

    private GameObject FindNextEmptySlot()
    {
        Debug.Log("Szukanie wolnego slota");
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount <= 1)
            {
                return slot;
            }
        }
        return new GameObject();
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

    public bool CheckIfFull()
    {
        int counter = 0;

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }

        if (counter == slotList.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckSlotsAvailable(int emptyNeeded)
    {
        int emptySlot = 0;

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount <= 1)
            {
                emptySlot += 1;
            }
        }

        if (emptySlot >= emptyNeeded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Usuniecie itemka z ekwipunku
    /// </summary>
    /// <param name="nameToRemove"></param>
    /// <param name="amountToRemove"></param>
    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove;

        for (var i = slotList.Count - 1; i >= 0; i--)
        {
            if (slotList[i].transform.childCount > 0)
            {
                // w ekwipunkyu mamy nazwe przedmiotu z dopiskiem "clone" 
                if (slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter != 0)
                {
                    Destroy(slotList[i].transform.GetChild(0).gameObject);
                    counter -= 1;
                }
            }
        }
        ReCalculateList();
        QuestManager.Instance.RefreshTrackerList();
    }

    public void ReCalculateList()
    {
        itemList.Clear();

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name; //get item clone for ex: Stone (Clone)
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");

                itemList.Add(result);
            }
        }
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
