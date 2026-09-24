using System.Collections.Generic;
using KingdomLastStand.Economy;
using KingdomLastStand.Data;
using KingdomLastStand.Units;
using UnityEngine;

namespace KingdomLastStand.Demo
{
    public sealed class DemoBoard : MonoBehaviour
    {
        private readonly List<DemoUnitToken> _units = new List<DemoUnitToken>();
        private Vector3[] _slots;
        private CurrencyWallet _wallet;
        private UnitData _archer;
        private UnitData _mage;
        private UnitData _selected;
        private string _message = "Recruit units, then drag matching archers together to merge.";

        public int Gold => _wallet != null ? _wallet.Gold : 0;
        public string Message => _message;
        public UnitData SelectedData => _selected;
        public IReadOnlyList<DemoUnitToken> Units => _units;

        public void Configure(CurrencyWallet wallet, UnitData archer, UnitData mage, Vector3[] slots)
        {
            _wallet = wallet;
            _archer = archer;
            _mage = mage;
            _slots = slots;
            _wallet.Changed += HandleWalletChanged;
        }

        private void OnDestroy()
        {
            if (_wallet != null) _wallet.Changed -= HandleWalletChanged;
        }

        public bool BuyArcher() => Buy(_archer);
        public bool BuyMage() => Buy(_mage);

        public void Select(DemoUnitToken unit)
        {
            _selected = unit != null ? unit.Data : null;
            if (unit != null)
                _message = $"{unit.Data.name} — drag to an empty space to reposition, or onto a matching unit to merge.";
        }

        public void FinishDrag(DemoUnitToken unit, Vector3 releasePosition)
        {
            if (unit == null || !_units.Contains(unit)) return;

            var other = FindUnitNear(releasePosition, unit);
            if (other != null)
            {
                if (MergeRules.CanMerge(
                        new UnitIdentity(unit.Data.FamilyId, unit.Data.Tier),
                        new UnitIdentity(other.Data.FamilyId, other.Data.Tier),
                        highestStandardTier: 3))
                {
                    Merge(unit, other);
                    return;
                }

                Swap(unit, other);
                return;
            }

            var slot = FindNearestSlot(releasePosition);
            if (slot < 0)
            {
                unit.transform.position = _slots[unit.SlotIndex];
                _message = "Move units onto the formation area.";
                return;
            }

            var occupant = FindUnitAtSlot(slot, unit);
            if (occupant != null)
            {
                var oldSlot = unit.SlotIndex;
                unit.SlotIndex = slot;
                occupant.SlotIndex = oldSlot;
                unit.transform.position = _slots[slot];
                occupant.transform.position = _slots[oldSlot];
                _message = "Units repositioned.";
                return;
            }

            unit.SlotIndex = slot;
            unit.transform.position = _slots[slot];
            _message = "Unit repositioned.";
        }

        private bool Buy(UnitData data)
        {
            if (data == null) return false;
            var slot = FindFreeSlot();
            if (slot < 0)
            {
                _message = "Formation full. Drag units together to merge.";
                return false;
            }

            if (!_wallet.TrySpendGold(data.BuyCost))
            {
                _message = "Not enough gold.";
                return false;
            }

            SpawnUnit(data, slot);
            _message = $"{data.name} recruited.";
            return true;
        }

        private DemoUnitToken SpawnUnit(UnitData data, int slot)
        {
            var instance = Instantiate(data.Prefab, _slots[slot], Quaternion.identity);
            var tower = instance.GetComponent<UnitTower>();
            if (tower == null)
            {
                Destroy(instance);
                Debug.LogError($"Unit prefab '{data.Prefab.name}' needs a UnitTower component.", this);
                return null;
            }

            tower.Configure(data);
            var token = instance.GetComponent<DemoUnitToken>();
            if (token == null) token = instance.AddComponent<DemoUnitToken>();
            token.Initialize(this, data, slot);
            _units.Add(token);
            instance.SetActive(true);
            return token;
        }

        private void Merge(DemoUnitToken first, DemoUnitToken second)
        {
            var result = first.Data.MergeResult;
            if (result == null)
            {
                first.transform.position = _slots[first.SlotIndex];
                _message = "That unit has reached its top tier.";
                return;
            }

            var slot = first.SlotIndex;
            Remove(first);
            Remove(second);
            var upgraded = SpawnUnit(result, slot);
            if (upgraded != null)
            {
                upgraded.transform.localScale *= 1.16f;
                _selected = result;
                _message = $"Merge complete: {result.name}!";
            }
        }

        private void Swap(DemoUnitToken first, DemoUnitToken second)
        {
            var slot = first.SlotIndex;
            first.SlotIndex = second.SlotIndex;
            second.SlotIndex = slot;
            first.transform.position = _slots[first.SlotIndex];
            second.transform.position = _slots[second.SlotIndex];
            _message = "Units swapped places.";
        }

        private void Remove(DemoUnitToken token)
        {
            _units.Remove(token);
            if (_selected == token.Data) _selected = null;
            Destroy(token.gameObject);
        }

        private DemoUnitToken FindUnitNear(Vector3 point, DemoUnitToken except)
        {
            DemoUnitToken nearest = null;
            var bestDistance = 0.75f * 0.75f;
            foreach (var candidate in _units)
            {
                if (candidate == null || candidate == except) continue;
                var distance = (candidate.transform.position - point).sqrMagnitude;
                if (distance >= bestDistance) continue;
                bestDistance = distance;
                nearest = candidate;
            }

            return nearest;
        }

        private DemoUnitToken FindUnitAtSlot(int slot, DemoUnitToken except)
        {
            foreach (var unit in _units)
                if (unit != null && unit != except && unit.SlotIndex == slot)
                    return unit;
            return null;
        }

        private int FindNearestSlot(Vector3 point)
        {
            var bestIndex = -1;
            var bestDistance = 1.2f * 1.2f;
            for (var i = 0; i < _slots.Length; i++)
            {
                var distance = (_slots[i] - point).sqrMagnitude;
                if (distance >= bestDistance) continue;
                bestDistance = distance;
                bestIndex = i;
            }
            return bestIndex;
        }

        private int FindFreeSlot()
        {
            for (var i = 0; i < _slots.Length; i++)
                if (FindUnitAtSlot(i, null) == null)
                    return i;
            return -1;
        }

    }
}
