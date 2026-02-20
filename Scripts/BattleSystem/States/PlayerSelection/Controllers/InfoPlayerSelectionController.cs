using Assets.Game.Scripts.BattleSystem.Services.UIStateInfo;
using Assets.Game.Scripts.BattleSystem.States.PlayerSelection.Interfaces;
using Assets.Game.Scripts.Common.UniversalStateMachine;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.PlayerSelection.Controllers
{
    public class InfoPlayerSelectionController : IInfoPlayerSelectionController
    {
        [Inject] private readonly IStateInfoService _stateInfo;

        public InfoPlayerSelectionController(IStateInfoService stateInfo)
        {
            _stateInfo = stateInfo;
        }

        public void Activate()
        {
            _stateInfo.ShowInfo("Choose your skills");
        }

        public void Deactivate()
        {
            _stateInfo.ShowInfo(string.Empty);
        }

        public void Init(PlayerSelectionDependencies dependencies, IStateSwitcher stateSwitcher)
        {
        }
    }
}
