using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGizmos : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 9f); // Attacking Distance

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 18f); // Chasing Distance

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 21f); // Stop chasing Distance
    }
}
