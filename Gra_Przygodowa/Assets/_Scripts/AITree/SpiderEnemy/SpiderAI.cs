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
        blackboard.enemyTransform = GetComponent<Transform>();
        blackboard.enemyBody = GetComponent<Rigidbody>();
        
        Node root = new TaskWander(blackboard);

        return root;
    }
}
