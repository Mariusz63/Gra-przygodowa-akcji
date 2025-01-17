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
        
        Node root = new TaskWander(blackboard);

        return root;
    }
}
