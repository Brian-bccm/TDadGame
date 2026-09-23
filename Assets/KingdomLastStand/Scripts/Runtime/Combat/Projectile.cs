using UnityEngine;

namespace KingdomLastStand.Combat
{
    public sealed class Projectile : MonoBehaviour
    {
        private Transform _target;
        private IDamageable _damageable;
        private float _damage;
        private float _speed;

        public void Launch(Transform target, IDamageable damageable, float damage, float speed)
        {
            _target = target;
            _damageable = damageable;
            _damage = Mathf.Max(0f, damage);
            _speed = Mathf.Max(0.1f, speed);
        }

        private void Update()
        {
            if (_target == null || _damageable == null || !_damageable.IsAlive)
            {
                Destroy(gameObject);
                return;
            }

            transform.position = Vector3.MoveTowards(
                transform.position,
                _target.position,
                _speed * Time.deltaTime);

            if ((transform.position - _target.position).sqrMagnitude > 0.01f) return;

            _damageable.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
