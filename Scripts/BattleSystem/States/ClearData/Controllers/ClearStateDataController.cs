using Assets.Game.Scripts.BattleSystem.States.Waiting;
using Assets.Game.Scripts.BattleSystem.Services.SceneDataAccessor;
using Assets.Game.Scripts.BattleSystem.States.ClearData.Interfaces;
using Assets.Game.Scripts.Common.UniversalStateMachine;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.ClearData.Controllers
{
    public class ClearStateDataController : IClearStateDataController
    {
        private IStateSwitcher _stateSwitcher;

        [Inject] private readonly BattleStateMachineData _data;
        [Inject] private readonly BattleUnitContainer _sceneData;

        public void Init(ClearDataDependencies dependencies, IStateSwitcher stateSwitcher)
        {
            _stateSwitcher = stateSwitcher;
        }

        public void Activate()
        {
            _data.selectedUnit = null;
            _data.selectedAbility = null;

            foreach (var units in _sceneData.Units)
            {
                units.Abilities.ClearAbilityForUse();
            }

            _stateSwitcher.SwitchState<WaitingState>();
        }

        public void Deactivate() { }
    }
}
