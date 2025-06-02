using UnityEngine;

namespace HGS.AI
{
    [AddComponentMenu("Haven Game Studio/AI/AI Decisions/AI Distance To Target")]
    public class AIDecisionDistanceToTarget : AIDecision
    {
        [SerializeField] private float distanceComparison;
        public override bool Decide()
        {
            if (_brain.Target == null) return false;

            return Vector2.Distance(transform.position, _brain.Target.position) <= distanceComparison;
        }

    }
}
