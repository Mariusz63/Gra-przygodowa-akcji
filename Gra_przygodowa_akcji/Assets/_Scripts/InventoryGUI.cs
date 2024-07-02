
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryGUI : MonoBehaviour, IDropHandler
{
    private InventoryController inventory;
    private GridCell[] gridCellTablice;
    private RectTransform[] gridCellTransform;
    private int selectedGridCells;

    //temporary
    [SerializeField]
    public GameObject swordPrefab;

    public void SetupInventory (InventoryController _inventory){
        this.inventory = _inventory;
        this.selectedGridCells = 0;
    }
    void Start()
    {
        this.gridCellTablice = this.GetComponentsInChildren<GridCell>();
        this.gridCellTransform = this.GetComponentsInChildren<RectTransform>();
        AddItemToInventory();
    }

    void Update()
    {
        if (isActiveAndEnabled)
        {
            selectedGridCells = CountSelectedGridCells(gridCellTablice);
        }
        //Debug.Log("Liczba wybranych: " + selectedGridCells);
    }

    private int CountSelectedGridCells(GridCell[] tab)
    {
        int count = 0;
        foreach (GridCell x in tab)
        {
            if (x.selected == true)
            {
                count++;
            }
        }
        return count;
    }

    public void AddItemToInventory()
    {
        inventory.Additem(new Item(Item.ItemType.Weapon, 1, false, "Potê¿ny miecz wpierdolu"));
        GameObject AddToGUI = Instantiate(swordPrefab, new Vector2(1200,700),Quaternion.identity,transform.parent);
        //AddToGUI = Instantiate(swordPrefab, new Vector2(1200,700),Quaternion.identity,transform.parent);
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DragableItem dragableItem = dropped.GetComponent<DragableItem>();
        dragableItem.transform.position = GetNearestCellPosition(eventData.position);
    }

    public Vector2 GetNearestCellPosition(Vector2 mousePosition)
    {
        Debug.Log(mousePosition);
        float offset_x = 105;
        float offset_y = 105;

        foreach (RectTransform each in gridCellTransform)
        {
            if (each.anchoredPosition == new Vector2(460, 0))
                continue;
            Debug.Log(each.anchoredPosition);
            if (each.position.x < mousePosition.x)
            {
                
            }
        }

        return new Vector2(960, 540);




        /*
        GridLayoutGroup gridLayout = GetComponent<GridLayoutGroup>();
        RectTransform gridRectTransform = gridLayout.GetComponent<RectTransform>();

        Vector2 localMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(gridRectTransform, mousePosition, null, out localMousePosition);

        float cellWidth = gridLayout.cellSize.x;
        float cellHeight = gridLayout.cellSize.y;

        int x = Mathf.FloorToInt((localMousePosition.x - gridRectTransform.rect.xMin) / cellWidth);
        int y = Mathf.FloorToInt((localMousePosition.y - gridRectTransform.rect.yMin) / cellHeight);

        x = Mathf.Clamp(x, 0, gridLayout.constraintCount - 1);
        y = Mathf.Clamp(y, 0, Mathf.FloorToInt(gridRectTransform.rect.height / cellHeight) - 1);

        return new Vector2(x, y);*/
    }
}
