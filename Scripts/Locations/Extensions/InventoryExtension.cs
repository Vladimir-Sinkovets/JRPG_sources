using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.Common.Characters;
using Assets.Game.Scripts.Locations.PlayerData.InventoryData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Game.Scripts.Locations.Extensions
{
    public static class InventoryExtension
    {
        public static Stats GetEquipmentStatsBonus(this Inventory inventory)
        {
            var stats = new Stats();

            stats += inventory.HeadSlot != null ? inventory.HeadSlot.Item.Stats : Stats.Empty;
            stats += inventory.ArmorSlot != null ? inventory.ArmorSlot.Item.Stats : Stats.Empty;
            stats += inventory.WeaponSlot != null ? inventory.WeaponSlot.Item.Stats : Stats.Empty;
            stats += inventory.Accessory1Slot != null ? inventory.Accessory1Slot.Item.Stats : Stats.Empty;
            stats += inventory.Accessory2Slot != null ? inventory.Accessory2Slot.Item.Stats : Stats.Empty;
            stats += inventory.Accessory3Slot != null ? inventory.Accessory3Slot.Item.Stats : Stats.Empty;
            stats += inventory.Accessory4Slot != null ? inventory.Accessory4Slot.Item.Stats : Stats.Empty;

            return stats;
        }

        public static IEnumerable<BattleAbility> GetEquipmentAbilities(this Inventory inventory)
        {
            if (inventory == null)
                throw new ArgumentNullException(nameof(inventory));

            var slots = new[]
            {
                inventory.HeadSlot,
                inventory.ArmorSlot,
                inventory.WeaponSlot,
                inventory.Accessory1Slot,
                inventory.Accessory2Slot,
                inventory.Accessory3Slot,
                inventory.Accessory4Slot
            };

            return slots
                .Where(slot => slot?.Item != null)
                .SelectMany(slot => slot.Item.Abilities ?? Enumerable.Empty<BattleAbility>());
        }
    }
}
