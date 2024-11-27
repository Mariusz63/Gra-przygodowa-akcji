using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool graczWZasiegu;
    public string nazwaPrzedmiotu;

    public string GetItemName()
    {
        return nazwaPrzedmiotu;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && graczWZasiegu && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject)
        {
            //if the inventory is NOT full
            if (!InventorySystem.Instance.CheckIfFull())
            {              
                InventorySystem.Instance.AddToInventory(nazwaPrzedmiotu);
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
            graczWZasiegu = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            graczWZasiegu = false;

        }
    }
}