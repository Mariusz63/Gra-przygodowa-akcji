using System;
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

    public float staminaSpentChoppingWood = 2;
    public GameObject spawnChoppedTree;

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

    public void GetHit(float damage)
    {
        StartCoroutine(hit(damage));
    }

    public IEnumerator hit(float damage)
    {
        Debug.Log("Damage: " + damage);
        yield return new WaitForSeconds(0.6f);

        animator.SetTrigger("shake");
        treeHealth -= damage;

        PlayerState.Instance.currentStamina -= staminaSpentChoppingWood;

        if (treeHealth <= 0)
        {
            TreeDead();
        }
    }

    void TreeDead()
    {
        Vector3 treePosition = spawnChoppedTree.transform.position;

        Destroy(transform.parent.transform.parent.gameObject);
        canBeChopped = false;
        SelectionManager.Instance.selectedTree = null;
        SelectionManager.Instance.chopHolder.gameObject.SetActive(false);

        GameObject brokenTree = Instantiate(Resources.Load<GameObject>("ChoppedTree"),
            new Vector3(treePosition.x, treePosition.y, treePosition.z), Quaternion.Euler(0, 0, 0));

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
