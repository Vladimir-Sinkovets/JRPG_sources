using Assets.Game.Scripts.BattleSystem.Unit;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Scripts.BattleSystem.Services.UnitPointers
{
    public class UnitPointer : MonoBehaviour, IUnitPointer
    {
        [SerializeField] private GameObject _pointer;
        [Space]
        [SerializeField] private float _scale;
        [SerializeField] private float _scaleDuration;
        [SerializeField] private Ease _scaleEase;

        private Dictionary<BattleUnit, PointerData> _pointersData = new();

        private class PointerData
        {
            public GameObject PointerInstance;
            public Sequence ScaleSequence;
            public Vector3 OriginalScale;
        }

        public void Clear()
        {
            foreach (var unit in _pointersData.Keys.ToList())
            {
                RemovePointer(unit);
            }

            _pointersData.Clear();
        }

        public void SetPointer(BattleUnit unit)
        {
            if (unit == null || _pointersData.ContainsKey(unit))
                return;

            var pointerInstance = Instantiate(_pointer);
            pointerInstance.SetActive(true);
            pointerInstance.transform.position = unit.transform.position;

            var originalScale = unit.transform.localScale;

            var scaleSequence = DOTween.Sequence();

            scaleSequence.Append(unit.transform.DOScale(_scale, _scaleDuration)
                            .SetEase(_scaleEase))
                         .Append(unit.transform.DOScale(1f, _scaleDuration).SetEase(_scaleEase))
                            .SetLoops(-1, LoopType.Restart);

            _pointersData.Add(unit, new PointerData
            {
                PointerInstance = pointerInstance,
                ScaleSequence = scaleSequence,
                OriginalScale = originalScale,
            });
        }

        public void RemovePointer(BattleUnit unit)
        {
            if (unit == null || !_pointersData.ContainsKey(unit))
                return;

            var data = _pointersData[unit];

            data.ScaleSequence?.Kill();

            if (data.PointerInstance != null)
                Destroy(data.PointerInstance);

            _pointersData.Remove(unit);

            unit.transform.localScale = data.OriginalScale;
        }

        private void OnDestroy()
        {
            Clear();
        }
    }
}
