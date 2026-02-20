using Assets.Game.Scripts.BattleSystem.States.Waiting;
using Assets.Game.Scripts.BattleSystem.States.Init.Interfaces;
using Assets.Game.Scripts.Common.UniversalStateMachine;

namespace Assets.Game.Scripts.BattleSystem.States.Init.Controllers
{
    public class SwitchStateController : ISwitchStateController
    {
        private IStateSwitcher _stateSwitcher;

        public void Activate()
        {
            _stateSwitcher.SwitchState<WaitingState>();
        }

        public void Deactivate() { }

        public void Init(InitDependencies dependencies, IStateSwitcher stateSwitcher)
        {
            _stateSwitcher = stateSwitcher;
        }
    }
}
