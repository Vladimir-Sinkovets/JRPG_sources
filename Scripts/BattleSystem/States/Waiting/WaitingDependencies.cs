using Assets.Game.Scripts.BattleSystem.States.Waiting.Interfaces;
using Assets.Game.Scripts.BattleSystem.States;
using System.Collections.Generic;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.Waiting
{
    public class WaitingDependencies
    {
        [Inject] public ITicksController TicksController;

        public IList<IStateController<WaitingDependencies>> Controllers =>
            new List<IStateController<WaitingDependencies>>()
            {
                TicksController,
            };
    }
}