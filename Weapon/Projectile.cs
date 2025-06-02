using System.Collections;
using HGS.LastBastion.Core.Stats;
using HGS.LastBastion.Weapon;
using UnityEngine;

namespace HGS.LastBastion
{
    [RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float damage;
        [SerializeField] private LayerMask targetLayermask;
        [SerializeField] private float speed = 10f;
        [SerializeField] private float lifeTime;

        private BoxCollider2D _collider2D;
        private Rigidbody2D _rigidbody2D;
        private ProjectileWeapon weapon;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }
        public void Initialization()
        {
            _collider2D = GetComponent<BoxCollider2D>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
        }

        public void InitializeProperties(float _damage, LayerMask _targetLayerMask, float _speed, float _lifeTime, ProjectileWeapon _projectileWeapon)
        {
            weapon = _projectileWeapon;
            damage = _damage;
            targetLayermask = _targetLayerMask;
            speed = _speed;
            lifeTime = _lifeTime;
        }

        public void MoveProjectile(Vector3 _angle)
        {
            StartCoroutine(UpdateMovementRoutine(_angle));
        }

        IEnumerator UpdateMovementRoutine(Vector3 _angle)
        {
            // Capture the weapon direction ONCE at the start of the coroutine
            Vector3 direction = _angle.normalized;

            // Calculate angle once for rotation
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            while (true)
            {
                _rigidbody2D.linearVelocity = direction * speed;  // Move in fixed direction

                yield return null;  // Update every frame (better smoothness)
            }
        }




        private void OnTriggerEnter2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & targetLayermask) != 0)
            {
                // Try to get a damageable component
                var damageable = other.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damage);
                }

                Destroy(gameObject); // Destroy the projectile after hitting
            }
        }
    }
}
