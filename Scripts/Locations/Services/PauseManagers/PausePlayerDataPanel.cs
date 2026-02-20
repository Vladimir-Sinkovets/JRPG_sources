using Assets.Game.Scripts.Locations.PlayerData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Locations.Services.PauseManagers
{
    public class PausePlayerDataPanel : MonoBehaviour
    {
        private const int MaxTeammatesCount = 4;

        [SerializeField] private PlayerCharacterDataItem _itemPrefab;

        [SerializeField] private RectTransform _container;

        [Inject] private PlayerStats _playerData;

        private List<PlayerCharacterDataItem> _items;

        private void Awake()
        {
            if (_items == null)
            {
                _items = new();
                for (int i = 0; i < MaxTeammatesCount; i++)
                    _items.Add(Instantiate(_itemPrefab, _container));
            }
        }

        public void UpdateInfo()
        {
            foreach (var item in _items)
            {
                item.Hide();
            }
            // todo: show player's team
            var player = _items.First();

            player.Show(_playerData.Stats);
        }
    }
}
