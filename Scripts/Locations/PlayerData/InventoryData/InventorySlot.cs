using Assets.Game.Scripts.Locations.Configs.Inventory;
using System;

namespace Assets.Game.Scripts.Locations.PlayerData.InventoryData
{
    [Serializable]
    public class InventorySlot
    {
        public int Amount;
        public InventoryItemConfig Item;
    }
}