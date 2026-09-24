using System;
using System.Collections.Generic;
using UnityEngine;

namespace KingdomLastStand.Data
{
    [CreateAssetMenu(menuName = "Kingdom Last Stand/Wave", fileName = "Wave_")]
    public sealed class WaveData : ScriptableObject
    {
        [Serializable]
        public sealed class SpawnGroup
        {
            [SerializeField] private EnemyData enemy;
            [SerializeField, Min(1)] private int count = 5;
            [SerializeField, Min(0.05f)] private float interval = 0.6f;
            [SerializeField, Min(0f)] private float delayBeforeGroup;

            public EnemyData Enemy => enemy;
            public int Count => count;
            public float Interval => interval;
            public float DelayBeforeGroup => delayBeforeGroup;

            public SpawnGroup(EnemyData enemyData, int enemyCount, float spawnInterval, float groupDelay = 0f)
            {
                enemy = enemyData;
                count = Mathf.Max(1, enemyCount);
                interval = Mathf.Max(0.05f, spawnInterval);
                delayBeforeGroup = Mathf.Max(0f, groupDelay);
            }
        }

        [SerializeField] private List<SpawnGroup> groups = new List<SpawnGroup>();
        public IReadOnlyList<SpawnGroup> Groups => groups;

        public void Configure(List<SpawnGroup> spawnGroups)
        {
            groups = spawnGroups ?? new List<SpawnGroup>();
        }
    }
}
