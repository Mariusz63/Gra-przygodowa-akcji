using UnityEngine;

namespace BTree
{
    [System.Serializable]
    public class BTBlackboard
    {
        // Ten obiekt (paj¹k), który ma siê poruszaæ
        public Transform enemyTransform;

        // Komponent fizyki
        public Rigidbody enemyBody;

        // Tablica waypointów do patrolu
        public Transform[] waypoints;

        // Prêdkoœæ poruszania
        public float speed = 2f;
        public float FOVradius = 6f;
        public float RadiusToForgetEnemy = 12f;

        // Mo¿esz dodaæ wiele innych rzeczy, np.:
        // public float visionRange;
        // public float health;
        // public Transform playerTransform;
        // itd.
    }
}
