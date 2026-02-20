using Assets.Game.Scripts.BattleSystem.Unit;
using System;

namespace Assets.Game.Scripts.BattleSystem.States.Execution
{
    public interface IAbilitiesExecution : IStateController<ExecuteAbilitiesDependencies>
    {
        event Action OnExecutionEnded;
        event Action<BattleUnit> OnUnitExecutingStarted;
        event Action<BattleUnit> OnUnitExecutingEnded;
    }
}