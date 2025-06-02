using HGS.AI;
using UnityEngine;

namespace HGS.LastBastion
{
    [AddComponentMenu("Haven Game Studio/AI/AI Decisions/AI Target Detect Radius 2D")]
    public class AIDecisionTargetDetectRadius2D : AIDecision
    {
        public float radius;
        public LayerMask targetLayer;

        private Collider2D[] results;

        private float _lastTargetCheckTimestamp;
        private bool lastReturnValue;
        public float targetCheckFrequency;


        /// <summary>
        /// Detects all targets within the specified radius on the targetLayer,
        /// sorts them by distance from this object's position,
        /// assigns the closest target to _brain.Target,
        /// and returns true if any targets are found, otherwise false.
        /// </summary>
        /// <returns>True if at least one target is detected, false otherwise.</returns>
        public override bool Decide()
        {
            // Get all colliders within the radius on the targetLayer
            results = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

            // Check if any targets were found
            if (results != null && results.Length > 0)
            {
                // Sort targets by distance from this object's position
                System.Array.Sort(results, (a, b) =>
                {
                    float distA = Vector2.Distance(transform.position, a.transform.position);
                    float distB = Vector2.Distance(transform.position, b.transform.position);
                    return distA.CompareTo(distB);
                });

                // Assign the closest target to _brain.Target
                _brain.Target = results[0].transform;
                return true;
            }

            // No targets found, clear target and return false
            _brain.Target = null;
            return false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
