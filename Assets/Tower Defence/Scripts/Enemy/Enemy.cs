using UnityEngine;

namespace Tower_Defence.Scripts.Enemy
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Transform targetPosition;
        public float     DistanceUntilEnd { get; private set; }

        protected void Update()
        {
            DistanceUntilEnd = Vector3.Distance(transform.position, targetPosition.position);
        }
    }
}