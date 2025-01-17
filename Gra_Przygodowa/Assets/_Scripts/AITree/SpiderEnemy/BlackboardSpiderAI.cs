using UnityEngine;

namespace BTree
{
    [System.Serializable]
    public class BTBlackboard
    {
        // Ten obiekt (paj�k), kt�ry ma si� porusza�
        public Transform enemyTransform;

        // Komponent fizyki
        public Rigidbody enemyBody;

        // Tablica waypoint�w do patrolu
        public Transform[] waypoints;

        // Pr�dko�� poruszania
        public float speed = 2f;
        public float FOVradius = 6f;
        public float RadiusToForgetEnemy = 12f;

        // Mo�esz doda� wiele innych rzeczy, np.:
        // public float visionRange;
        // public float health;
        // public Transform playerTransform;
        // itd.
    }
}
