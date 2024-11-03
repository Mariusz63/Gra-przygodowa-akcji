using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    public string itemName;

    public string GetItemName()
    {
        return itemName;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject)
        {
            //if the inventory is NOT full
            if (!InventorySystem.Instance.CheckIfFull())
            {
                Debug.Log("Item added to inventory.");
                InventorySystem.Instance.AddToInventory(itemName);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventory is full!!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

        }
    }
}