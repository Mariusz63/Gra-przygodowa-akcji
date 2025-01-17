using BTree;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class SpiderAI : BehaviorTree
{
    public BTBlackboard blackboard;

    protected override Node SetupTree()
    {
        blackboard.enemyTransform = transform.Find("Body");
        blackboard.enemyBody = GetComponentInChildren<Rigidbody>();

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckEnemyFOV(blackboard),
                new TaskGoToTarget(blackboard)
            }),
            new TaskWander(blackboard)
        });

        return root;
    }
}
