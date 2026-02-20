using Assets.Game.Scripts.BattleSystem.States.PlayerSelection;
using Assets.Game.Scripts.BattleSystem.States.TargetSelection.Interfaces;
using Assets.Game.Scripts.Common.Input;
using Assets.Game.Scripts.Common.UniversalStateMachine;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.TargetSelection.Controllers
{
    public class CancelButtonTargetSelectionController : ICancelButtonTargetSelectionController
    {
        private IStateSwitcher _stateSwitcher;

        [Inject] private IUIInputController _UIInputController;

        public void Activate()
        {
            _UIInputController.OnCancel += OnSelectionCanceledHandler;
        }

        public void Deactivate()
        {
            _UIInputController.OnCancel -= OnSelectionCanceledHandler;
        }

        public void Init(PlayerTargetDependencies dependencies, IStateSwitcher stateSwitcher)
        {
            _stateSwitcher = stateSwitcher;
        }

        private void OnSelectionCanceledHandler()
        {
            _stateSwitcher.SwitchState<PlayerSelectionState>();
        }
    }
}
