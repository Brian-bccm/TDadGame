using KingdomLastStand.Battle;
using KingdomLastStand.Combat;
using NUnit.Framework;
using UnityEngine;

namespace KingdomLastStand.Tests
{
    public sealed class BattleRulesTests
    {
        [Test]
        public void Damageable_ClampsHealthAtZeroAndRaisesDeathOnce()
        {
            var target = new GameObject("test-damageable");
            try
            {
                var health = target.AddComponent<Damageable>();
                health.Configure(10f);
                var deathCount = 0;
                health.Died += () => deathCount++;

                health.TakeDamage(25f);
                health.TakeDamage(1f);

                Assert.That(health.CurrentHealth, Is.Zero);
                Assert.That(health.IsAlive, Is.False);
                Assert.That(deathCount, Is.EqualTo(1));
            }
            finally
            {
                Object.DestroyImmediate(target);
            }
        }

        [Test]
        public void Castle_ReportsDestroyedAfterLethalHit()
        {
            var target = new GameObject("test-castle");
            try
            {
                var health = target.AddComponent<Damageable>();
                health.Configure(3f);
                var castle = target.AddComponent<CastleController>();
                var destroyed = false;
                castle.Destroyed += () => destroyed = true;

                castle.ReceiveEnemy(3);

                Assert.That(destroyed, Is.True);
            }
            finally
            {
                Object.DestroyImmediate(target);
            }
        }
    }
}

