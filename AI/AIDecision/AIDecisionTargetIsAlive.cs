using HGS.AI;
using HGS.LastBastion.Character.Core;
using HGS.LastBastion.Core.Stats;
using UnityEngine;

namespace HGS.LastBastion
{
    [AddComponentMenu("Haven Game Studio/AI/AI Decisions/AI Target Is Alive")]
    public class AIDecisionTargetIsAlive : AIDecision
    {
        private Health targetHealth;

        public override bool Decide()
        {
            if (targetHealth == null)
            {
                return false;
            }

            return targetHealth.Alive();
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            if (_brain.Target == null)
            {
                targetHealth = null;
                return;
            }
            targetHealth = _brain.Target.GetComponent<HGSCharacter>().health;
        }
    }
}
