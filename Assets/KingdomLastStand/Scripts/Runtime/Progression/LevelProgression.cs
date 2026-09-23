using System;
using System.Collections.Generic;

namespace KingdomLastStand.Progression
{
    public sealed class LevelProgression
    {
        private readonly HashSet<int> _completed = new HashSet<int>();

        public int HighestUnlockedLevel { get; private set; } = 1;
        public IReadOnlyCollection<int> CompletedLevels => _completed;

        public bool IsUnlocked(int levelNumber) => levelNumber > 0 && levelNumber <= HighestUnlockedLevel;

        public bool Complete(int levelNumber, int maximumLevel)
        {
            if (levelNumber <= 0 || levelNumber > maximumLevel)
                throw new ArgumentOutOfRangeException(nameof(levelNumber));
            if (!IsUnlocked(levelNumber)) return false;

            var wasNew = _completed.Add(levelNumber);
            if (wasNew && levelNumber < maximumLevel)
                HighestUnlockedLevel = Math.Max(HighestUnlockedLevel, levelNumber + 1);

            return wasNew;
        }
    }
}

