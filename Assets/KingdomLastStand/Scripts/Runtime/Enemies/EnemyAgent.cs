using System;
using KingdomLastStand.Battle;
using KingdomLastStand.Combat;
using KingdomLastStand.Data;
using UnityEngine;

namespace KingdomLastStand.Enemies
{
    [RequireComponent(typeof(Damageable))]
    public sealed class EnemyAgent : MonoBehaviour
    {
        private Transform[] _path;
        private EnemyData _data;
        private CastleController _castle;
        private Damageable _health;
        private int _waypointIndex;
        private bool _removed;
        private bool _deathSubscribed;

        public float PathProgress => _path == null || _path.Length == 0
            ? 0f
            : (_waypointIndex + 1f) / _path.Length;
        public IDamageable Health => _health;
        public int GoldReward => _data != null ? _data.GoldReward : 0;
        public event Action<EnemyAgent, bool> Removed;

        private void Awake()
        {
            EnsureHealth();
        }

        private void OnDestroy()
        {
            if (_health != null)
                _health.Died -= HandleDeath;
        }

        public void Initialize(EnemyData data, Transform[] path, CastleController castle)
        {
            _data = data ? data : throw new ArgumentNullException(nameof(data));
            _path = path ?? throw new ArgumentNullException(nameof(path));
            _castle = castle ? castle : throw new ArgumentNullException(nameof(castle));
            EnsureHealth();
            _waypointIndex = 0;
            _removed = false;
            _health.Configure(data.MaximumHealth);

            if (_path.Length > 0)
                transform.position = _path[0].position;
        }

        private void EnsureHealth()
        {
            if (_health == null) _health = GetComponent<Damageable>();
            if (_health != null && !_deathSubscribed)
            {
                _health.Died += HandleDeath;
                _deathSubscribed = true;
            }
        }

        private void Update()
        {
            if (_removed || !_health.IsAlive || _path == null || _path.Length == 0) return;

            var target = _path[_waypointIndex];
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                _data.MovementSpeed * Time.deltaTime);

            if ((transform.position - target.position).sqrMagnitude > 0.0025f) return;

            _waypointIndex++;
            if (_waypointIndex >= _path.Length)
                ReachCastle();
        }

        private void ReachCastle()
        {
            if (_removed) return;
            _castle.ReceiveEnemy(_data.CastleDamage);
            Remove(wasKilled: false);
        }

        private void HandleDeath() => Remove(wasKilled: true);

        private void Remove(bool wasKilled)
        {
            if (_removed) return;
            _removed = true;
            Removed?.Invoke(this, wasKilled);
            Destroy(gameObject);
        }
    }
}
