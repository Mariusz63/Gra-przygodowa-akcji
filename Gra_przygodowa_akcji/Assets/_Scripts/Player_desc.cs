using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_desc : MonoBehaviour
{
    public GameObject inventoryGUI;
    private bool menuActive = false;
    private InventoryController inventory;
    [SerializeField]
    private InventoryGUI uiInventory; 
    void Start()
    {
        inventory = new InventoryController();
        uiInventory.SetupInventory(inventory);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Open Inventory") && !menuActive){
            inventoryGUI.SetActive(true);
            menuActive = true;
        }
        else if (Input.GetButtonDown("Open Inventory") && menuActive){
            inventoryGUI.SetActive(false);
            menuActive = false;
        }
    }
}
