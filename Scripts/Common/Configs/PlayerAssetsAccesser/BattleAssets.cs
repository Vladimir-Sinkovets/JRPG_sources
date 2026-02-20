using Assets.Game.Scripts.BattleSystem.Unit.UI;
using TriInspector;
using UnityEngine;

namespace Assets.Game.Scripts.Common.Configs.PlayerAssetsAccesser
{
    [CreateAssetMenu(fileName = "Battle_assets", menuName = "Battle/Assets")]
    public class BattleAssets : ScriptableObject
    {
        [SerializeField] private GameObject _playerPrefab;
        [Title("Unit prefabs")]
        [SerializeField] private UnitUI _unitUI;
        [SerializeField] private FloatingDamageMediator _floatingDamage;

        public GameObject PlayerPrefab { get => _playerPrefab; }
        public UnitUI UnitUI { get => _unitUI; }
        public FloatingDamageMediator FloatingDamage { get => _floatingDamage; }
    }
}