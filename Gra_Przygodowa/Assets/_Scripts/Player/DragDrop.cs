using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f;
        //So the ray cast will ignore the item itself.
        canvasGroup.blocksRaycasts = false;
        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(transform.root);
        itemBeingDragged = gameObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //So the item will move with our mouse (at same speed)  and so it will be consistant if the canvas has a different scale (other then 1);
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var tempItemReference = itemBeingDragged;
        itemBeingDragged = null;

        // Dragged item outside of inventory 
        if (tempItemReference.transform.parent == tempItemReference.transform.root)
        {
            // Hide the icon of the item at this point
            CancelDragging(tempItemReference);
        }

        // Dropped in the same slot
        if (tempItemReference.transform.parent == startParent)
        {
            CancelDragging(tempItemReference);
        }

        // Dropped in another slot
        if (tempItemReference.transform.parent != tempItemReference.transform.root &&
            tempItemReference.transform.parent != startParent)
        {
            // Other slot did not accept item (Probably different item)
            if (tempItemReference.transform.parent.childCount > 2)
            {
                CancelDragging(tempItemReference);
                Debug.Log("Was not accepted into this slot");
            }
            else // item was moved to othe slot
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    DivideStack(tempItemReference);
                }
                Debug.Log("Should moved into other slot");
            }
        }

        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    private void DivideStack(GameObject tempItemReference)
    {
        // Optional - Activate UI with division logic (slider, user input)

        InventoryItem item = tempItemReference.GetComponent<InventoryItem>();
        // Check if item/stack has move than 1 item
        if (item.amountInInventory > 1)
        {
            // Add new item to inventory
            item.amountInInventory--;
            InventorySystem.Instance.AddToInventory(item.thisName, false); // false because we dont wont stack itmes
        }
    }

    void CancelDragging(GameObject tempItemReference)
    {
        transform.position = startPosition;
        transform.SetParent(startParent);
        tempItemReference.SetActive(true);
    }

    private void DropItemIntoTheWorld(GameObject tempItemReference)
    {
        // Get clean name
        string cleanName = tempItemReference.name.Split(new string[] { "(Clone)" }, StringSplitOptions.None)[0];

        // Instantiate item
        GameObject item = Instantiate(Resources.Load<GameObject>(cleanName + "_Model"));
    }
}