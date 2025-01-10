using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGizmos : MonoBehaviour
{
    public float attackDistance = 9f;
    public float chasingDistance = 18f;
    public float stopChasingDistance = 21f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance); // Attacking Distance

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chasingDistance); // Chasing Distance

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stopChasingDistance); // Stop chasing Distance
    }
}
