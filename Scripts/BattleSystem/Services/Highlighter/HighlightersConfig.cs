using System;
using UnityEngine;

namespace Assets.Game.Scripts.BattleSystem.Services.Highlighter
{
    [CreateAssetMenu(fileName = "Highlighters config", menuName = "Battle/Highlighters config")]
    public class HighlightersConfig : ScriptableObject
    {
        [SerializeField] private GameObject _canBeTargetPrefab;
        [SerializeField] private GameObject _targetPrefab;
        [SerializeField] private GameObject _selectedUnitPrefab;

        public GameObject GetPrefab(HighlighterType type)
        {
            return type switch
            {
                HighlighterType.CanBeTarget => _canBeTargetPrefab,
                HighlighterType.Target => _targetPrefab,
                HighlighterType.SelectedUnit => _selectedUnitPrefab,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
