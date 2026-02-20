using Assets.Game.Scripts.BattleSystem.States.ClearData.Interfaces;
using System.Collections.Generic;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.ClearData
{
    public class ClearDataDependencies
    {
        [Inject] public IClearStateDataController ClearController;

        public IList<IStateController<ClearDataDependencies>> Controllers =>
            new List<IStateController<ClearDataDependencies>>()
            {
                ClearController,
            };
    }
}