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
    }
}
