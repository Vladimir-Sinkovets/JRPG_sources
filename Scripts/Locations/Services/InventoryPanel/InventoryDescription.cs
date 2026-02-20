using Assets.Game.Scripts.Locations.Configs.Inventory;
using Assets.Game.Scripts.Locations.PlayerData.InventoryData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.Locations.Services.InventoryPanel
{
    public class InventoryDescription : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _iconBorders;
        [SerializeField] private TMP_Text _typeLabel;
        [SerializeField] private TMP_Text _type;
        [SerializeField] private StatsPanel _statsPanel;
        [SerializeField] private AbilityListView _abilityListView;

        public void SetSlot(InventorySlot slot)
        {
            _name.text = slot.Item.Name;
            _description.text = slot.Item.Description;

            _icon.color = Color.white;
            _icon.sprite = slot.Item.Sprite;
            _iconBorders.enabled = true;

            _typeLabel.text = "Type:";

            if (slot.Item.ItemType == ItemType.Equipment)
            {
                _statsPanel.SetStats(slot.Item.Stats);
                _abilityListView.SetAbilities(slot.Item.Abilities);
                _type.text = $"Equipment({slot.Item.EquipmentType})";
            }
            else
            {
                _statsPanel.Clear();
                _abilityListView.Clear();
                _type.text = "-";
            }
        }

        public void Clear()
        {
            _name.text = string.Empty;
            _description.text = string.Empty;

            _typeLabel.text = string.Empty;
            _type.text = string.Empty;

            _icon.color = new Color(0, 0, 0, 0);
            _icon.sprite = null;
            _iconBorders.enabled = false;

            _statsPanel.Clear();
            _abilityListView.Clear();
        }
    }
}
