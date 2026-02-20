using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace Assets.Game.Scripts.Locations.Configs.Inventory
{
    [CreateAssetMenu(fileName = "Item_data_base", menuName = "Item database")]
    public class InventoryItemDataBase : ScriptableObject
    {
        [SerializeField]
        [InlineEditor]
        [ListDrawerSettings(
            AlwaysExpanded = true, 
            Draggable = true,
            HideAddButton = true)]
        private InventoryItemConfig[] _inventoryItemConfigs;

        public InventoryItemConfig GetItemById(string id)
        {
            var item = _inventoryItemConfigs.FirstOrDefault(x => x.Id == id);
            
            if (item == null)
                Debug.LogError($"Item {id} does not exist");

            return item;
        }

        public bool HasItem(InventoryItemConfig itemConfig)
        {
            if (itemConfig == null)
                return false;

            return _inventoryItemConfigs.Contains(itemConfig);
        }

#if UNITY_EDITOR
        public void AddItem(InventoryItemConfig item)
        {
            Undo.RecordObject(this, "Add item to database");
            List<InventoryItemConfig> newItems = new List<InventoryItemConfig>(_inventoryItemConfigs)
    {
        item
    };
            _inventoryItemConfigs = newItems.ToArray();
            EditorUtility.SetDirty(this);
        }
#endif
    }
}
