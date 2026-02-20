using Assets.Game.Scripts.BattleSystem.States.Execution.Interfaces;
using System.Collections.Generic;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.Execution
{
    public class ExecuteAbilitiesDependencies
    {
        [Inject] public IAbilitiesExecution AbilitiesExecution;
        [Inject] public IUnitEvents UnitEvents;
        [Inject] public ISwitchStateExecutionController SwitchController;
        [Inject] public IInfoExecutionController InfoController;
        [Inject] public IHighlightingExecutionController HighlightingController;

        public IList<IStateController<ExecuteAbilitiesDependencies>> Controllers =>
            new List<IStateController<ExecuteAbilitiesDependencies>>()
            {
                AbilitiesExecution,
                UnitEvents,
                SwitchController,
                InfoController,
                HighlightingController,
            };
    }
}
