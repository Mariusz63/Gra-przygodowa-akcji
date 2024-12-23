using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    //public GameObject Item
    //{
    //    get
    //    {
    //        if (transform.childCount > 0)
    //        {
    //            return transform.GetChild(0).gameObject;
    //        }
    //        return null;
    //    }
    //}

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");

        //if the slot is empty
        if (transform.childCount <=1)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.dropItemSound);

            DragDrop.itemBeingDragged.transform.SetParent(transform);
            DragDrop.itemBeingDragged.transform.localPosition = new Vector2(0, 0);

            if (transform.CompareTag("QuickSlot") == false || transform.CompareTag("Slot"))
            {
                DragDrop.itemBeingDragged.GetComponent<InventoryItem>().isInsideQuickSlot = false;
                InventorySystem.Instance.ReCalculateList();
            }

            if (transform.CompareTag("QuickSlot") || transform.CompareTag("Slot"))
            {
                DragDrop.itemBeingDragged.GetComponent<InventoryItem>().isInsideQuickSlot = true;
                // we need to recalculate list because its no longer inside the system
                InventorySystem.Instance.ReCalculateList();
            }
        }
        else // The slot is not empty
        {
            InventoryItem draggedItem = DragDrop.itemBeingDragged.GetComponent<InventoryItem>();
            // Check if both items are the same and limit is not reached
            if (draggedItem.thisName == GetStoredItem().thisName && IsLimitExceded(draggedItem) == false) 
            {
               // Merge dragged item and stored item
                GetStoredItem().amountInInventory += draggedItem.amountInInventory;
                DestroyImmediate(DragDrop.itemBeingDragged);
            }
            else
            {
                DragDrop.itemBeingDragged.transform.SetParent(transform);
            }
        }
        StartCoroutine(DelayedSacn());
    }

    IEnumerator DelayedSacn()
    {
        yield return new WaitForSeconds(0.3f);
        SellSystem.Instance.ScanItemsInSlots();
        SellSystem.Instance.UpdateSellAmountUI();
    }

    /// <summary>
    /// Retuns stored item
    /// </summary>
    /// <returns></returns>
    InventoryItem GetStoredItem()
    {
        return transform.GetChild(0).GetComponent<InventoryItem>();
    }

    /// <summary>
    /// Check if the limit has been reached
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool IsLimitExceded(InventoryItem item)
    {
        return (item.amountInInventory + GetStoredItem().amountInInventory) > InventorySystem.Instance.stackLimit;
    }
}