using Assets.Game.Scripts.Common.Services.Coroutines;
using Assets.Game.Scripts.BattleSystem.States;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.BeforeTurn
{
    public class BeforeTurnStateDependencies
    {
        [Inject] public ICoroutineManager CoroutineManager;
        [Inject] public BattleStateMachineData StateMachineData;
    }
}