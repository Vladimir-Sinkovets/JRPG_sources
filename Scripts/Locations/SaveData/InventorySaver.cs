using Assets.Game.Scripts.Locations.Configs.Inventory;
using Assets.Game.Scripts.Locations.PlayerData.InventoryData;
using PixelCrushers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Locations.SaveData
{
    public class InventorySaver : Saver
    {
        [SerializeField] private Inventory _inventoryData;

        [Inject] private InventoryItemDataBase _inventoryItemDatabase;

        public override void ApplyData(string s)
        {
            var data = JsonUtility.FromJson<InventorySaveData>(s);

            if (data == null || data.Slots == null)
                return;

            var slots = data.Slots.Select(x => new InventorySlot()
            {
                Amount = x.Amount,
                Item = _inventoryItemDatabase.GetItemById(x.Id),
            });

            _inventoryData.SetSlots(slots);

            _inventoryData.SetEquipment(
                data.HeadSlot != -1 ? _inventoryData.Slots.ElementAt(data.HeadSlot) : null,
                data.ArmorSlot != -1 ? _inventoryData.Slots.ElementAt(data.ArmorSlot) : null,
                data.WeaponSlot != -1 ? _inventoryData.Slots.ElementAt(data.WeaponSlot) : null,
                data.Accessory1Slot != -1 ? _inventoryData.Slots.ElementAt(data.Accessory1Slot) : null,
                data.Accessory2Slot != -1 ? _inventoryData.Slots.ElementAt(data.Accessory2Slot) : null,
                data.Accessory3Slot != -1 ? _inventoryData.Slots.ElementAt(data.Accessory3Slot) : null,
                data.Accessory4Slot != -1 ? _inventoryData.Slots.ElementAt(data.Accessory4Slot) : null);
        }

        public override string RecordData()
        {
            var savingSlots = _inventoryData.Slots.Select(x => new InventorySlotSaveData()
                {
                    Amount = x.Amount,
                    Id = x.Item.Id,
                })
                .ToArray();

            var inventorySlots = _inventoryData.Slots.ToList();

            return JsonUtility.ToJson(new InventorySaveData()
            {
                Slots = savingSlots.ToList(),
                HeadSlot = _inventoryData.HeadSlot != null ? inventorySlots.IndexOf(_inventoryData.HeadSlot) : -1,
                ArmorSlot = _inventoryData.ArmorSlot != null ? inventorySlots.IndexOf(_inventoryData.ArmorSlot) : -1,
                WeaponSlot = _inventoryData.WeaponSlot != null ? inventorySlots.IndexOf(_inventoryData.WeaponSlot) : -1,
                Accessory1Slot = _inventoryData.Accessory1Slot != null ? inventorySlots.IndexOf(_inventoryData.Accessory1Slot) : -1,
                Accessory2Slot = _inventoryData.Accessory2Slot != null ? inventorySlots.IndexOf(_inventoryData.Accessory2Slot) : -1,
                Accessory3Slot = _inventoryData.Accessory3Slot != null ? inventorySlots.IndexOf(_inventoryData.Accessory3Slot) : -1,
                Accessory4Slot = _inventoryData.Accessory4Slot != null ? inventorySlots.IndexOf(_inventoryData.Accessory4Slot) : -1,
            });
        }
        [Serializable]
        private class InventorySaveData
        {
            public List<InventorySlotSaveData> Slots;

            public int HeadSlot = -1;
            public int ArmorSlot = -1;
            public int WeaponSlot = -1;
            public int Accessory1Slot = -1;
            public int Accessory2Slot = -1;
            public int Accessory3Slot = -1;
            public int Accessory4Slot = -1;
        }
        [Serializable]
        private class InventorySlotSaveData
        {
            public string Id;
            public int Amount;
        }
    }
}
