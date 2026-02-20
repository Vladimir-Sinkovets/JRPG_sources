using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.Common.Characters;
using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace Assets.Game.Scripts.Locations.Configs.Inventory
{
    [CreateAssetMenu(fileName = "Item", menuName = "Item")]
    public class InventoryItemConfig : ScriptableObject
    {
        [Required]
        [SerializeField] private string _id;
        [Required]
        [SerializeField] private string _name;
        [SpritePreview]
        [SerializeField] private Sprite _sprite;
        [Space]
        [SerializeField] private ItemType _itemType;
        
        [ShowIf(nameof(_itemType), ItemType.Equipment)]
        [SerializeField] private EquipmentType _itemEquipmentType;

        [HideIf(nameof(_itemType), ItemType.Equipment)]
        [SerializeField] private bool _isStackable = true;
        
        [Space]
        [ShowIf(nameof(_itemType), ItemType.Equipment)]
        [InlineProperty(LabelWidth = 90), LabelWidth(40)]
        [SerializeField] private Stats _stats;
        
        [Space]
        [ShowIf(nameof(_itemType), ItemType.Equipment)]
        [SerializeField] private List<BattleAbility> _abilities;

        [Required]
        [SerializeField, TextArea(5, 15)] private string _description;

        #region Id generation
        [Button("Generate unique id")]
        private void GenerateId()
        {
            _id = Guid.NewGuid().ToString();
        }
        #endregion

        public string Id { get => _id; }
        public Sprite Sprite { get => _sprite; }
        public string Description { get => _description; }
        public string Name { get => _name; }
        public Stats Stats { get => _stats; }
        public List<BattleAbility> Abilities { get => _abilities; }
        public bool IsStackable
        {
            get
            {
                if (_itemType == ItemType.Equipment)
                    return false;

                return _isStackable;
            }
        }
        public EquipmentType EquipmentType { get => _itemEquipmentType; }
        public ItemType ItemType { get => _itemType; }

#if UNITY_EDITOR
        public void Initialize(
            string id,
            string name,
            Sprite sprite,
            ItemType itemType,
            EquipmentType equipmentType,
            bool isStackable,
            Stats stats,
            List<BattleAbility> abilities,
            string description)
        {
            _id = id;
            _name = name;
            _sprite = sprite;
            _itemType = itemType;
            _itemEquipmentType = equipmentType;
            _isStackable = isStackable;
            _stats = stats;  // Может быть null для не-экипировки
            _abilities = abilities;
            _description = description;
        }
#endif
    }
}
