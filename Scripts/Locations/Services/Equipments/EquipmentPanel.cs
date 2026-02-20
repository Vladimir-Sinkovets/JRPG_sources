using Assets.Game.Scripts.Common.UI.Panels;
using Assets.Game.Scripts.Locations.PlayerData.InventoryData;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Assets.Game.Scripts.Locations.Services.Equipments
{
    public class EquipmentPanel : MonoBehaviour
    {
        [SerializeField] private EquipmentSlot _headSlot;
        [SerializeField] private EquipmentSlot _armorSlot;
        [SerializeField] private EquipmentSlot _weaponSlot;
        [SerializeField] private EquipmentSlot _accessory1Slot;
        [SerializeField] private EquipmentSlot _accessory2Slot;
        [SerializeField] private EquipmentSlot _accessory3Slot;
        [SerializeField] private EquipmentSlot _accessory4Slot;
        [Space]
        [SerializeField] private InventoryItemChoicePanel _itemChoice;
        [SerializeField] private PanelBase _choicePanel;
        [Space]
        [SerializeField] private PanelBase _panel;

        [Inject] private Inventory _inventory;

        public void Start()
        {
            _headSlot.OnClicked += OnSlotClickedHandler;
            _armorSlot.OnClicked += OnSlotClickedHandler;
            _weaponSlot.OnClicked += OnSlotClickedHandler;
            _accessory1Slot.OnClicked += OnSlotClickedHandler;
            _accessory2Slot.OnClicked += OnSlotClickedHandler;
            _accessory3Slot.OnClicked += OnSlotClickedHandler;
            _accessory4Slot.OnClicked += OnSlotClickedHandler;

            _itemChoice.OnItemChosen += OnItemChosenHandler;

            _headSlot.EquipmentSlotType = EquipmentSlotType.Head;
            _armorSlot.EquipmentSlotType = EquipmentSlotType.Armor;
            _weaponSlot.EquipmentSlotType = EquipmentSlotType.Weapon;
            _accessory1Slot.EquipmentSlotType = EquipmentSlotType.Accessory1;
            _accessory2Slot.EquipmentSlotType = EquipmentSlotType.Accessory2;
            _accessory3Slot.EquipmentSlotType = EquipmentSlotType.Accessory3;
            _accessory4Slot.EquipmentSlotType = EquipmentSlotType.Accessory4;
        }

        private void OnItemChosenHandler(EquipmentSlot equipmentSlot, InventorySlot inventorySlot)
        {
            _inventory.EquipItem(equipmentSlot.EquipmentSlotType, inventorySlot);
        }

        private void OnSlotClickedHandler(EquipmentSlot slot)
        {
            _panel.LastSelected = EventSystem.current.currentSelectedGameObject;

            _itemChoice.SetSlot(slot);

            _panel.UnSelect();
            _choicePanel.OnPanelClosed += OnPanelClosedHandler;
            _choicePanel.Show();
            _choicePanel.Select();
        }

        public void Activate()
        {
            UpdateSlots();
        }

        public void Deactivate()
        {
        }

        private void UpdateSlots()
        {
            _headSlot.Clear();
            _armorSlot.Clear();
            _weaponSlot.Clear();
            _accessory1Slot.Clear();
            _accessory2Slot.Clear();
            _accessory3Slot.Clear();
            _accessory4Slot.Clear();

            _headSlot.SetSlot(_inventory.HeadSlot);
            _armorSlot.SetSlot(_inventory.ArmorSlot);
            _weaponSlot.SetSlot(_inventory.WeaponSlot);
            _accessory1Slot.SetSlot(_inventory.Accessory1Slot);
            _accessory2Slot.SetSlot(_inventory.Accessory2Slot);
            _accessory3Slot.SetSlot(_inventory.Accessory3Slot);
            _accessory4Slot.SetSlot(_inventory.Accessory4Slot);
        }

        private void OnPanelClosedHandler(PanelBase panel)
        {
            panel.OnPanelClosed -= OnPanelClosedHandler;

            panel.UnSelect();

            _panel.Select();

            UpdateSlots();
        }

        private void OnDestroy()
        {
            _headSlot.OnClicked -= OnSlotClickedHandler;
            _armorSlot.OnClicked -= OnSlotClickedHandler;
            _weaponSlot.OnClicked -= OnSlotClickedHandler;
            _accessory1Slot.OnClicked -= OnSlotClickedHandler;
            _accessory2Slot.OnClicked -= OnSlotClickedHandler;
            _accessory3Slot.OnClicked -= OnSlotClickedHandler;
            _accessory4Slot.OnClicked -= OnSlotClickedHandler;

            _itemChoice.OnItemChosen -= OnItemChosenHandler;
        }
    }
}
