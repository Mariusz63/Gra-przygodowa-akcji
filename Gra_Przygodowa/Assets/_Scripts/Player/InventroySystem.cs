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
    // public bool isOpen;

    // Pickup Popup alert;
    public GameObject pickupAlert;
    public Text pickupName;
    public Image pickupImage;

    public List<string> pickupItems;

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
        //searching for transforms inside in inventoryScreenUI
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    //void Update()
    //{

    //    if (Input.GetKeyDown(KeyCode.LeftControl) && !isOpen)
    //    {

    //        Debug.Log("i is pressed");
    //        inventoryScreenUI.SetActive(true);
    //        Cursor.lockState = CursorLockMode.None;
    //        Cursor.visible = true;

    //        SelectionManager.Instance.DisableSelection();
    //        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

    //        isOpen = true;

    //    }
    //}

    public void AddToInventory(string itemName)
    {
        whatSlotToEquip = FindNextEmptySlot();

        itemToAdd = (GameObject)Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
        itemToAdd.transform.SetParent(whatSlotToEquip.transform);

        itemList.Add(itemName);

        Sprite sprite = itemToAdd.GetComponent<Image>().sprite;
        TriggerPickupPopUp(itemName, itemToAdd.GetComponent<Image>().sprite);

        ReCalculateList();
        QuestManager.Instance.RefreshTrackerList();
    }

    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
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

    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove;

        for (var i = slotList.Count - 1;i>= 0; i--)
        {
            if (slotList[i].transform.childCount > 0)
            {
                //in inventory we get "clone" 
                if (slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter != 0)
                {
                   Destroy( slotList[i].transform.GetChild(0).gameObject);
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

    // Liczy ile przedmiotow mamy o tej samej nazwie
    public int CheckItemAmount(string name)
    {
        int itemCounter = 0;
        foreach(string item in itemList)
        {
            if(item == name)
            {
                itemCounter++;
            }
        }
        return itemCounter;
    }

}