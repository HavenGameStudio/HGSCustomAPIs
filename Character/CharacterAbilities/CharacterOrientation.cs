using System.Collections;
using HGS.LastBastion.Character;
using HGS.Tools;
using UnityEngine;

namespace HGS.LastBastion.Character.CharacterAbilities
{
    public enum FacingDirection { North, South, East, West }
    public enum FacingType { None, Movement, WeaponAngle }
    public enum OrientationType { ModelFlip, ModelRotate }
    [AddComponentMenu("Haven Game Studio/Character/Abilities/Character Orientation")]
    public class CharacterOrientation : CharacterAbility
    {
        public override string HelpBoxText()
        {
            return "Controls how the character faces based on movement or weapon direction. Supports model flipping and rotation, and updates animation parameters to reflect direction. Essential for aligning visuals with gameplay behavior in 2D or top-down characters.";
        }

        private CharacterMovement characterMovement;
        private Weapon.Weapon characterWeapon;

        [SerializeField] private FacingType facingType;

        [Header("Orientation Type")]
        [SerializeField] private OrientationType characterOrientationType = OrientationType.ModelFlip;


        [HGSEnumCondition("characterOrientationType", OrientationType.ModelFlip)]
        [SerializeField] private Vector3 FlipValueRight = new Vector3(1, 1, 1);

        [HGSEnumCondition("characterOrientationType", OrientationType.ModelFlip)]
        [SerializeField] private Vector3 FlipValueLeft = new Vector3(-1, 1, 1);

        [HGSEnumCondition("characterOrientationType", OrientationType.ModelRotate)]
        [SerializeField] private Vector3 RotateValue = new Vector3(0f, 180, 0);

        [Header("Debug")]
        [HGSReadOnly]
        public FacingDirection facingDirection;
        protected override void PerformAbility()
        {
            base.PerformAbility();
            switch (facingType)
            {
                case FacingType.Movement:
                    FaceMovement();
                    break;
                case FacingType.WeaponAngle:
                    FaceWeaponAngle();
                    break;
                default:
                    break;
            }
        }

        public void FaceMovement()
        {
            if (characterMovement == null)
            {
                Debug.LogError("CharacterMovement is missing.");
                return;
            }

            Vector2 movement = characterMovement.movement;

            if (movement.sqrMagnitude <= 0.01f) // Use sqrMagnitude for better performance
                return;

            if (Mathf.Abs(movement.x) >= Mathf.Abs(movement.y))
            {
                // Horizontal movement
                if (movement.x > 0)
                {
                    character.characterModel.transform.localScale = FlipValueRight;
                    facingDirection = FacingDirection.East;
                }
                else
                {
                    character.characterModel.transform.localScale = FlipValueLeft;
                    facingDirection = FacingDirection.West;
                }

                if (!AnimatorHelper.HasParameter(AnimatorHelper.HorizontalDirectionParameter, animator)) return;

                animator.SetFloat(AnimatorHelper.HorizontalDirectionParameter, movement.x);
            }
            else
            {
                // Vertical movement
                if (movement.y > 0)
                {
                    facingDirection = FacingDirection.North;
                }
                else
                {
                    facingDirection = FacingDirection.South;
                }

                if (!AnimatorHelper.HasParameter(AnimatorHelper.VerticalDirectionParameter, animator)) return;

                animator.SetFloat(AnimatorHelper.VerticalDirectionParameter, movement.y);
            }
        }


        public void FaceWeaponAngle()
        {
            if (characterWeapon == null)
            {
                characterWeapon = character.FindAbility<CharacterHandleWeapon>()?.characterWeapon;
                return;
            }

            // Get weapon forward vector in 2D (we assume weapon aims using local right)
            Vector2 weaponDirection = characterWeapon.transform.right;

            // Normalize to ensure consistent blend values
            weaponDirection.Normalize();

            // Set facing direction left/right based on horizontal component
            if (weaponDirection.x >= 0f)
            {
                character.characterModel.transform.localScale = FlipValueRight;
                facingDirection = FacingDirection.East;
            }
            else
            {
                character.characterModel.transform.localScale = FlipValueLeft;
                facingDirection = FacingDirection.West;
            }

            // Apply normalized direction to animator
            animator.SetFloat(AnimatorHelper.HorizontalDirectionParameter, weaponDirection.x);
            animator.SetFloat(AnimatorHelper.VerticalDirectionParameter, weaponDirection.y);
        }




        protected override void Initialization()
        {
            base.Initialization();
        }

        protected override IEnumerator InitializationAsync()
        {
            while (character._characterAbilities.Length <= 0)
            {
                yield return null;
            }
            characterMovement = character.FindAbility<CharacterMovement>();
            CharacterHandleWeapon handleWeapon = character.FindAbility<CharacterHandleWeapon>();
            characterWeapon = handleWeapon.characterWeapon;
        }

        //CUSTOM INSPECTOR
        public void SetFacingType(FacingType facingType)
        {
            facingType = this.facingType;
        }

        public void SetOrientationType(OrientationType _orientationType)
        {
            characterOrientationType = _orientationType;
            RotateValue = new Vector3(0f, 180f, 0f);
        }
    }
}
