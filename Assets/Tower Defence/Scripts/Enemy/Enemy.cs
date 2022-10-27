using UnityEngine;

namespace Tower_Defence.Scripts.Enemy
{
    public class Enemy : MonoBehaviour
    {
        //============================================================
        // Inspector Variables:
        //============================================================

        [SerializeField] private Transform targetPosition;

        //============================================================
        // Public Fields:
        //============================================================

        public float distanceToEnd = 0;

        //============================================================
        // Unity Lifecycle:
        //============================================================

        protected void Update()
        {
            distanceToEnd = Vector3.Distance(transform.position, targetPosition.position);
        }
    }
}