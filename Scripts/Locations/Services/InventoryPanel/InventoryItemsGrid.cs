using Assets.Game.Scripts.Common.UI;
using Assets.Game.Scripts.Locations.Configs.Inventory;
using Assets.Game.Scripts.Locations.PlayerData.InventoryData;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Assets.Game.Scripts.Locations.Services.InventoryPanel
{
    public class InventoryItemsGrid : MonoBehaviour
    {
        public event Action<InventorySlot> OnSelected;
        public event Action<InventorySlot> OnClicked;

        [SerializeField] private InventoryItemView _uiItemPrefab;
        [SerializeField] private GridLayoutGroup _grid;

        [Inject] private Inventory _inventory;

        private List<InventoryItemView> _pool;
        private List<InventoryItemView> _currentItems;

        private InventoryItemsFilter _filter;

        public List<InventoryItemView> CurrentItems { get => _currentItems; }

        public void Activate()
        {
            _inventory.OnInventoryChanged += OnInventoryChangedHandler;

            UpdateView();
        }

        public void Deactivate()
        {
            _inventory.OnInventoryChanged -= OnInventoryChangedHandler;

            if (_currentItems != null)
                foreach (var item in _currentItems)
                    item.Clear();
        }

        public void SetFilter(InventoryItemsFilter filter)
        {
            _filter = filter;
        }

        public void UpdateView()
        {
            _pool ??= new List<InventoryItemView>();
            _currentItems ??= new List<InventoryItemView>();

            _currentItems.Clear();

            var slotIndex = 0;
            var activeSlotCount = 0;

            var slots = ApplyFilter(_inventory.Slots);

            foreach (var slot in slots)
            {
                if (slotIndex >= _pool.Count)
                {
                    var newItem = Instantiate(_uiItemPrefab, _grid.transform);
                    _pool.Add(newItem);

                    newItem.Init();
                }

                var itemView = _pool[slotIndex];

                itemView.Clear();

                _currentItems.Add(itemView);
                itemView.OnSelected -= OnSelectedHandler;
                itemView.OnSelected += OnSelectedHandler;

                itemView.OnClicked -= OnClickedHandler;
                itemView.OnClicked += OnClickedHandler;

                if (slot.Item != null)
                {
                    itemView.SetItem(slot);
                    itemView.gameObject.SetActive(true);

                    if (_inventory.IsItemEquiped(slot))
                        itemView.SetBackColor(new Color32(169, 169, 169, 255));
                }
                else
                    Debug.LogError("Slot is empty");

                slotIndex++;
                activeSlotCount = slotIndex;
            }

            SetNavigation();

            for (int i = activeSlotCount; i < _pool.Count; i++)
            {
                _pool[i].Clear();
                _pool[i].gameObject.SetActive(false);
            }
        }

        private IEnumerable<InventorySlot> ApplyFilter(IEnumerable<InventorySlot> slots)
        {
            if (_filter != null)
            {
                slots = slots.Where(s => s.Item.ItemType == _filter.ItemType);

                if (_filter.ItemType == ItemType.Equipment)
                    slots = slots.Where(s => s.Item.EquipmentType == _filter.EquipmentType);
            }

            return slots;
        }

        private void OnClickedHandler(InventorySlot slot) => OnClicked?.Invoke(slot);

        public void SelectFirstItem()
        {
            if (_currentItems.FirstOrDefault() != null)
                EventSystem.current.SetSelectedGameObject(_currentItems.FirstOrDefault().gameObject);
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        private void OnInventoryChangedHandler() => UpdateView();

        private void OnSelectedHandler(InventorySlot slot) => OnSelected?.Invoke(slot);

        private void SetNavigation()
        {
            var rows = _grid.GetColumnsCount();

            for (int i = 0; i < _currentItems.Count; i++)
            {
                var navigation = _currentItems[i].Selectable.navigation;
                navigation.mode = Navigation.Mode.Explicit;

                if (i - rows >= 0)
                    navigation.selectOnUp = _currentItems[i - rows].Selectable;
                if (i - 1 >= 0)
                    navigation.selectOnLeft = _currentItems[i - 1].Selectable;
                if (i + 1 < _currentItems.Count())
                    navigation.selectOnRight = _currentItems[i + 1].Selectable;
                if (i + rows < _currentItems.Count())
                    navigation.selectOnDown = _currentItems[i + rows].Selectable;

                _currentItems[i].Selectable.navigation = navigation;
            }
        }

        private void OnDestroy()
        {
            if (_pool != null)
                foreach (var item in _pool)
                    item.Clear();

            if (_inventory != null)
            {
                _inventory.OnInventoryChanged -= OnInventoryChangedHandler;
            }
        }
    }

    public class InventoryItemsFilter
    {
        public ItemType ItemType = ItemType.None;
        public EquipmentType EquipmentType;
    }
}
