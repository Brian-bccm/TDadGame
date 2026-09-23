using KingdomLastStand.Combat;
using KingdomLastStand.Data;
using KingdomLastStand.Enemies;
using UnityEngine;

namespace KingdomLastStand.Units
{
    public sealed class UnitTower : MonoBehaviour
    {
        private const int TargetBufferSize = 64;
        private Collider[] _targetBuffer;

        [SerializeField] private UnitData data;
        [SerializeField] private Transform projectileOrigin;
        [SerializeField] private LayerMask enemyLayerMask = ~0;

        private float _nextAttackTime;

        private void Awake() => _targetBuffer = new Collider[TargetBufferSize];

        private void Update()
        {
            if (data == null || Time.time < _nextAttackTime) return;

            var target = FindLeadingTarget();
            if (target == null) return;

            Fire(target);
            _nextAttackTime = Time.time + data.AttackInterval;
        }

        public void Configure(UnitData unitData) => data = unitData;

        private EnemyAgent FindLeadingTarget()
        {
            var count = Physics.OverlapSphereNonAlloc(
                transform.position,
                data.AttackRange,
                _targetBuffer,
                enemyLayerMask,
                QueryTriggerInteraction.Collide);

            EnemyAgent best = null;
            var bestProgress = float.MinValue;

            for (var i = 0; i < count; i++)
            {
                if (_targetBuffer[i] == null) continue;
                var candidate = _targetBuffer[i].GetComponentInParent<EnemyAgent>();
                if (candidate == null || !candidate.Health.IsAlive || candidate.PathProgress <= bestProgress) continue;
                best = candidate;
                bestProgress = candidate.PathProgress;
            }

            return best;
        }

        private void Fire(EnemyAgent target)
        {
            if (data.ProjectilePrefab == null) return;

            var origin = projectileOrigin != null ? projectileOrigin : transform;
            var instance = Instantiate(data.ProjectilePrefab, origin.position, Quaternion.identity);
            var projectile = instance.GetComponent<Projectile>();

            if (projectile == null)
            {
                Destroy(instance);
                Debug.LogError($"Projectile prefab '{data.ProjectilePrefab.name}' needs a Projectile component.", this);
                return;
            }

            projectile.Launch(target.transform, target.Health, data.Damage, data.ProjectileSpeed);
        }

        private void OnDrawGizmosSelected()
        {
            if (data == null) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, data.AttackRange);
        }
    }
}
