using Assets.Game.Scripts.Common.Characters;
using Assets.Game.Scripts.Locations.Extensions;
using Assets.Game.Scripts.Locations.PlayerData;
using Assets.Game.Scripts.Locations.PlayerData.InventoryData;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Locations.Services.PlayerInfoPanels
{
    public class PlayerInfoPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _statsText;

        [Inject] private Inventory _inventory;
        [Inject] private PlayerStats _playerStats;

        public void Activate()
        {
            _inventory.OnInventoryChanged += OnInventoryChangedHandler;

            UpdateInfo();
        }

        public void Deactivate()
        {
            _inventory.OnInventoryChanged -= OnInventoryChangedHandler;
        }

        private void UpdateInfo()
        {
            var stats = _inventory.GetEquipmentStatsBonus();

            _statsText.text = string.Empty;

            _statsText.text += $"{_playerStats.Stats.Hp} + <color=green>{stats.Hp}</color>\n";
            _statsText.text += $"{_playerStats.Stats.Attack} + <color=green>{stats.Attack}</color>\n";
            _statsText.text += $"{_playerStats.Stats.MagicalAttack} + <color=green>{stats.MagicalAttack}</color>\n";
            _statsText.text += $"{_playerStats.Stats.Defence} + <color=green>{stats.Defence}</color>\n";
            _statsText.text += $"{_playerStats.Stats.MagicalDefence} + <color=green>{stats.MagicalDefence}</color>\n";
            _statsText.text += $"{_playerStats.Stats.Speed} + <color=green>{stats.Speed}</color>";
        }

        private void OnInventoryChangedHandler() => UpdateInfo();

        private void OnDestroy()
        {
            if ( _inventory != null ) _inventory.OnInventoryChanged -= OnInventoryChangedHandler;
        }
    }
}
