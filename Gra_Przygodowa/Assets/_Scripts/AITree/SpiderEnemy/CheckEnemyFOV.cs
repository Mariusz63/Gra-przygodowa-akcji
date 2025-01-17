using BTree;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckEnemyFOV : Node
{
    private static int playerLayer = 1 << 0;
    //private static LayerMask playerLayer = 0;

    private Transform transform;
    private float fovRadius;
    private BTBlackboard bb;

    public CheckEnemyFOV(BTBlackboard bb)
    {
        this.bb = bb;
        transform = bb.enemyTransform;
        fovRadius = bb.FOVradius;
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");
        if (t == null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, fovRadius, playerLayer);
            foreach (var col in colliders)
            {
                if (col.CompareTag("Player"))
                {
                    parent.parent.SetData("target", col.transform);
                    Debug.Log("Gracz - zaczynam poœcig.");
                    return NodeState.SUCCESS;
                }
            }

            return NodeState.FAILURE;
        }
        return NodeState.SUCCESS;
    }
}
