using System.Collections;
using System.Collections.Generic;
using HGS.LastBastion.Character.Core;
using HGS.LastBastion.Character.Statemachine;
using HGS.LastBastion.Core.Managers;
using HGS.Tools;
using UnityEngine;

namespace HGS.LastBastion.Character.CharacterAbilities
{
    public enum AbilityPerformType { Update, FixedUpdate, Script, None }
    public enum AnimatorParemeterType { Int, Bool, Float, Trigger }

    public class CharacterAbility : MonoBehaviour
    {
        protected HGSCharacter character;
        protected Animator animator;
        protected InputManager inputManager;
        protected Rigidbody2D _rb;

        [HideInInspector] public string abilityName;

        [Header("Blocking Movement States")]
        [SerializeField] List<CharacterStates.ConditionStates> blockingConditionStates = new List<CharacterStates.ConditionStates>();
        [Space]

        [Space]
        [Header("Perform Type")]
        [SerializeField] protected AbilityPerformType abilityPerformType;

        protected virtual void Awake()
        {
            character = GetComponentInParent<HGSCharacter>();
        }

        private void OnEnable()
        {
            character.OnCharacterInitialize += Initialization;
        }
        // Update is called once per frame
        void Update()
        {
            if (abilityPerformType == AbilityPerformType.Update)
            {
                PerformAbility();
            }
        }

        private void FixedUpdate()
        {
            if (abilityPerformType == AbilityPerformType.FixedUpdate)
            {
                PerformAbility();
            }
        }

        protected virtual void Initialization()
        {
            inputManager = FindFirstObjectByType<InputManager>();
            if (character == null)
            {
                Debug.LogError($"Character is not assigned or cannot find in the components");
            }
            _rb = character._rb;

            BindAnimator();
            StartCoroutine(InitializationAsync());
        }

        /// <summary>
        /// Initialization async.
        /// Call this if to make the initialization asynchronous
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator InitializationAsync()
        {
            yield break;
        }

        /// <summary>
        /// Performs the ability
        /// </summary>
        protected virtual void PerformAbility() { }

        #region ANIMATIONS
        protected void BindAnimator()
        {
            if (character.characterAnimator == null)
            {
                Debug.Log($"Finding Animator in children");
                character.characterAnimator = GetComponentInChildren<Animator>();
            }

            animator = character.characterAnimator;
        }

        protected virtual void PerformAnimation(AnimatorParemeterType _parameterType, string animatorParameter, bool parameterValue)
        {
            if (animator == null) return;

            switch (_parameterType)
            {
                case AnimatorParemeterType.Bool:
                    AnimatorHelper.SetBool(animator, animatorParameter, parameterValue);
                    break;
                case AnimatorParemeterType.Trigger:
                    animator.SetTrigger(animatorParameter);
                    break;
                default:
                    break;
            }
        }
        #endregion

        /// <summary>
        /// Register the input of the player to use a certain ability
        /// </summary>
        /// <param name="inputManager"></param>
        public virtual bool RegisterInput(InputManager inputManager)
        {
            return character.characterType == CharacterType.Player;
        }

        /// <summary>
        /// Checks if the character has condition that will prevent using a certian ability.
        /// </summary>
        /// <param name="_conditionState">The current condition of the Character.</param>
        /// <returns></returns>
        protected bool HasCondition(CharacterStates.ConditionStates _conditionState)
        {
            foreach (var state in blockingConditionStates)
            {
                if (state == _conditionState)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual void ChangePerformType(AbilityPerformType _abilityPerformType)
        {
            abilityPerformType = _abilityPerformType;
        }

#if UNITY_EDITOR
        public void SetPerformType(AbilityPerformType performType)
        {
            this.abilityPerformType = performType;
        }

        public virtual string HelpBoxText()
        {
            return "";
        }
#endif
    }
}
