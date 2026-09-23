using System;

namespace KingdomLastStand.Economy
{
    public static class OfflineRewardCalculator
    {
        public static int Calculate(DateTime lastSeenUtc, DateTime nowUtc, int goldPerHour, int maximumHours)
        {
            if (goldPerHour < 0) throw new ArgumentOutOfRangeException(nameof(goldPerHour));
            if (maximumHours < 0) throw new ArgumentOutOfRangeException(nameof(maximumHours));
            if (nowUtc <= lastSeenUtc || goldPerHour == 0 || maximumHours == 0) return 0;

            var elapsedHours = Math.Min((nowUtc - lastSeenUtc).TotalHours, maximumHours);
            return checked((int)Math.Floor(elapsedHours * goldPerHour));
        }
    }
}

