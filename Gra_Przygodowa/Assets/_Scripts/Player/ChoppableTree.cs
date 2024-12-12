using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ChoppableTree : MonoBehaviour
{
    public bool playerInRange;
    public bool canBeChopped;

    public float treeMaxHealth;
    public float treeHealth;

    public Animator animator;

    public float staminaSpentChoppingWood = 20;

    private void Start()
    {
        treeHealth = treeMaxHealth;
        //Get Animator component from grandparent 
        animator = transform.parent.transform.parent.GetComponent<Animator>();
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

    public void GetHit()
    {
        animator.SetTrigger("shake");
        treeHealth -= 5;

        PlayerState.Instance.currentStamina -= staminaSpentChoppingWood;

        if (treeHealth <= 0)
        {
            TreeDead();
        }
    }

    void TreeDead()
    {
        Vector3 treePosition = transform.position;

        Destroy(transform.parent.transform.parent.gameObject);
        canBeChopped = false;
        SelectionManager.Instance.selectedTree = null;
        SelectionManager.Instance.chopHolder.gameObject.SetActive(false);

        GameObject brokenTree = Instantiate(Resources.Load<GameObject>("ChoppedTree"), 
            new Vector3(treePosition.x, treePosition.y + 1, treePosition.z), Quaternion.Euler(0,0,0));

    }

    private void Update()
    {
        if (canBeChopped)
        {
            GlobalState.Instance.resourceHealth = treeHealth;
            GlobalState.Instance.resourceMaxHealth = treeMaxHealth;
        }
    }
}
