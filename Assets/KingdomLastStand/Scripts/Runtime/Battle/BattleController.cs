using System;
using UnityEngine;

namespace KingdomLastStand.Battle
{
    public enum BattleState
    {
        Ready,
        Running,
        Victory,
        Defeat
    }

    public sealed class BattleController : MonoBehaviour
    {
        [SerializeField] private WaveManager waveManager;
        [SerializeField] private CastleController castle;

        public BattleState State { get; private set; } = BattleState.Ready;
        public event Action<BattleState> StateChanged;

        public void ConfigureForDemo(WaveManager waves, CastleController targetCastle)
        {
            if (waveManager != null) waveManager.WaveCompleted -= HandleVictory;
            if (castle != null) castle.Destroyed -= HandleDefeat;
            waveManager = waves;
            castle = targetCastle;
            if (waveManager != null) waveManager.WaveCompleted += HandleVictory;
            if (castle != null) castle.Destroyed += HandleDefeat;
        }

        private void Awake()
        {
            if (waveManager != null) waveManager.WaveCompleted += HandleVictory;
            if (castle != null) castle.Destroyed += HandleDefeat;
        }

        private void OnDestroy()
        {
            if (waveManager != null) waveManager.WaveCompleted -= HandleVictory;
            if (castle != null) castle.Destroyed -= HandleDefeat;
        }

        public void StartBattle()
        {
            if (State != BattleState.Ready)
                throw new InvalidOperationException($"Cannot start battle from state {State}.");

            SetState(BattleState.Running);
            waveManager.Begin();
        }

        private void HandleVictory()
        {
            if (State == BattleState.Running)
                SetState(BattleState.Victory);
        }

        private void HandleDefeat()
        {
            if (State == BattleState.Running)
                SetState(BattleState.Defeat);
        }

        private void SetState(BattleState state)
        {
            State = state;
            StateChanged?.Invoke(state);
        }
    }
}
