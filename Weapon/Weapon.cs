using System;
using System.Collections;
using HGS.LastBastion.Character.Core;
using HGS.LastBastion.Core.Managers;
using HGS.LastBastion.Core.Stats;
using UnityEditor.EditorTools;
using UnityEngine;

namespace HGS.LastBastion.Weapon
{
    [RequireComponent(typeof(WeaponAim2D), typeof(BoxCollider2D))]
    public class Weapon : MonoBehaviour
    {
        private HGSCharacter character;
        private BoxCollider2D _collider2D;

        [Header("Damage")]
        [Tooltip("The damage ammount this weapon will deal")]
        [SerializeField] protected float Damage;
        [Tooltip("Delay (in seconds) before using the weapon. Use this to sync the damage with animation")]
        [SerializeField] protected float delayBeforeUse;

        [Space]
        [Header("Target LayerMask")]
        [Tooltip("The target's layermask to damage")]
        [SerializeField] protected LayerMask targetLayerMask;

        [Space]
        [Header("Weapon Behavior")]
        [Tooltip("If this is true the weapon will flip when character flips")]
        [SerializeField] protected bool flipWeaponOnCharacterFlip = true;

        [Space]
        [Header("Animation")]
        [SerializeField] protected Animator animator;
        [SerializeField] protected string AttackAnimatorParameter;

        [Space]
        [Header("Cooldown")]
        [Tooltip("Cooldown time in seconds before the weapon can be used again")]
        [SerializeField] protected float cooldownTime = 1f;

        protected float lastUseTime = -Mathf.Infinity;

        #region DAMAGE AREA
        [SerializeField] protected Vector2 damageAreaPos;
        [SerializeField] protected Vector2 damageAreaSize = new Vector2(1f, 0.5f);
        #endregion

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Start()
        {
            Initialization();
        }
        #region Initialization
        protected virtual void Initialization()
        {
            _collider2D = GetComponent<BoxCollider2D>();
            if (_collider2D == null)
            {
                _collider2D = gameObject.AddComponent<BoxCollider2D>();
            }
            _collider2D.offset = damageAreaPos;
            _collider2D.size = damageAreaSize;
            StartCoroutine(GetNeededComponents());

        }
        protected virtual IEnumerator GetNeededComponents()
        {
            yield return new WaitForSeconds(1f);
            character = GetComponentInParent<HGSCharacter>();
            animator = character.characterAnimator;
        }
        #endregion

        protected virtual void Update()
        {
            Vector2 worldOffset = transform.rotation * _collider2D.offset;
            damageAreaPos = (Vector2)transform.position + worldOffset;
            damageAreaSize = _collider2D.size;
        }


        #region WEAPON USE
        public virtual bool UseWeapon()
        {
            if (Time.time < lastUseTime + cooldownTime)
            {
                // Cooldown not finished, ignore use
                return false;
            }

            if (_collider2D == null) return false;

            // Update last use time
            lastUseTime = Time.time;


            if (animator != null && AnimatorHelper.HasParameter(AttackAnimatorParameter, animator))
            {
                animator.SetTrigger(AttackAnimatorParameter); // play the attack animation false
            }

            return true;

        }

        protected virtual IEnumerator DamageTarget(IDamageable damageable)
        {
            yield break;
        }
        #endregion
    }
}
