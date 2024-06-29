
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryGUI : MonoBehaviour
{
    private InventoryController inventory;
    private GridCell[] gridCellTablice;
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
        inventory.Additem(new Item(Item.ItemType.Weapon, 1, false, "Pot�ny miecz wpierdolu"));
        GameObject AddToGUI = Instantiate(swordPrefab, new Vector2(1200,700),Quaternion.identity,transform.parent);
    }

    public void OnDrop(PointerEventData eventData)
    {
        SpriteShowHandler draggableItem = eventData.pointerDrag.GetComponent<SpriteShowHandler>();

        if (draggableItem != null)
        {
            // Tutaj mo�na doda� logik� przyci�gania przedmiotu do kom�rki
            RectTransform invPanel = transform as RectTransform;

            if (RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
            {
                draggableItem.GetComponent<RectTransform>().anchoredPosition = GetNearestCellPosition(draggableItem.GetComponent<RectTransform>());
                // Zaktualizuj stan kom�rki ekwipunku
            }
        }
    }
    private Vector2 GetNearestCellPosition(RectTransform item)
    {
        // Logika znajdowania najbli�szej kom�rki
        // To mo�na zaimplementowa� na r�ne sposoby, np. iteruj�c po wszystkich kom�rkach
        // i znajduj�c najbli�sz� kom�rk� do pozycji upuszczenia przedmiotu

        return new Vector2(0, 0);
    }

}
