using UnityEngine;

namespace KingdomLastStand.Data
{
    [CreateAssetMenu(menuName = "Kingdom Last Stand/Unit", fileName = "Unit_")]
    public sealed class UnitData : ScriptableObject
    {
        [SerializeField] private string id = "unit.archer.t1";
        [SerializeField] private string familyId = "archer";
        [SerializeField, Min(1)] private int tier = 1;
        [SerializeField] private GameObject prefab;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField, Min(0.1f)] private float damage = 5f;
        [SerializeField, Min(0.05f)] private float attackInterval = 0.8f;
        [SerializeField, Min(0.1f)] private float attackRange = 4f;
        [SerializeField, Min(0.1f)] private float projectileSpeed = 12f;
        [SerializeField, Min(0)] private int buyCost = 20;
        [SerializeField] private UnitData mergeResult;

        public string Id => id;
        public string FamilyId => familyId;
        public int Tier => tier;
        public GameObject Prefab => prefab;
        public GameObject ProjectilePrefab => projectilePrefab;
        public float Damage => damage;
        public float AttackInterval => attackInterval;
        public float AttackRange => attackRange;
        public float ProjectileSpeed => projectileSpeed;
        public int BuyCost => buyCost;
        public UnitData MergeResult => mergeResult;

        public void Configure(string unitId, string unitFamilyId, int unitTier, GameObject unitPrefab,
            GameObject projectilePrefab, float unitDamage, float interval, float range, float shotSpeed,
            int cost, UnitData nextTier)
        {
            id = unitId;
            familyId = unitFamilyId;
            tier = Mathf.Max(1, unitTier);
            prefab = unitPrefab;
            this.projectilePrefab = projectilePrefab;
            damage = Mathf.Max(0.1f, unitDamage);
            attackInterval = Mathf.Max(0.05f, interval);
            attackRange = Mathf.Max(0.1f, range);
            projectileSpeed = Mathf.Max(0.1f, shotSpeed);
            buyCost = Mathf.Max(0, cost);
            mergeResult = nextTier;
        }
    }
}
