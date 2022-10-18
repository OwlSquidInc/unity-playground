using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tower_Defence.Scripts.Towers
{
   public class StandardTower : MonoBehaviour
   {
      [SerializeField] private Transform   modelTransform;
      [SerializeField] private BoxCollider rangeCollider;

      [Header("Debug")]

      [SerializeField] private int towerRange;
      [SerializeField] private TargetSelectionMode targetSelectionMode;

      [Space(10)]

      [SerializeField] private GameObject       target;
      [SerializeField] private List<GameObject> targetsInRange = new List<GameObject>();

      protected void Awake()
      {
         // just in-case the collider size isn't set correctly
         rangeCollider.size = new Vector3(towerRange, 0, towerRange);
      }

      protected void Update()
      {
         if (target == null)
         {
            // make sure we have targets to chose from
            if (targetsInRange.Count == 0) { return; }

            // chose a target based on our selection mode
            target = targetSelectionMode switch {
                  TargetSelectionMode.First => targetsInRange[0],
                  TargetSelectionMode.Last  => targetsInRange[^1],
                  _                         => throw new ArgumentOutOfRangeException()
            };
         }

         modelTransform.LookAt(target.transform);

#if UNITY_EDITOR
         Debug.DrawLine(modelTransform.position, target.transform.position, Color.cyan);
#endif
      }

      protected void OnTriggerEnter(Collider other)
      {
         targetsInRange.Add(other.gameObject);

         // reset our target so we are forced to select a new target
         target = null;
      }

      protected void OnTriggerExit(Collider other)
      {
         if (target.Equals(other.gameObject)) { target = null; }
         targetsInRange.Remove(other.gameObject);
      }
   }
}