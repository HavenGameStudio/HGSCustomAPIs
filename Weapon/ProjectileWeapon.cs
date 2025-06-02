using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace HGS.LastBastion.Weapon
{
    public class ProjectileWeapon : Weapon
    {
        [SerializeField] private Projectile projectilePrefab;

        [SerializeField] private float projectileSpeed;
        [SerializeField] private float projectileDamage;
        [SerializeField] private float projectileLifetime = 5;

        public override bool UseWeapon()
        {
            if (!base.UseWeapon()) return false;

            lastUseTime = Time.time;

            StartCoroutine(UseWeaponCoroutine());
            return true;
        }

        IEnumerator UseWeaponCoroutine()
        {
            yield return new WaitForSeconds(delayBeforeUse);
            GameObject projectileObj = Instantiate(projectilePrefab.gameObject, transform.position, quaternion.identity);

            Projectile projectile = projectileObj.GetComponent<Projectile>();
            projectile.Initialization();
            projectile.InitializeProperties(projectileDamage, targetLayerMask, projectileSpeed, projectileLifetime, this);

            projectile.MoveProjectile(transform.right);

        }
    }
}
