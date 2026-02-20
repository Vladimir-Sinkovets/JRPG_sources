using Assets.Game.Scripts.BattleSystem.Configs;
using Assets.Game.Scripts.Common.Configs.PlayerAssetsAccesser;
using Assets.Game.Scripts.Locations.Configs.Inventory;
using Assets.Game.Scripts.Locations.Configs.Party;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Common.Installers
{
    public class GlobalConfigsInstaller : MonoInstaller
    {
        [SerializeField] private BattleAssets _battleAssets;
        [SerializeField] private InventoryItemDataBase _inventoryItemDataBase;
        [SerializeField] private TokensConfig _tokensConfig;
        [SerializeField] private AlliesDataBase _alliesDataBase;

        public override void InstallBindings()
        {
            Container.BindInstance(_battleAssets);

            Container.Bind<InventoryItemDataBase>()
                .FromInstance(_inventoryItemDataBase);

            Container.Bind<TokensConfig>()
                .FromInstance(_tokensConfig);

            Container.Bind<AlliesDataBase>()
                .FromInstance(_alliesDataBase);
        }
    }
}