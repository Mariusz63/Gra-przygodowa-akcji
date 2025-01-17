using BTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TaskGoToTarget : Node
{
    private Transform transform;
    private Rigidbody body;
    private BTBlackboard bb;

    public TaskGoToTarget(BTBlackboard bb)
    {
        this.transform = bb.enemyTransform;
        this.bb = bb;
        body = bb.enemyBody;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");

        if (!body)
        {
            Debug.LogError("Brak komponentu Rigidbody!");
            return NodeState.FAILURE; // albo inny sposób obs³ugi b³êdu
        }

        // Sprawdzamy odleg³oœæ od celu
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > bb.RadiusToForgetEnemy)
        {
            // "Zapominamy" o graczu
            parent.parent.ClearData("target");
            Debug.Log("Gracz za daleko - koñczê poœcig.");
            return NodeState.FAILURE;
        }

        if (distance > 0.5f)
        {
            // Wyliczamy kierunek
            Vector3 direction = (target.position - transform.position).normalized;

            // Przesuwamy siê za pomoc¹ Rigidbody
            Vector3 newPosition = body.position + (direction * bb.speed * Time.deltaTime);
            body.MovePosition(newPosition);

            // Obrót w stronê celu (ignorujemy ró¿nicê wysokoœci)
            Vector3 lookPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
            Quaternion targetRotation = Quaternion.LookRotation(lookPosition - transform.position);
            body.MoveRotation(targetRotation);
        }

        state = NodeState.RUNNING;
        return state;
    }
}
