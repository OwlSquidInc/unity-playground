using System;
using System.Collections.Generic;
using Tower_Defence.Scripts.Enums;
using UnityEngine;

namespace Tower_Defence.Scripts.Towers
{
   public class StandardTower : MonoBehaviour
   {
      //============================================================
      // Inspector Variables:
      //============================================================

      [SerializeField] private Transform   modelTransform;
      [SerializeField] private BoxCollider rangeCollider;

      [Space(10)]

      [SerializeField] private int                 towerRange;
      [SerializeField] private TargetSelectionMode targetSelectionMode;

      [Header("Debug")]

      [SerializeField] private GameObject       target;
      [SerializeField] private List<GameObject> targetsInRange = new List<GameObject>();

      //============================================================
      // Unity Lifecycle:
      //============================================================

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
         if (Input.GetKeyDown(KeyCode.UpArrow)) { SetTargetSelectionMode(TargetSelectionMode.First); }
         if (Input.GetKeyDown(KeyCode.DownArrow)) { SetTargetSelectionMode(TargetSelectionMode.Last); }
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

      //============================================================
      // Private Methods:
      //============================================================

      private void SetTargetSelectionMode(TargetSelectionMode newTargetSelectionMode)
      {
         target = null;
         targetSelectionMode = newTargetSelectionMode;
      }
   }
}