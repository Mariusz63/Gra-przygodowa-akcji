using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Singleton
public class StorageManager : MonoBehaviour
{
    public static StorageManager Instance { get; set; }

    [SerializeField] GameObject storageBoxSmallUI;
    // TO DO: add medium and large StorageBox
    [SerializeField] StorageBox selectedStorage;
    public bool storageUIOpen;

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

    public void OpenBox(StorageBox storage)
    {
        MovementManager.Instance.EnableLook(false);
        MovementManager.Instance.EnableMovement(false);
        SetSelectedStorage(storage);

        PopulateStorage(GetRelevantUI(selectedStorage));

        GetRelevantUI(selectedStorage).SetActive(true);
        storageUIOpen = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SelectionManager.Instance.DisableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
    }

    private void PopulateStorage(GameObject storageUI)
    {
        // Get all slots of the ui
        List<GameObject> uiSlots = new List<GameObject>();

        foreach (Transform child in storageUI.transform)
        {
            uiSlots.Add(child.gameObject);
        }

        // Now, instantiate the prefab and set it as a child of each GameObject
        foreach (string name in selectedStorage.items)
        {
            foreach (GameObject slot in uiSlots)
            {
                if (slot.transform.childCount < 1)
                {
                    var itemToAdd = Instantiate(Resources.Load<GameObject>(name), slot.transform.position, slot.transform.rotation);
                    itemToAdd.name = name;
                    itemToAdd.transform.SetParent(slot.transform);
                    break;
                }
            }
        }
    }

    public void CloseBox()
    {
        MovementManager.Instance.EnableLook(true);
        MovementManager.Instance.EnableMovement(true);

        RecalculateStorage(GetRelevantUI(selectedStorage));

        GetRelevantUI(selectedStorage).SetActive(false);
        storageUIOpen = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SelectionManager.Instance.EnableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
    }

    private void RecalculateStorage(GameObject gameObject)
    {
        // Get all slots fro mthe UI
        List<GameObject> uiSlots = new List<GameObject>();

        foreach (Transform child in gameObject.transform)
        {
            uiSlots.Add(child.gameObject);
        }

        // Clear list of items
        selectedStorage.items.Clear();

        List<GameObject> toBeDeleted = new List<GameObject>();

        // Take the inventory items and convert them into strings
        foreach (GameObject slot in uiSlots)
        {
            if (slot.transform.childCount > 0)
            {
                string result = RemoveCloneText(slot);
                selectedStorage.items.Add(result);
                toBeDeleted.Add(slot.transform.GetChild(0).gameObject);
            }
        }

        foreach (GameObject item in toBeDeleted)
        {
            Destroy(item);
        }
    }

    // Remove "Clone" text
    private string RemoveCloneText(GameObject gameObject)
    {       
        string name = gameObject.transform.GetChild(0).name;
        string str = "Clone";
        return name.Replace(str, "");
    }

    public void SetSelectedStorage(StorageBox storage)
    {
        selectedStorage = storage;
    }

    private GameObject GetRelevantUI(StorageBox storage)
    {
        //TO DO: Create a switch for other types
        return storageBoxSmallUI;
    }
}
