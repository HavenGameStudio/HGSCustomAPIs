using HGS.LastBastion;
using HGS.LastBastion.Character.CharacterAbilities;
using HGS.LastBastion.Character.Core;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace HGS.AI
{
    [AddComponentMenu("Haven Game Studio/AI/AI Actions/AI Action Move Towards Target")]
    public class AIActionMoveTowardsTarget : AIAction
    {
        public float MoveSpeed = 2f;

        private CharacterMovement characterMovement;

        protected override void Awake()
        {
            base.Awake();
            characterMovement = _brain.Owner.GetComponentInChildren<CharacterMovement>();
        }

        public override void PerformAction()
        {
            Move();
        }

        private void Move()
        {
            if (_brain.Target == null) return;
            characterMovement.SetAIMovement(_brain.Target.position);
        }

        public override void OnExitState()
        {
            base.OnExitState();
            characterMovement.StopMoving();
            Debug.Log($"{_brain.Owner.name} Movetowardstarget Exit");

        }
    }
}
