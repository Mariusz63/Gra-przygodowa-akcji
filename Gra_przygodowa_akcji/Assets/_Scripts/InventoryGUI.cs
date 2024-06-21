
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGUI : MonoBehaviour
{
    private InventoryController inventory;
    private List<GameObject> cellList;
    [SerializeField]
    private GameObject gridCell;

    public void SetupInventory (InventoryController _inventory){
        this.inventory = _inventory;
        
    }
    void Start()
    {
        this.cellList = new List<GameObject>();
        SetSizeByLevel(inventory);
    }

    void Update()
    {
        
    }


    public void SetSizeByLevel (InventoryController _inventory){
        int invLevel = _inventory.inventoryLevel;
        int [] invSize = {12, 18, 24, 30};
        int midPointX = 0, midPointY = 0, spacing = 70;

        GameObject clone = Instantiate(gridCell, new Vector3 (1420,540,0f) , Quaternion.identity, transform);

        RectTransform clonetransform = clone.GetComponent<RectTransform>();
        Debug.Log(clonetransform.anchoredPosition);
    }
}
