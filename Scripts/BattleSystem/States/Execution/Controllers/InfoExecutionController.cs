using Assets.Game.Scripts.BattleSystem.Services.UIStateInfo;
using Assets.Game.Scripts.BattleSystem.States.Execution.Interfaces;
using Assets.Game.Scripts.Common.UniversalStateMachine;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.Execution.Controllers
{
    public class InfoExecutionController : IInfoExecutionController
    {
        [Inject] private readonly IStateInfoService _stateInfo;

        public void Activate()
        {
            _stateInfo.ShowInfo("Execution");
        }

        public void Deactivate()
        {
            _stateInfo.ShowInfo(string.Empty);
        }

        public void Init(ExecuteAbilitiesDependencies dependencies, IStateSwitcher stateSwitcher) { }
    }
}
