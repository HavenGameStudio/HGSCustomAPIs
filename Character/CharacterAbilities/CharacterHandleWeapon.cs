using HGS.LastBastion.Character;
using HGS.LastBastion.Core.Managers;
using HGS.Tools;
using UnityEngine;

namespace HGS.LastBastion.Character.CharacterAbilities
{
    [AddComponentMenu("Haven Game Studio/Character/Abilities/Character Handle Weapon")]
    public class CharacterHandleWeapon : CharacterAbility
    {
        public override string HelpBoxText()
        {
            return "This ability handles weapon usage for the character. It instantiates and attaches a weapon prefab, manages weapon orientation, and triggers weapon attacks based on input. Use this for characters that can equip and use ranged or melee weapons.";
        }

        [Space]
        [Tooltip("The Weapon of this character")]
        [SerializeField] private Weapon.Weapon weaponPrefab;

        [Tooltip("The weapon attachment of this character")]
        [SerializeField] private Transform weaponParent;

        [Space]
        [Tooltip("If this is true, The character will face the weapon angle whenever it uses the weapon")]
        [SerializeField] private bool faceWeaponAngle;

        [Header("Debug")]
        [HGSReadOnly]
        public Weapon.Weapon characterWeapon;

        //other abilities
        private CharacterOrientation orientation;

        public override bool RegisterInput(InputManager inputManager)
        {
            if (!base.RegisterInput(inputManager) || abilityPerformType == AbilityPerformType.Script)
            {
                return false;
            }
            inputManager.OnAttackEvent += UseWeapon;
            return true;
        }
        protected override void Initialization()
        {
            base.Initialization();

            if (weaponPrefab == null) return;

            if (weaponParent == null) return;

            GameObject spawnedWeapon = Instantiate(weaponPrefab.gameObject, weaponParent);
            characterWeapon = spawnedWeapon.GetComponent<Weapon.Weapon>();
        }
        public void UseWeapon()
        {
            if (characterWeapon == null)
            {
                Debug.LogError($"There is no weapon on this character");
            }
            if (faceWeaponAngle)
            {
                if (orientation == null) orientation = character.FindAbility<CharacterOrientation>();

                orientation.FaceWeaponAngle();
            }
            characterWeapon.UseWeapon();
        }
        public void AimAtTarget(Vector3 target)
        {
            if (target == null) return;

            Vector3 direction = target - transform.position;
            direction.z = 0f; // Ensure it's in 2D plane

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            characterWeapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            // Optional sprite flip based on aim direction
            Vector3 localScale = characterWeapon.transform.localScale;
            if (angle > 90f || angle < -90f)
                localScale.y = -Mathf.Abs(localScale.y);
            else
                localScale.y = Mathf.Abs(localScale.y);
            transform.localScale = localScale;
        }

#if UNITY_EDITOR
        //CUSTOM INSPECTOR
        public void SetWeaponAttachment(Transform _weaponAttachment)
        {
            weaponParent = _weaponAttachment;
        }

#endif


    }
}
