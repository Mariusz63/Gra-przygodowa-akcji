using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent agent;
    public float stopAttackingDistance = 3f;

    [Header("Attacking")]
    public float attackRate = 2f; // attack each second
    private float attackTimer;
    public int damageToInflict = 12; // hitpoint per second

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ------ Initialization -------- //
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        LookAtPlayer();

        if (attackTimer < 0 && (PlayerState.Instance.currentHealth > 0))
        {
            AttackPlayer();
            attackTimer = 1f / attackRate;
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }

        // --- CHeck if agent should stop atatcking ---- //
        float distanceFromPlayer = Vector3.Distance(player.position, agent.transform.position);
        if ((distanceFromPlayer > stopAttackingDistance) || (PlayerState.Instance.currentHealth <= 0))
        {
            animator.SetBool("isAttacking", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //}

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0,yRotation, 0);
    }

    private void AttackPlayer()
    {
        agent.gameObject.GetComponent<Animal>().PlayAttackSound();
        PlayerState.Instance.TakeDamage(damageToInflict);
    }
}
