using Assets.Game.Scripts.BattleSystem.Services.Highlighter;
using Assets.Game.Scripts.BattleSystem.States.Execution.Interfaces;
using Assets.Game.Scripts.BattleSystem.Unit;
using Assets.Game.Scripts.Common.UniversalStateMachine;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.Execution.Controllers
{
    public class HighlightingExecutionController : IHighlightingExecutionController
    {
        [Inject] private readonly IUnitHighlighter _highlighter;

        private ExecuteAbilitiesDependencies _dependencies;

        public void Activate()
        {
            _dependencies.AbilitiesExecution.OnUnitExecutingStarted += OnUnitExecutingStartedHandler;
            _dependencies.AbilitiesExecution.OnUnitExecutingEnded += OnUnitExecutingEndedHandler;
        }

        public void Deactivate()
        {
            _dependencies.AbilitiesExecution.OnUnitExecutingStarted -= OnUnitExecutingStartedHandler;
            _dependencies.AbilitiesExecution.OnUnitExecutingEnded -= OnUnitExecutingEndedHandler;

            _highlighter.RemoveAllHighlights();
        }


        public void Init(ExecuteAbilitiesDependencies dependencies, IStateSwitcher stateSwitcher)
        {
            _dependencies = dependencies;
        }

        private void OnUnitExecutingStartedHandler(BattleUnit unit)
        {
            _highlighter.Highlight(unit, HighlighterType.SelectedUnit);
        }

        private void OnUnitExecutingEndedHandler(BattleUnit unit)
        {
            _highlighter.RemoveHighlight(unit);
        }
    }
}
