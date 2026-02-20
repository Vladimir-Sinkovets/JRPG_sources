using Assets.Game.Scripts.Locations.Configs.Inventory;
using Assets.Game.Scripts.Locations.PlayerData.InventoryData;
using PixelCrushers.DialogueSystem.Wrappers;
using PixelCrushers.Wrappers;
using System;
using TriInspector;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Locations.LocationObjects
{
    [DeclareFoldoutGroup("Links", Title = "Links", Expanded = false)]
    public class ItemSource : MonoBehaviour
    {
        [InlineEditor(InlineEditorModes.FullEditor)]
        [SerializeField] private InventoryItemConfig _item;
        [Space]
        [Group("Links"), SerializeField] private Usable _usable;
        [Group("Links"), SerializeField] private ActiveSaver _activeSaver;
        [Group("Links"), SerializeField] private Light _light;
        [Group("Links"), SerializeField] private ParticleSystem _particleSystem;
        [Space]
        [SerializeField] private float _disappearanceDuration = 1;

        private Inventory _inventory;

        [Inject] private InventoryItemDataBase _dataBase;

        public string ItemName => _item.Name;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (EditorApplication.isPlaying || PrefabUtility.IsPartOfPrefabAsset(this))
                return;

            if (Guid.TryParse(_activeSaver.key, out var guid))
                return;

            if (_activeSaver != null && (_activeSaver.key == _activeSaver.name || string.IsNullOrEmpty(_activeSaver.key)))
            {
                _activeSaver.key = Guid.NewGuid().ToString();
                EditorUtility.SetDirty(this);
                PrefabUtility.RecordPrefabInstancePropertyModifications(this);
            }
        }
#endif

        private void Start()
        {
            if (!_dataBase.HasItem(_item))
            {
                _usable.gameObject.SetActive(false);

                return;
            }

            _inventory = FindAnyObjectByType<Inventory>();

            if (_usable != null && string.IsNullOrEmpty(_usable.overrideUseMessage))
                _usable.overrideUseMessage = _item.Name;

            if (_usable != null && string.IsNullOrEmpty(_usable.overrideName))
                _usable.overrideName = "Pick up item";
        }

        public void PickUpItem()
        {
            _inventory.AddItem(_item, 1);

            _usable.gameObject.SetActive(false);
        }
    }
}
