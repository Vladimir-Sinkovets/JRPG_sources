using Assets.Game.Scripts.BattleSystem.Services.CameraFocusServices;
using Assets.Game.Scripts.BattleSystem.States.ClearData;
using Assets.Game.Scripts.BattleSystem.States.Execution.Interfaces;
using Assets.Game.Scripts.BattleSystem.States.Loss;
using Assets.Game.Scripts.BattleSystem.States.Win;
using Assets.Game.Scripts.Common.UniversalStateMachine;
using System;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.Execution.Controllers
{
    public class SwitchStateExecutionController : ISwitchStateExecutionController
    {
        private ExecuteAbilitiesDependencies _dependencies;
        private IStateSwitcher _stateSwitcher;

        [Inject] private ICameraFocusService _cameraFocusService;

        public void Init(ExecuteAbilitiesDependencies dependencies, IStateSwitcher stateSwitcher)
        {
            _dependencies = dependencies;
            _stateSwitcher = stateSwitcher;
        }

        public void Activate()
        {
            _dependencies.UnitEvents.OnPlayerDied += OnPlayerDiedHandler;
            _dependencies.UnitEvents.OnAllEnemiesDied += OnAllEnemiesDiedHandler;
            _dependencies.AbilitiesExecution.OnExecutionEnded += OnExecutionEndedHandler;
        }

        public void Deactivate()
        {
            _dependencies.UnitEvents.OnPlayerDied -= OnPlayerDiedHandler;
            _dependencies.UnitEvents.OnAllEnemiesDied -= OnAllEnemiesDiedHandler;
            _dependencies.AbilitiesExecution.OnExecutionEnded -= OnExecutionEndedHandler;
        }

        private void OnAllEnemiesDiedHandler()
        {
            ClearFocus();
            _stateSwitcher.SwitchState<WinState>();
        }

        private void OnPlayerDiedHandler()
        {
            ClearFocus();
            _stateSwitcher.SwitchState<LossState>();
        }

        private void OnExecutionEndedHandler()
        {
            ClearFocus();
            _stateSwitcher.SwitchState<ClearDataState>();
        }

        private void ClearFocus()
        {
            _cameraFocusService.ResetTarget();
        }
    }
}
