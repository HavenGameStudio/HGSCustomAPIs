using UnityEngine;

namespace HGS.LastBastion.Character.Statemachine
{
    public class CharacterStates
    {
        public enum ConditionStates
        {
            Normal,
            Building,
            Chopping
        }

        public enum MovementStates
        {
            Idle,
            Walking
        }

        public ConditionStates currentConditionState { get; private set; }
        public MovementStates currentMovementState { get; private set; }

    }
}
