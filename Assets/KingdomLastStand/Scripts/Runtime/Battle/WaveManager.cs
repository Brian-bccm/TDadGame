using System;
using System.Collections;
using System.Collections.Generic;
using KingdomLastStand.Data;
using KingdomLastStand.Enemies;
using UnityEngine;

namespace KingdomLastStand.Battle
{
    public sealed class WaveManager : MonoBehaviour
    {
        [SerializeField] private WaveData wave;
        [SerializeField] private Transform[] path;
        [SerializeField] private CastleController castle;

        private readonly HashSet<EnemyAgent> _aliveEnemies = new HashSet<EnemyAgent>();
        private bool _finishedSpawning;

        public event Action<EnemyAgent> EnemyKilled;
        public event Action WaveCompleted;

        public void Configure(WaveData waveData, Transform[] enemyPath, CastleController targetCastle)
        {
            wave = waveData;
            path = enemyPath;
            castle = targetCastle;
        }

        public void Begin()
        {
            if (wave == null || path == null || path.Length == 0 || castle == null)
                throw new InvalidOperationException("WaveManager is missing wave, path, or castle configuration.");

            StopAllCoroutines();
            _finishedSpawning = false;
            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            foreach (var group in wave.Groups)
            {
                if (group.Enemy == null || group.Enemy.Prefab == null) continue;

                if (group.DelayBeforeGroup > 0f)
                    yield return new WaitForSeconds(group.DelayBeforeGroup);

                for (var i = 0; i < group.Count; i++)
                {
                    Spawn(group.Enemy);
                    if (i + 1 < group.Count)
                        yield return new WaitForSeconds(group.Interval);
                }
            }

            _finishedSpawning = true;
            TryCompleteWave();
        }

        private void Spawn(EnemyData data)
        {
            var instance = Instantiate(data.Prefab, path[0].position, Quaternion.identity);
            var enemy = instance.GetComponent<EnemyAgent>();
            if (enemy == null)
            {
                Destroy(instance);
                Debug.LogError($"Enemy prefab '{data.Prefab.name}' needs an EnemyAgent component.", this);
                return;
            }

            enemy.Initialize(data, path, castle);
            enemy.Removed += HandleEnemyRemoved;
            _aliveEnemies.Add(enemy);
        }

        private void HandleEnemyRemoved(EnemyAgent enemy, bool wasKilled)
        {
            enemy.Removed -= HandleEnemyRemoved;
            _aliveEnemies.Remove(enemy);
            if (wasKilled) EnemyKilled?.Invoke(enemy);
            TryCompleteWave();
        }

        private void TryCompleteWave()
        {
            if (_finishedSpawning && _aliveEnemies.Count == 0)
                WaveCompleted?.Invoke();
        }
    }
}
