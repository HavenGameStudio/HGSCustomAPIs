using System.Collections;
using HGS.LastBastion.Core.Stats;
using UnityEngine;

namespace HGS.LastBastion.Weapon
{
    public class MeleeWeapon : Weapon
    {
        protected override IEnumerator DamageTarget(IDamageable damageable)
        {
            yield return new WaitForSeconds(delayBeforeUse);
            damageable.TakeDamage(Damage); // Replace 10 with your actual damage amount
        }

        public override bool UseWeapon()
        {
            if (!base.UseWeapon()) return false;

            Collider2D[] hits = Physics2D.OverlapBoxAll(damageAreaPos, damageAreaSize, transform.eulerAngles.z, targetLayerMask);

            foreach (var hit in hits)
            {
                IDamageable damageable = hit.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    StartCoroutine(DamageTarget(damageable));
                }
            }
            return true;
        }
    }
}
