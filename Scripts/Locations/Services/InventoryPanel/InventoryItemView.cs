using Assets.Game.Scripts.Locations.PlayerData.InventoryData;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.Locations.Services.InventoryPanel
{
    public class InventoryItemView : MonoBehaviour
    {
        public event Action<InventorySlot> OnSelected;
        public event Action<InventorySlot> OnClicked;

        [SerializeField] private Image _icon;
        [SerializeField] private Image _background;
        [SerializeField] private Image _border;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _amount;
        [Space]
        [SerializeField] private Selectable _selectable;

        private InventorySlot _slot;
        private Color _originalBackgroundColor;

        public Selectable Selectable { get => _selectable; }

        public void Init()
        {
            _originalBackgroundColor = _background.color;
        }

        public void SetItem(InventorySlot slot)
        {
            _icon.sprite = slot.Item.Sprite;
            _name.text = slot.Item.Name;

            _amount.text = slot.Item.IsStackable ? slot.Amount.ToString() : string.Empty;

            _slot = slot;
        }

        public void Clear()
        {
            _slot = null;
            OnSelected = null;
            OnClicked = null;

            _border.enabled = false;

            _background.color = _originalBackgroundColor;
        }

        public void OnClickedHandler() => OnClicked?.Invoke(_slot);

        public void OnSelectedHandler()
        {
            OnSelected?.Invoke(_slot);

            _border.enabled = true;
        }

        public void OnDeselectedHandler()
        {
            _border.enabled = false;
        }

        public void SetBackColor(Color color)
        {
            _background.color = color;
        }
    }
}