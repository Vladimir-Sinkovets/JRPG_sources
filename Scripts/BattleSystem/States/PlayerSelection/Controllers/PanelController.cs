using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Services.ActionPanel;
using Assets.Game.Scripts.BattleSystem.States.PlayerSelection.Interfaces;
using Assets.Game.Scripts.Common.UniversalStateMachine;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.PlayerSelection.Controllers
{
    public class PanelController : IPanelController
    {
        [Inject] private IActionPanelMenu _panelMenu;
        [Inject] private BattleStateMachineData _data;

        private IStateSwitcher _stateSwitcher;
        private PlayerSelectionDependencies _dependencies;

        public void Init(PlayerSelectionDependencies dependencies, IStateSwitcher stateSwitcher)
        {
            _dependencies = dependencies;
            _stateSwitcher = stateSwitcher;
        }

        public void Activate()
        {
            _panelMenu.Enable(_data.selectedUnit);

            _panelMenu.OnUseButtonClick += OnUseButtonClickHandler;
        }

        public void Deactivate()
        {
            _panelMenu.Disable();

            _panelMenu.OnUseButtonClick -= OnUseButtonClickHandler;
        }

        private void OnUseButtonClickHandler(BattleAbility battleAbility)
        {
            _data.selectedAbility = battleAbility;

            _stateSwitcher.SwitchState<PlayerTargetSelectionState>();
        }
    }
}