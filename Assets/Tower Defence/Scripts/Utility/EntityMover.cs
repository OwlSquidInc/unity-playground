using UnityEngine;

namespace Tower_Defence.Scripts.Utility
{
    public class EntityMover : MonoBehaviour
    {
        //============================================================
        // Inspector Variables:
        //============================================================

        [Space(6)]

        [SerializeField] private bool automaticallyMove;
        [SerializeField] private Vector3 translateDirection = new Vector3(0, 0, 1);
        [SerializeField] private float   translateSpeed = 2f;

        //============================================================
        // Unity Lifecycle:
        //============================================================

        protected void Update()
        {
            if (automaticallyMove)
            {
                transform.Translate(translateDirection * (Time.deltaTime * translateSpeed));
            }

#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.DownArrow))  { transform.Translate(Vector3.back    * (Time.deltaTime * translateSpeed)); }
            if (Input.GetKey(KeyCode.UpArrow))    { transform.Translate(Vector3.forward * (Time.deltaTime * translateSpeed)); }
            if (Input.GetKey(KeyCode.LeftArrow))  { transform.Translate(Vector3.left    * (Time.deltaTime * translateSpeed)); }
            if (Input.GetKey(KeyCode.RightArrow)) { transform.Translate(Vector3.right   * (Time.deltaTime * translateSpeed)); }
#endif
        }
    }
}