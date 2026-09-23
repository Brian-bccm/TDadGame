namespace KingdomLastStand.Units
{
    public readonly struct UnitIdentity
    {
        public string FamilyId { get; }
        public int Tier { get; }
        public bool IsFinalEvolution { get; }

        public UnitIdentity(string familyId, int tier, bool isFinalEvolution = false)
        {
            FamilyId = familyId;
            Tier = tier;
            IsFinalEvolution = isFinalEvolution;
        }
    }

    public static class MergeRules
    {
        public static bool CanMerge(UnitIdentity first, UnitIdentity second, int highestStandardTier)
        {
            if (string.IsNullOrWhiteSpace(first.FamilyId) || string.IsNullOrWhiteSpace(second.FamilyId))
                return false;

            return first.FamilyId == second.FamilyId
                   && first.Tier == second.Tier
                   && first.Tier > 0
                   && first.Tier < highestStandardTier
                   && !first.IsFinalEvolution
                   && !second.IsFinalEvolution;
        }
    }
}

