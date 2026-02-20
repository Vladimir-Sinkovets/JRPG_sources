using Assets.Game.Scripts.Common.UI.Panels;
using Assets.Game.Scripts.Locations.Configs.Inventory;
using Assets.Game.Scripts.Locations.Extensions;
using Assets.Game.Scripts.Locations.PlayerData.InventoryData;
using Assets.Game.Scripts.Locations.Services.InventoryPanel;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Game.Scripts.Locations.Services.Equipments
{
    public class InventoryItemChoicePanel : MonoBehaviour
    {
        public event Action<EquipmentSlot, InventorySlot> OnItemChosen;

        [SerializeField] private InventoryItemsGrid _inventoryItemsGrid;
        [SerializeField] private PanelBase _panelBase;
        [Space]
        [SerializeField] private EquipmentComparisonSlot _currentEquipmentComparisonSlot;
        [SerializeField] private EquipmentComparisonSlot _newEquipmentComparisonSlot;
        [SerializeField] private TMP_Text _typeTitle;

        private EquipmentSlot _currentEquipmentSlot;

        public void SetSlot(EquipmentSlot slot) => _currentEquipmentSlot = slot;

        public void Activate()
        {
            _typeTitle.text = _currentEquipmentSlot.EquipmentSlotType.GetName();

            _currentEquipmentComparisonSlot.Clear();
            _newEquipmentComparisonSlot.Clear();

            _inventoryItemsGrid.SetFilter(new InventoryItemsFilter()
            {
                ItemType = ItemType.Equipment,
                EquipmentType = _currentEquipmentSlot.EquipmentSlotType.MapToEquipmentType(),
            });

            _inventoryItemsGrid.OnSelected += OnSelectedHandler;
            _inventoryItemsGrid.OnClicked += OnClickedHandler;

            _inventoryItemsGrid.Activate();
            _inventoryItemsGrid.SelectFirstItem();
        }

        public void Deactivate()
        {
            _inventoryItemsGrid.Deactivate();

            _inventoryItemsGrid.OnSelected -= OnSelectedHandler;
            _inventoryItemsGrid.OnClicked -= OnClickedHandler;
        }

        private void OnClickedHandler(InventorySlot itemSlot)
        {
            OnItemChosen?.Invoke(_currentEquipmentSlot, itemSlot);

            _panelBase.Close();
        }

        private void OnSelectedHandler(InventorySlot slot)
        {
            _currentEquipmentComparisonSlot.SetItem(_currentEquipmentSlot.InventorySlot?.Item, slot.Item);
            _newEquipmentComparisonSlot.SetItem(slot.Item, _currentEquipmentSlot.InventorySlot?.Item);
        }

        private void OnDestroy()
        {
            if (_inventoryItemsGrid != null)
            {
                _inventoryItemsGrid.OnSelected -= OnSelectedHandler;
                _inventoryItemsGrid.OnClicked -= OnClickedHandler;
            }
        }
    }
}
