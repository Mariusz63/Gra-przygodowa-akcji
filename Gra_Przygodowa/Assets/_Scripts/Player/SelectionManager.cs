using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//Singleton 
public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; set; }

    public bool onTarget;
    public GameObject selectedObject;   //We want this particular object
    public GameObject interaction_Info_UI;
    Text interaction_text;

    public Image centerDotIcon;
    public Image handIcon;

    public bool handIsVisible; //if hand is visible we wont to swing the tools

    //public GameObject selectedTree;
    public GameObject chopHolder;
    public GameObject selectedStorageBox;

    private void Start()
    {
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<Text>();
    }

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

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            // NPC 
            NPC npc = selectionTransform.GetComponent<NPC>();
            if (npc && npc.playerInRange)
            {
                interaction_text.text = "Talk";
                interaction_Info_UI.SetActive(true);

                if (Input.GetMouseButton(0) && npc.isTalkingWithPlayer == false)
                {
                    npc.StartConversation();
                }

                if (DialogSystem.Instance.dialogUIActive)
                {
                    interaction_Info_UI.SetActive(false);
                    centerDotIcon.gameObject.SetActive(false);
                }
            }

            // Chopp tree
            //ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();

            //if (choppableTree && choppableTree.playerInRange)
            //{
            //    choppableTree.canBeChopped = true;
            //    selectedTree = choppableTree.gameObject;
            //    chopHolder.gameObject.SetActive(true);
            //}
            //else
            //{
            //    if(selectedTree != null)
            //    {
            //        selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
            //        selectedTree = null;
            //        chopHolder.gameObject.SetActive(false);
            //    }
            //}


            // Storage Box
            StorageBox storageBox = selectionTransform.GetComponent<StorageBox>();
            if (storageBox && storageBox.isPlayerInRange && PlacementSystem.Instance.inPlacementMode == false)
            {
                interaction_text.text = "Open";
                interaction_Info_UI.SetActive(true);
                selectedStorageBox = storageBox.gameObject;

                // Check is the player click left button
                if (Input.GetMouseButtonDown(0))
                    StorageManager.Instance.OpenBox(storageBox);
            }
            else
            {
                if (selectedStorageBox != null)
                    selectedStorageBox = null;
            }

            // Animal
            Animal animal = selectionTransform.GetComponent<Animal>();
            if (animal && animal.playerInRange)
            {

                if (animal.isDead)
                {
                    interaction_text.text = "Loot";
                    interaction_Info_UI.SetActive(true);
                    ActiveCenterDot();

                    if (Input.GetMouseButtonDown(0))
                    {
                        Lootable lootable = animal.GetComponent<Lootable>();
                        Loot(lootable);
                    }
                }
                else
                {
                    interaction_text.text = animal.animalName;
                    interaction_Info_UI.SetActive(true);

                    ActiveCenterDot();

                    if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsHoldingWeapon() && EquipSystem.Instance.IsThereASwingLock() == false)
                        StartCoroutine(DealDamage(animal, 0.3f, EquipSystem.Instance.GetWeaponDamage()));
                }
            }

            // Pickable items
            InteractableObject ourInteractable = selectionTransform.GetComponent<InteractableObject>();
            if (ourInteractable && ourInteractable.graczWZasiegu)
            {
                onTarget = true;
                selectedObject = ourInteractable.gameObject;

                interaction_text.text = ourInteractable.GetItemName();
                interaction_Info_UI.SetActive(true);

                ActiveCenterDot();
            }

            if (!npc && !ourInteractable && !storageBox && !animal)
            {
                interaction_text.text = "";
                interaction_Info_UI.SetActive(false);
            }

            if (!ourInteractable && !animal)
            {
                onTarget = false;
                handIsVisible = false;
                centerDotIcon.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);
            }

        }
    }

    // Calculate loot and spawn items on the ground
    private void Loot(Lootable lootable)
    {
        // Calcualte
        if (lootable.wasLootCalculated == false)
        {
            List<LootRecived> recivedLoot = new List<LootRecived>();

            foreach (LootPossibility loot in lootable.possibleLoot)
            {
                // there can be negative numbers to reduce the possibility of a given item 
                // 0 -> 1 (50% drop rate)  1/2   0,1
                // -1 -> 1 (30%)           1/3   -1.0.1
                // -3 -> 1 (20%)           1/5   -3,-2,-1,0,1
                // -3 -> 2 (33% to get something) (1/6 1/6 2/6) -3,-2,-1,0,1(17%),2(17%)

                var lootAmount = UnityEngine.Random.Range(loot.amountMin, loot.amountMax + 1);
                if (lootAmount > 0)
                {
                    LootRecived lootRecived = new LootRecived();
                    lootRecived.item = loot.item;
                    lootRecived.amount = lootAmount;

                    recivedLoot.Add(lootRecived);
                }
            }
            lootable.finalLoot = recivedLoot;
            lootable.wasLootCalculated = true;
        }

        // spawn items on the ground
        Vector3 lootSpawnPosition = lootable.gameObject.transform.position;

        foreach (LootRecived lootRecived in lootable.finalLoot)
        {
            for (int i = 0; i < lootRecived.amount; i++)
            {
                GameObject lootSpawn = Instantiate(Resources.Load<GameObject>(lootRecived.item.name + "_Model"),
                    new Vector3(lootSpawnPosition.x, lootSpawnPosition.y + 0.25f, lootSpawnPosition.z), Quaternion.Euler(0, 0, 0));
            }
        }

        // Destroy body
        DestroyDeadBody(lootable.gameObject);
    }

    IEnumerator DestroyDeadBody(GameObject body)
    {
        yield return new WaitForSeconds(10f);
        Destroy(body);
    }

    private void ActiveCenterDot()
    {
        centerDotIcon.gameObject.SetActive(false);
        handIcon.gameObject.SetActive(true);
        handIsVisible = true;
    }

    IEnumerator DealDamage(Animal animal, float delay, int damage)
    {
        yield return new WaitForSeconds(delay);
        animal.TakeDamage(damage);
    }

    public void DisableSelection()
    {
        handIcon.enabled = false;
        centerDotIcon.enabled = false;
        interaction_Info_UI.SetActive(false);
        interaction_text.text = null;

        selectedObject = null;
    }

    public void EnableSelection()
    {
        handIcon.enabled = true;
        centerDotIcon.enabled = true;
        interaction_Info_UI.SetActive(true);
    }
}