using System;

namespace KingdomLastStand.Services
{
    public interface IRewardedAdService
    {
        bool IsReady { get; }
        void Initialize();
        void Show(string placementId, Action onRewardGranted, Action onUnavailable = null);
    }

    public sealed class MockRewardedAdService : IRewardedAdService
    {
        public bool IsReady { get; private set; }

        public void Initialize() => IsReady = true;

        public void Show(string placementId, Action onRewardGranted, Action onUnavailable = null)
        {
            if (!IsReady)
            {
                onUnavailable?.Invoke();
                return;
            }

            onRewardGranted?.Invoke();
        }
    }
}

