using System;

namespace KingdomLastStand.Economy
{
    public sealed class CurrencyWallet
    {
        public int Gold { get; private set; }
        public int Gems { get; private set; }

        public event Action Changed;

        public CurrencyWallet(int gold = 0, int gems = 0)
        {
            if (gold < 0 || gems < 0)
                throw new ArgumentOutOfRangeException(nameof(gold), "Currency cannot be negative.");

            Gold = gold;
            Gems = gems;
        }

        public void AddGold(int amount)
        {
            ValidatePositive(amount);
            checked { Gold += amount; }
            Changed?.Invoke();
        }

        public void AddGems(int amount)
        {
            ValidatePositive(amount);
            checked { Gems += amount; }
            Changed?.Invoke();
        }

        public bool TrySpendGold(int amount)
        {
            ValidatePositive(amount);
            if (Gold < amount) return false;
            Gold -= amount;
            Changed?.Invoke();
            return true;
        }

        public bool TrySpendGems(int amount)
        {
            ValidatePositive(amount);
            if (Gems < amount) return false;
            Gems -= amount;
            Changed?.Invoke();
            return true;
        }

        private static void ValidatePositive(int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");
        }
    }
}

