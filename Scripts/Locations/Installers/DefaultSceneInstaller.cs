using Assets.Game.Scripts.Common.SaveData;
using Assets.Game.Scripts.Locations.Player;
using Assets.Game.Scripts.Locations.PlayerData;
using Assets.Game.Scripts.Locations.PlayerData.InventoryData;
using Assets.Game.Scripts.Locations.PlayerData.PartyData;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Locations.Installers
{
    public class DefaultSceneInstaller : MonoInstaller
    {
        [SerializeField] private PlayerStats _playerStats;
        [SerializeField] private GameMetaDataSaver _gameMetaDataSaver;
        [SerializeField] private Inventory _inventory;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private Party _partyData;

        public override void InstallBindings()
        {
            Container.BindInstance(_gameMetaDataSaver);

            Container.BindInstance(_inventory);

            Container.Bind<PlayerStats>()
                .FromInstance(_playerStats);

            Container.BindInstance(_playerController);

            Container.Bind<Party>()
                .FromInstance(_partyData);
        }
    }
}