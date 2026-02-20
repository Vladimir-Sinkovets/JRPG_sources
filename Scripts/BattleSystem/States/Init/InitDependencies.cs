using Assets.Game.Scripts.BattleSystem.States.Init.Interfaces;
using System.Collections.Generic;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.Init
{
    public class InitDependencies
    {
        [Inject] public IUnitSpawnerInitController UnitSpawner;
        [Inject] public ISwitchStateController SwitchStateController;

        public IList<IStateController<InitDependencies>> Controllers =>
            new List<IStateController<InitDependencies>>()
            {
                UnitSpawner,
                SwitchStateController,
            };
    }
}