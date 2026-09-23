using System;
using KingdomLastStand.Combat;
using UnityEngine;

namespace KingdomLastStand.Battle
{
    [RequireComponent(typeof(Damageable))]
    public sealed class CastleController : MonoBehaviour
    {
        private Damageable _health;

        public IDamageable Health => _health;
        public event Action Destroyed;

        private void Awake()
        {
            _health = GetComponent<Damageable>();
            _health.Died += HandleDestroyed;
        }

        private void OnDestroy()
        {
            if (_health != null)
                _health.Died -= HandleDestroyed;
        }

        public void ReceiveEnemy(int damage) => _health.TakeDamage(Mathf.Max(1, damage));

        private void HandleDestroyed() => Destroyed?.Invoke();
    }
}

