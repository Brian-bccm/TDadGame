using UnityEngine;
using KingdomLastStand.Data;

namespace KingdomLastStand.Demo
{
    [RequireComponent(typeof(Collider))]
    public sealed class DemoUnitToken : MonoBehaviour
    {
        public DemoBoard Board { get; private set; }
        public int SlotIndex { get; set; }
        public UnitData Data { get; private set; }

        private Vector3 _pointerOffset;
        private Vector3 _originalPosition;
        private bool _dragged;

        public void Initialize(DemoBoard board, UnitData data, int slotIndex)
        {
            Board = board;
            Data = data;
            SlotIndex = slotIndex;
            _originalPosition = transform.position;
        }

        private void OnMouseDown()
        {
            if (Board == null || Camera.main == null) return;
            _originalPosition = transform.position;
            _dragged = false;
            if (TryGetPointerPosition(out var pointer))
                _pointerOffset = transform.position - pointer;
            Board.Select(this);
        }

        private void OnMouseDrag()
        {
            if (Board == null || Camera.main == null) return;
            if (TryGetPointerPosition(out var pointer))
            {
                var next = pointer + _pointerOffset;
                _dragged |= (next - _originalPosition).sqrMagnitude > 0.025f;
                if (_dragged) transform.position = next;
            }
        }

        private void OnMouseUp()
        {
            if (Board == null || !_dragged) return;
            if (TryGetPointerPosition(out var pointer))
                Board.FinishDrag(this, pointer + _pointerOffset);
        }

        private static bool TryGetPointerPosition(out Vector3 position)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var plane = new Plane(Vector3.forward, Vector3.zero);
            if (plane.Raycast(ray, out var distance))
            {
                position = ray.GetPoint(distance);
                return true;
            }

            position = default;
            return false;
        }
    }
}
