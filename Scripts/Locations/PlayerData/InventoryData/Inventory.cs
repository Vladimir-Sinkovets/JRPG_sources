using Assets.Game.Scripts.Locations.Configs.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEngine;

namespace Assets.Game.Scripts.Locations.PlayerData.InventoryData
{
    public class Inventory: MonoBehaviour
    {
        public event Action OnInventoryChanged;
        public event Action<ItemAddedEventArgs> OnItemAdded;

        private Dictionary<EquipmentSlotType, (EquipmentType itemEquipmentType, Action<InventorySlot> action)> _slotHandlers;

        [SerializeField, DisableInEditMode]
        private List<InventorySlot> _slots = new List<InventorySlot>();

        private InventorySlot _headSlot;
        private InventorySlot _armorSlot;
        private InventorySlot _weaponSlot;
        private InventorySlot _accessory1Slot;
        private InventorySlot _accessory2Slot;
        private InventorySlot _accessory3Slot;
        private InventorySlot _accessory4Slot;

        #region props
        public IEnumerable<InventorySlot> Slots { get => _slots; }
        public InventorySlot HeadSlot { get => _headSlot; }
        public InventorySlot ArmorSlot { get => _armorSlot; }
        public InventorySlot WeaponSlot { get => _weaponSlot; }
        public InventorySlot Accessory1Slot { get => _accessory1Slot; }
        public InventorySlot Accessory2Slot { get => _accessory2Slot; }
        public InventorySlot Accessory3Slot { get => _accessory3Slot; }
        public InventorySlot Accessory4Slot { get => _accessory4Slot; }
        #endregion

        private void Start()
        {
            _slotHandlers =
                new Dictionary<EquipmentSlotType, (EquipmentType, Action<InventorySlot>)>
                {
                    { EquipmentSlotType.Head,       new (EquipmentType.Head,      slot => _headSlot = slot) },
                    { EquipmentSlotType.Armor,      new (EquipmentType.Armor,     slot => _armorSlot = slot) },
                    { EquipmentSlotType.Weapon,     new (EquipmentType.Weapon,    slot => _weaponSlot = slot) },
                    { EquipmentSlotType.Accessory1, new (EquipmentType.Accessory, slot => _accessory1Slot = slot) },
                    { EquipmentSlotType.Accessory2, new (EquipmentType.Accessory, slot => _accessory2Slot = slot) },
                    { EquipmentSlotType.Accessory3, new (EquipmentType.Accessory, slot => _accessory3Slot = slot) },
                    { EquipmentSlotType.Accessory4, new (EquipmentType.Accessory, slot => _accessory4Slot = slot) }
                };
        }

        public void AddItem(InventoryItemConfig item, int amount)
        {
            if (_slots == null)
                _slots = new List<InventorySlot>();

            var existingSlot = _slots.FirstOrDefault(x => x.Item == item);

            if (existingSlot != null && item.IsStackable == true)
            {
                existingSlot.Amount += amount;
            }
            else
            {
                _slots.Add(new InventorySlot()
                {
                    Item = item,
                    Amount = amount,
                });
            }

            OnItemAdded?.Invoke(new ItemAddedEventArgs() { Item = item, Amount = amount});
            OnInventoryChanged?.Invoke();
        }

        public void SetSlots(IEnumerable<InventorySlot> newSlots)
        {
            _slots = newSlots.ToList();

            OnInventoryChanged?.Invoke();
        }

        public void EquipItem(EquipmentSlotType slotType, InventorySlot inventorySlot)
        {
            if (inventorySlot == null && !_slots.Contains(inventorySlot))
            {
                Debug.LogError($"Item does not exist");
                return;
            }

            if (_slotHandlers.TryGetValue(slotType, out var handler))
            {
                if (inventorySlot.Item.EquipmentType == handler.itemEquipmentType)
                {
                    UnequipItem(inventorySlot);

                    handler.action(inventorySlot);

                    OnInventoryChanged?.Invoke();
                }
            }
            else
            {
                Debug.LogError($"Does not have implementation for slot {slotType}");
            }
        }

        public bool IsItemEquiped(InventorySlot slot)
        {
            if (_headSlot == slot) return true;
            if (_armorSlot == slot) return true;
            if (_weaponSlot == slot) return true;
            if (_accessory1Slot == slot) return true;
            if (_accessory2Slot == slot) return true;
            if (_accessory3Slot == slot) return true;
            if (_accessory4Slot == slot) return true;

            return false;
        }

        public void SetEquipment(
            InventorySlot headSlot,
            InventorySlot armorSlot,
            InventorySlot weaponSlot,
            InventorySlot accessory1Slot,
            InventorySlot accessory2Slot,
            InventorySlot accessory3Slot,
            InventorySlot accessory4Slot)
        {
            _headSlot = headSlot;
            _armorSlot = armorSlot;
            _weaponSlot = weaponSlot;
            _accessory1Slot = accessory1Slot;
            _accessory2Slot = accessory2Slot;
            _accessory3Slot = accessory3Slot;
            _accessory4Slot = accessory4Slot;
        }

        private void UnequipItem(InventorySlot slot)
        {
            if (_headSlot == slot) _headSlot = null;
            if (_armorSlot == slot) _armorSlot = null;
            if (_weaponSlot == slot) _weaponSlot = null;
            if (_accessory1Slot == slot) _accessory1Slot = null;
            if (_accessory2Slot == slot) _accessory2Slot = null;
            if (_accessory3Slot == slot) _accessory3Slot = null;
            if (_accessory4Slot == slot) _accessory4Slot = null;
        }
    }

    public class ItemAddedEventArgs
    {
        public InventoryItemConfig Item;
        public int Amount;
    }
}