using UnityEngine;

namespace KingdomLastStand.Data
{
    [CreateAssetMenu(menuName = "Kingdom Last Stand/Enemy", fileName = "Enemy_")]
    public sealed class EnemyData : ScriptableObject
    {
        [SerializeField] private string id = "enemy.basic";
        [SerializeField] private string displayName = "Footman";
        [SerializeField] private GameObject prefab;
        [SerializeField, Min(1f)] private float maximumHealth = 25f;
        [SerializeField, Min(0.1f)] private float movementSpeed = 1.5f;
        [SerializeField, Min(1)] private int castleDamage = 1;
        [SerializeField, Min(0)] private int goldReward = 5;

        public string Id => id;
        public string DisplayName => displayName;
        public GameObject Prefab => prefab;
        public float MaximumHealth => maximumHealth;
        public float MovementSpeed => movementSpeed;
        public int CastleDamage => castleDamage;
        public int GoldReward => goldReward;

        public void Configure(string enemyId, string name, GameObject enemyPrefab, float health, float speed, int damage, int reward)
        {
            id = enemyId;
            displayName = name;
            prefab = enemyPrefab;
            maximumHealth = Mathf.Max(1f, health);
            movementSpeed = Mathf.Max(0.1f, speed);
            castleDamage = Mathf.Max(1, damage);
            goldReward = Mathf.Max(0, reward);
        }
    }
}
