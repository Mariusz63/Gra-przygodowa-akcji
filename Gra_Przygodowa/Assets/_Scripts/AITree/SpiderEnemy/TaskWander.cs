using BTree;
using UnityEngine;

public class TaskWander : Node
{
    private BTBlackboard bb;

    private int currentWaypointIndex = 0;

    [Header("Ustawienia przerwy (w sekundach)")]
    public float waitTime = 1f;
    private float waitCounter = 0f;
    private bool waiting = false;

    public TaskWander(BTBlackboard _blackboard)
    {
        bb = _blackboard;
    }

    public override NodeState Evaluate()
    {
        if (waiting)
        {
            waitCounter += Time.fixedDeltaTime;
            if (waitCounter > waitTime)
                waiting = false;
        }

        // Pobieramy waypoint z tablicy w Blackboardzie
        Transform wp = bb.waypoints[currentWaypointIndex];

        Vector3 horizontalDistance = new Vector3(
            bb.enemyTransform.position.x - wp.position.x,
            0f,
            bb.enemyTransform.position.z - wp.position.z
        );

        if (horizontalDistance.magnitude < 1f)
        {
            waitCounter = 0f;
            waiting = true;

            currentWaypointIndex = (currentWaypointIndex + 1) % bb.waypoints.Length;
        }
        else
        {
            // Korzystamy z prêdkoœci (bb.speed) i rb (bb.enemyBody)
            MoveTo(wp.position);
        }

        state = NodeState.RUNNING;
        return state;
    }

    private void MoveTo(Vector3 targetPosition)
    {
        // Ustaw nowy wektor ruchu tylko po X i Z
        Vector3 targetXZ = new Vector3(
            targetPosition.x,
            bb.enemyBody.position.y,
            targetPosition.z
        );

        // Ruch
        bb.enemyBody.MovePosition(
            Vector3.MoveTowards(
                bb.enemyBody.position,
                targetXZ,
                bb.speed * Time.fixedDeltaTime
            )
        );

        // Obracanie
        Vector3 lookDir = new Vector3(
            targetPosition.x,
            bb.enemyBody.position.y,
            targetPosition.z
        );
        bb.enemyTransform.LookAt(lookDir);
    }
}
