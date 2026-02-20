using Assets.Game.Scripts.BattleSystem.Unit;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.Services.Highlighter
{
    public class UnitHighlighter : MonoBehaviour, IUnitHighlighter
    {
        [SerializeField] private HighlightersConfig _config;

        private Dictionary<BattleUnit, Dictionary<HighlighterType, GameObject>> _highlighters = new();

        public void Highlight(BattleUnit unit, HighlighterType type)
        {
            if (_highlighters.ContainsKey(unit) == false)
            {
                _highlighters.Add(unit, new Dictionary<HighlighterType, GameObject>());
            }

            if (_highlighters[unit].ContainsKey(type) == false)
            {
                var highlighter = GetHighlighterInstance(type, unit.transform);

                highlighter.transform.position = unit.transform.position;

                _highlighters[unit].Add(type, highlighter);
            }

            _highlighters[unit][type].SetActive(true);
        }

        public void HighlightRange(IEnumerable<BattleUnit> units, HighlighterType type)
        {
            foreach (BattleUnit unit in units)
            {
                Highlight(unit, type);
            }
        }

        public void RemoveHighlight(BattleUnit unit)
        {
            if (_highlighters.ContainsKey(unit) == false || _highlighters[unit].Count() == 0)
            {
                Debug.LogWarning($"Unit has no highlighters, {unit}");
                return;
            }

            foreach (var item in _highlighters[unit])
            {
                item.Value.SetActive(false);
            }
        }

        public void RemoveHighlightsRange(IEnumerable<BattleUnit> units)
        {
            foreach (BattleUnit unit in units)
            {
                RemoveHighlight(unit);
            }
        }

        public void RemoveAllHighlights()
        {
            foreach (var item in _highlighters)
            {
                RemoveHighlight(item.Key);
            }
        }

        public void RemoveHighlightByType(BattleUnit unit, HighlighterType type)
        {
            if (_highlighters.ContainsKey(unit) == false || _highlighters[unit].Count() == 0)
            {
                Debug.LogWarning($"Unit has no highlighters, {unit}");
                return;
            }

            var highlighter = _highlighters[unit]
                .FirstOrDefault(x => x.Key == type).Value;

            if (highlighter == null)
            {
                Debug.LogWarning($"Unit has no highlighter of {type} type, {unit}");
                return;
            }

            highlighter.SetActive(false);
        }

        public void RemoveHighlightsRangeByType(IEnumerable<BattleUnit> units, HighlighterType type)
        {
            foreach (BattleUnit unit in units)
            {
                RemoveHighlightByType(unit, type);
            }
        }
        private GameObject GetHighlighterInstance(HighlighterType type, Transform parent)
        {
            return Instantiate(_config.GetPrefab(type), parent);
        }
    }
}