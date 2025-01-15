using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    public string itemName;
    public float detectionRange = 6;

    private KeyCode interactionKey = SettingsManager.Instance.GetKeyCode("GetItem");

    public string GetItemName()
    {
        return itemName;
    }

    private void Update()
    {
        float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);
        if(distance < detectionRange)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }

        if (Input.GetKeyDown(interactionKey) && 
            playerInRange && 
            SelectionManager.Instance.onTarget && 
            SelectionManager.Instance.selectedObject == gameObject)
        {
            //if the inventory is NOT full
            if (InventorySystem.Instance.CheckSlotsAvailable(1))
            {              
                InventorySystem.Instance.AddToInventory(itemName,true);
                InventorySystem.Instance.pickupItems.Add(gameObject.name); 
                Destroy(gameObject);
                Debug.Log("Item added to inventory.");
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