using System;
using KingdomLastStand.Economy;
using KingdomLastStand.Progression;
using KingdomLastStand.Units;
using NUnit.Framework;

namespace KingdomLastStand.Tests
{
    public sealed class CoreRulesTests
    {
        [Test]
        public void Wallet_DoesNotSpendWhenBalanceIsInsufficient()
        {
            var wallet = new CurrencyWallet(gold: 50);
            Assert.That(wallet.TrySpendGold(60), Is.False);
            Assert.That(wallet.Gold, Is.EqualTo(50));
        }

        [Test]
        public void Merge_RequiresSameFamilyAndTier()
        {
            var first = new UnitIdentity("archer", 1);
            var same = new UnitIdentity("archer", 1);
            var differentTier = new UnitIdentity("archer", 2);

            Assert.That(MergeRules.CanMerge(first, same, 5), Is.True);
            Assert.That(MergeRules.CanMerge(first, differentTier, 5), Is.False);
        }

        [Test]
        public void OfflineReward_IsCapped()
        {
            var lastSeen = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var reward = OfflineRewardCalculator.Calculate(lastSeen, lastSeen.AddHours(20), 100, 8);
            Assert.That(reward, Is.EqualTo(800));
        }

        [Test]
        public void CompletingLevel_UnlocksNextLevelOnce()
        {
            var progression = new LevelProgression();
            Assert.That(progression.Complete(1, 30), Is.True);
            Assert.That(progression.HighestUnlockedLevel, Is.EqualTo(2));
            Assert.That(progression.Complete(1, 30), Is.False);
        }

        [Test]
        public void CompletingLockedLevel_DoesNotUnlockIt()
        {
            var progression = new LevelProgression();
            Assert.That(progression.Complete(3, 30), Is.False);
            Assert.That(progression.HighestUnlockedLevel, Is.EqualTo(1));
        }

        [Test]
        public void OfflineReward_ReturnsZeroForFutureTimestamp()
        {
            var now = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            Assert.That(OfflineRewardCalculator.Calculate(now.AddHours(2), now, 100, 8), Is.Zero);
        }

        [Test]
        public void Merge_RejectsFinalEvolution()
        {
            var evolved = new UnitIdentity("archer", 5, isFinalEvolution: true);
            Assert.That(MergeRules.CanMerge(evolved, evolved, 6), Is.False);
        }
    }
}
