using Assets.Game.Scripts.BattleSystem.Services.UIStateInfo;
using Assets.Game.Scripts.BattleSystem.States.TargetSelection.Interfaces;
using Assets.Game.Scripts.Common.UniversalStateMachine;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.TargetSelection.Controllers
{
    public class InfoTargetSelectionController : IInfoTargetSelectionController
    {
        [Inject] private readonly IStateInfoService _stateInfo;

        public void Activate()
        {
            _stateInfo.ShowInfo("Choose target");
        }

        public void Deactivate()
        {
            _stateInfo.ShowInfo(string.Empty);
        }

        public void Init(PlayerTargetDependencies dependencies, IStateSwitcher stateSwitcher)
        {

        }
    }
}
