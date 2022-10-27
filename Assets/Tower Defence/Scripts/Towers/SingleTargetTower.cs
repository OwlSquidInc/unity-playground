using System;
using System.Collections;
using System.Collections.Generic;
using Tower_Defence.Scripts.Enums;
using UnityEngine;

namespace Tower_Defence.Scripts.Towers
{
   public class SingleTargetTower : MonoBehaviour
   {
      //============================================================
      // Inspector Variables:
      //============================================================

      [SerializeField] private Transform   modelTransform;
      [SerializeField] private SphereCollider rangeCollider;

      [Space(10)]

      [SerializeField] private int                 towerRange;
      [SerializeField] private TargetSelectionMode targetSelectionMode;

      [Header("Debug")]

      [Space(6)]

      [SerializeField] private Enemy.Enemy       target;
      [SerializeField] private float             distanceToTarget;
      [SerializeField] private List<Enemy.Enemy> targetsInRange = new List<Enemy.Enemy>();

      //============================================================
      // Private Fields:
      //============================================================

      private IEnumerator targetingCoroutineEnumerator;

      private readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

      //============================================================
      // Unity Lifecycle:
      //============================================================

      private void Awake()
      {
         // just in-case the collider size isn't set correctly
         rangeCollider.radius = towerRange;

         StartCoroutine(targetingCoroutineEnumerator = TargetingCoroutine());
      }

      private void Update()
      {
         if (target != null)
         {
            modelTransform.LookAt(target.transform);
            distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
         }

#if UNITY_EDITOR
         if (target != null) { Debug.DrawLine(modelTransform.position, target.transform.position, Color.cyan); }
         if (Input.GetKeyDown(KeyCode.C)) { SetTargetSelectionMode(TargetSelectionMode.ClosestToExit); }
         if (Input.GetKeyDown(KeyCode.F)) { SetTargetSelectionMode(TargetSelectionMode.FurthestFromExit); }
#endif
      }

      private void OnTriggerEnter(Collider other)
      {
         targetsInRange.Add(other.GetComponent<Enemy.Enemy>());
      }

      private void OnTriggerExit(Collider other)
      {
         Enemy.Enemy exitedEnemy = other.GetComponent<Enemy.Enemy>();

         if (target == exitedEnemy) { target = null; }
         targetsInRange.Remove(exitedEnemy);
      }

      private void OnDestroy()
      {
         targetingCoroutineEnumerator = null;
      }

      //============================================================
      // Private Methods:
      //============================================================

      // debug method to test target switching
      private void SetTargetSelectionMode(TargetSelectionMode newTargetSelectionMode)
      {
         targetSelectionMode = newTargetSelectionMode;
      }

      //============================================================
      // Coroutines:
      //============================================================

      private IEnumerator TargetingCoroutine()
      {
         while (targetingCoroutineEnumerator != null)
         {
            // if there are no targets in range then all we can do is wait
            if (targetsInRange.Count == 0) { yield return waitForEndOfFrame; continue; }

            Enemy.Enemy        newTarget            = null;
            IList<Enemy.Enemy> copyOfTargetsInRange = new List<Enemy.Enemy>(targetsInRange);

            foreach (Enemy.Enemy enemy in copyOfTargetsInRange)
            {
               // remove the target if it does not exist or is no-longer in range
               if (enemy == null)
               {
                  targetsInRange.Remove(enemy);
                  continue;
               }

               // use the first enemy initially as our new target - we cannot perform comparisons without both
               if (newTarget == null)
               {
                  newTarget = enemy;
                  continue;
               }

               // calculate the new target based on our target selection mode
               newTarget = targetSelectionMode switch {
                     TargetSelectionMode.ClosestToExit    => (enemy.distanceToEnd < newTarget.distanceToEnd) ? enemy : newTarget,
                     TargetSelectionMode.FurthestFromExit => (enemy.distanceToEnd > newTarget.distanceToEnd) ? enemy : newTarget,
                     _                                    => throw new ArgumentOutOfRangeException()
               };
            }

            target = newTarget;
            yield return waitForEndOfFrame;
         }
      }
   }
}