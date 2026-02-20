using Assets.Game.Scripts.BattleSystem.Services.Highlighter;
using Assets.Game.Scripts.BattleSystem.States.PlayerSelection.Interfaces;
using Assets.Game.Scripts.BattleSystem.Unit;
using Assets.Game.Scripts.Common.UniversalStateMachine;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.PlayerSelection.Controllers
{
    public class UnitHighlightingController : IUnitHighlightingController
    {
        [Inject] private readonly IUnitHighlighter _highlighter;
        [Inject] private readonly BattleStateMachineData _data;

        private PlayerSelectionDependencies _dependencies;
        private BattleUnit _lastSelectedUnit;

        public void Init(PlayerSelectionDependencies dependencies, IStateSwitcher stateSwitcher)
        {
            _dependencies = dependencies;
        }

        public void Activate()
        {
            _highlighter.Highlight(_data.selectedUnit, HighlighterType.SelectedUnit);
        }

        public void Deactivate()
        {
            _highlighter.RemoveHighlight(_data.selectedUnit);
        }
    }
}
