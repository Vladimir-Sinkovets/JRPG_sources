using Assets.Game.Scripts.BattleSystem.Services.Highlighter;
using Assets.Game.Scripts.BattleSystem.States.TargetSelection.Interfaces;
using Assets.Game.Scripts.BattleSystem.Unit;
using Assets.Game.Scripts.Common.UniversalStateMachine;
using System.Collections.Generic;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.TargetSelection.Controllers
{
    public class HighlightingTargetSelectionController : IHighlightingTargetSelectionController
    {
        [Inject] private readonly IUnitHighlighter _unitHighlighter;
        private PlayerTargetDependencies _dependencies;

        public void Init(PlayerTargetDependencies dependencies, IStateSwitcher stateSwitcher)
        {
            _dependencies = dependencies;
        }

        public void Activate()
        {
            HighlightClickableUnits(_dependencies.TargetSelector.ClickableUnits);

            _dependencies.TargetSelector.OnTargetSelected += OnTargetSelectedHandler;
        }

        public void Deactivate()
        {
            _unitHighlighter.RemoveAllHighlights();

            _dependencies.TargetSelector.OnTargetSelected -= OnTargetSelectedHandler;
        }

        private void HighlightClickableUnits(IEnumerable<BattleUnit> units)
        {
            _unitHighlighter.HighlightRange(units, HighlighterType.CanBeTarget);
        }

        private void OnTargetSelectedHandler(BattleUnit unit)
        {
            _unitHighlighter.RemoveHighlight(unit);
            _unitHighlighter.Highlight(unit, HighlighterType.Target);
        }
    }
}
