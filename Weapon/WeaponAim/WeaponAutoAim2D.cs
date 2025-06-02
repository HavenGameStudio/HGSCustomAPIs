using UnityEngine;

namespace HGS.LastBastion
{
    public class WeaponAutoAim2D : MonoBehaviour
    {
        private Weapon.Weapon characterWeapon;

        [SerializeField] private float scanRadius;
        [SerializeField] private LayerMask targetLayerMask;

        public bool Authorize;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            characterWeapon = GetComponent<Weapon.Weapon>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!Authorize) return;

            Collider2D[] targetsInRadius = Physics2D.OverlapCircleAll(transform.position, scanRadius, targetLayerMask);
            if (targetsInRadius.Length == 0)
            {
                return;
            }

            // Find the closest target
            Collider2D closestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;

            foreach (var target in targetsInRadius)
            {
                Vector3 directionToTarget = target.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    closestTarget = target;
                }
            }

            if (closestTarget != null)
            {
                Vector3 direction = closestTarget.transform.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, scanRadius);
        }
    }
}
