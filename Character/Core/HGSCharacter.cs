using System;
using HGS.LastBastion.Core.Stats;
using UnityEngine;
using HGS.LastBastion.Character.CharacterAbilities;
using HGS.LastBastion.Character.Statemachine;
using HGS.Tools;
using HGS.AI;

namespace HGS.LastBastion.Character.Core
{
    public enum CharacterType { Player, AI }
    public enum AIType { NoneAI, Ally, Enemy }
    public class HGSCharacter : MonoBehaviour
    {
        public CharacterType characterType;

        [HGSEnumCondition("characterType", CharacterType.AI)]
        public AIType aIType;

        public GameObject characterModel;
        public Animator characterAnimator;

        //Character Abilities
        public CharacterAbility[] _characterAbilities { get; private set; }

        //UNITY BUILT IN COMPONENTS REQUIRED IN CHARACTER
        [HideInInspector] public Rigidbody2D _rb;
        [HideInInspector] public BoxCollider2D _boxCollider2D;

        #region Events
        public event Action OnCharacterInitialize;
        #endregion

        #region Stats
        [Header("Stats")]
        public Health health;

        [Header("Healthbar")]
        public bool CreateHealthBar;
        [HGSCondition("CreateHealthBar", true, false)]
        public HealthBar healthBarObject;
        #endregion

        [HGSEnumCondition("characterType", CharacterType.AI)]
        [Header("Brain")]
        [HGSReadOnly]
        public AIBrain brain;

        [HideInInspector] public CharacterStates.ConditionStates characterCondition;
        [HideInInspector] public CharacterStates.MovementStates characterMovementState;

        protected float randomFloatValue;
        protected bool hasRandomParameter;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Initialization();
            hasRandomParameter = AnimatorHelper.HasParameter("RandomFloat", characterAnimator);
            Debug.Log($"{name} has a random parameter? : {hasRandomParameter}");
        }
        private void Update()
        {
            if (!hasRandomParameter) return;
            randomFloatValue = GenerateRandomFloat();
            characterAnimator.SetFloat(AnimatorHelper.RandomFloatParameter, randomFloatValue);
        }

        private float GenerateRandomFloat()
        {
            return UnityEngine.Random.Range(0f, 1f);
        }

        private void Initialization()
        {
            _rb = GetComponent<Rigidbody2D>();
            _boxCollider2D = GetComponent<BoxCollider2D>();

            CacheAbilities();
            OnCharacterInitialize?.Invoke();
        }

        /// <summary>
        /// Finds all abilities and store it in a field
        /// </summary>
        private void CacheAbilities()
        {
            _characterAbilities = GetComponentsInChildren<CharacterAbility>();
        }

        /// <summary>
        /// Finds an ability cached in _characterAbilities
        /// </summary>
        /// <typeparam name="T">Ability to find</typeparam>
        /// <returns>Ability from cached ability</returns>
        public T FindAbility<T>() where T : CharacterAbility
        {
            foreach (var ability in _characterAbilities)
            {
                if (ability is T match)
                    return match;
            }
            return null;
        }

        /// <summary>
        /// Finds an ability by it's name.
        /// Use the FindAbility instead for better performance
        /// </summary>
        /// <param name="name">The name of the ability</param>
        /// <returns>CharacterAbility</returns>
        public CharacterAbility FindAbilityByName(string name)
        {
            for (int i = 0; i < _characterAbilities.Length; i++)
            {
                if (_characterAbilities[i].abilityName == name)
                {
                    return _characterAbilities[i];
                }
            }

            return null;
        }

        #region Character State Machine
        public void ChangeState(CharacterStates.ConditionStates _conditionState)
        {
            characterCondition = _conditionState;
        }

        public void ChangeState(CharacterStates.MovementStates _movementState)
        {
            characterMovementState = _movementState;
        }
        #endregion

    }
}
