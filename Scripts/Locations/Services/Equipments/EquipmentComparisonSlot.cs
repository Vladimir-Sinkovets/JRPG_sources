using Assets.Game.Scripts.Locations.Configs.Inventory;
using Assets.Game.Scripts.Locations.Services.InventoryPanel;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.Locations.Services.Equipments
{
    public class EquipmentComparisonSlot : MonoBehaviour
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private Image _icon;
        [SerializeField] private StatsComparisonPanel _statsPanel;
        [SerializeField] private AbilityListView _abilityListView;

        public void SetItem(InventoryItemConfig item, InventoryItemConfig itemCompareWith)
        {
            if (item == null)
            {
                Clear();
                return;
            }

            _title.text = item.Name;
            _icon.sprite = item.Sprite;
            _icon.color = Color.white;
            _statsPanel.SetStats(item?.Stats, itemCompareWith?.Stats);
            _abilityListView.SetAbilities(item.Abilities);
        }

        public void Clear()
        {
            _title.text = null;
            _icon.sprite = null;
            _icon.color = new Color(0, 0, 0, 0);
            _statsPanel.Clear();
            _abilityListView.Clear();
        }
    }
}
