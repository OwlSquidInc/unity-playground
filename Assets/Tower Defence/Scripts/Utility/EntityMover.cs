using UnityEngine;

namespace Tower_Defence.Scripts.Utility
{
    public class EntityMover : MonoBehaviour
    {
        [SerializeField] private Transform thisTransform;

        [Space(6)]

        [SerializeField] private Vector3 translateDirection = new Vector3(0, 0, 1);
        [SerializeField] private float   translateSpeed = 2f;

        protected void FixedUpdate()
        {
            thisTransform.Translate(translateDirection * (Time.deltaTime * translateSpeed));
        }
    }
}