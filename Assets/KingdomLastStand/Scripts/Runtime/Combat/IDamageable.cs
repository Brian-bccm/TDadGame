namespace KingdomLastStand.Combat
{
    public interface IDamageable
    {
        bool IsAlive { get; }
        float CurrentHealth { get; }
        float MaximumHealth { get; }
        void TakeDamage(float amount);
    }
}
