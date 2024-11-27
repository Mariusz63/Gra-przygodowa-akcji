using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{
    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && // Left Mouse Button        
            QuestManager.Instance.isQuestMenuOpen == false &&
            SelectionManager.Instance.handIsVisible == false) 
        {
            animator.SetTrigger("hit");
            SwingSoundDelay();
        }
    }

    //trigger this code in animator
    public void GetHitAnim()
    {
        //GameObject selectedTree = SelectionManager.Instance.selectedTree;
        //if (selectedTree != null)
        //{
        //    selectedTree.GetComponent<ChoppableTree>().GetHit();
        //}
    }

    IEnumerator SwingSoundDelay()
    {
        yield return new WaitForSeconds(0.2f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);
    }
}
