using HGS.AI;
using UnityEngine;

namespace HGS.LastBastion
{
    [AddComponentMenu("Haven Game Studio/AI/AI Decisions/AI Distance To Target Collider Edge")]
    public class AIDecisionDistanceToTargetColliderEdge : AIDecision
    {
        /// The possible comparison modes
		public enum ComparisonModes { StrictlyLowerThan, LowerThan, Equals, GreaterThan, StrictlyGreaterThan }
        /// the comparison mode
        [Tooltip("the comparison mode")]
        public ComparisonModes ComparisonMode = ComparisonModes.GreaterThan;
        /// the distance to compare with
        [Tooltip("the distance to compare with")]
        public float Distance;

        /// <summary>
        /// On Decide we check our distance to the Target
        /// </summary>
        /// <returns></returns>
        public override bool Decide()
        {
            return EvaluateDistance();
        }

        /// <summary>
        /// Returns true if the distance conditions are met
        /// </summary>
        /// <returns></returns>
        protected virtual bool EvaluateDistance()
        {
            if (_brain.Target == null)
            {
                return false;
            }

            Collider2D targetCollider = _brain.Target.GetComponent<Collider2D>();
            Vector2 closestPoint = targetCollider.ClosestPoint(transform.position);
            float distance = Vector3.Distance(this.transform.position, closestPoint);

            if (ComparisonMode == ComparisonModes.StrictlyLowerThan)
            {
                return (distance < Distance);
            }
            if (ComparisonMode == ComparisonModes.LowerThan)
            {
                return (distance <= Distance);
            }
            if (ComparisonMode == ComparisonModes.Equals)
            {
                return (distance == Distance);
            }
            if (ComparisonMode == ComparisonModes.GreaterThan)
            {
                return (distance >= Distance);
            }
            if (ComparisonMode == ComparisonModes.StrictlyGreaterThan)
            {
                return (distance > Distance);
            }
            return false;
        }

    }
}
