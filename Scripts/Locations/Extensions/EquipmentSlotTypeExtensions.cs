using Assets.Game.Scripts.Locations.Configs.Inventory;
using Assets.Game.Scripts.Locations.PlayerData.InventoryData;
using UnityEngine;

namespace Assets.Game.Scripts.Locations.Extensions
{
    public static class EquipmentSlotTypeExtensions
    {
        public static string GetName(this EquipmentSlotType equipmentSlotType)
        {
            var name = string.Empty;
            switch (equipmentSlotType)
            {
                case EquipmentSlotType.Head:
                    name = "Head";
                    break;
                case EquipmentSlotType.Armor:
                    name = "Armor";
                    break;
                case EquipmentSlotType.Weapon:
                    name = "Weapon";
                    break;
                case EquipmentSlotType.Accessory1:
                    name = "Accessory 1";
                    break;
                case EquipmentSlotType.Accessory2:
                    name = "Accessory 2";
                    break;
                case EquipmentSlotType.Accessory3:
                    name = "Accessory 3";
                    break;
                case EquipmentSlotType.Accessory4:
                    name = "Accessory 4";
                    break;
                default:
                    break;
            }

            return name;
        }

        public static EquipmentType MapToEquipmentType(this EquipmentSlotType equipmentSlotType)
        {
            switch (equipmentSlotType)
            {
                case EquipmentSlotType.Head:
                    return EquipmentType.Head;
                case EquipmentSlotType.Armor:
                    return EquipmentType.Armor;
                case EquipmentSlotType.Weapon:
                    return EquipmentType.Weapon;
                case EquipmentSlotType.Accessory1:
                    return EquipmentType.Accessory;
                case EquipmentSlotType.Accessory2:
                    return EquipmentType.Accessory;
                case EquipmentSlotType.Accessory3:
                    return EquipmentType.Accessory;
                case EquipmentSlotType.Accessory4:
                    return EquipmentType.Accessory;
                default:
                    Debug.LogError($"Does not have implementation for {equipmentSlotType}");
                    return EquipmentType.Accessory;
            }
        }

    }
}
