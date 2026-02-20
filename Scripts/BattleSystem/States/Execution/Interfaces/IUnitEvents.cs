using System;

namespace Assets.Game.Scripts.BattleSystem.States.Execution
{
    public interface IUnitEvents : IStateController<ExecuteAbilitiesDependencies>
    {
        event Action OnAllEnemiesDied;
        event Action OnUnitDied;
        event Action OnPlayerDied;
    }
}
