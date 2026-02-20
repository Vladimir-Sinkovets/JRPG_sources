using Assets.Game.Scripts.Locations.PlayerData.InventoryData;
using UnityEngine;

namespace Assets.Game.Scripts.Locations.Services.InventoryPanel
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private InventoryItemsGrid _inventoryItemsGrid;
        [SerializeField] private InventoryDescription _inventoryDescription;

        public void Activate()
        {
            _inventoryItemsGrid.Activate();
            _inventoryDescription.Clear();

            _inventoryItemsGrid.OnSelected += OnItemSelectedHandler;

            _inventoryItemsGrid.SelectFirstItem();
        }

        public void Deactivate()
        {
            _inventoryItemsGrid.Deactivate();
            _inventoryDescription.Clear();

            _inventoryItemsGrid.OnSelected -= OnItemSelectedHandler;
        }

        private void OnItemSelectedHandler(InventorySlot slot)
        {
            _inventoryDescription.SetSlot(slot);
        }

        private void OnDestroy()
        {
            if (_inventoryItemsGrid == null)
                return;

            _inventoryItemsGrid.OnSelected -= OnItemSelectedHandler;
        }
    }
}
