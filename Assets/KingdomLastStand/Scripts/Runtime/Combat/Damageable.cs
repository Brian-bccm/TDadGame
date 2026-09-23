using System;
using UnityEngine;

namespace KingdomLastStand.Combat
{
    public sealed class Damageable : MonoBehaviour, IDamageable
    {
        [SerializeField, Min(1f)] private float maximumHealth = 100f;

        public bool IsAlive => CurrentHealth > 0f;
        public float CurrentHealth { get; private set; }
        public float MaximumHealth => maximumHealth;

        public event Action<float, float> HealthChanged;
        public event Action Died;

        private void Awake() => RestoreToFull();

        public void Configure(float health)
        {
            maximumHealth = Mathf.Max(1f, health);
            RestoreToFull();
        }

        public void RestoreToFull()
        {
            CurrentHealth = maximumHealth;
            HealthChanged?.Invoke(CurrentHealth, maximumHealth);
        }

        public void TakeDamage(float amount)
        {
            if (!IsAlive || amount <= 0f) return;

            CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
            HealthChanged?.Invoke(CurrentHealth, maximumHealth);

            if (CurrentHealth <= 0f)
                Died?.Invoke();
        }
    }
}

