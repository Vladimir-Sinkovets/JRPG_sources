using Assets.Game.Scripts.BattleSystem.States;
using Assets.Game.Scripts.BattleSystem.States.BeforeTurn;
using Assets.Game.Scripts.BattleSystem.States.ClearData;
using Assets.Game.Scripts.BattleSystem.States.ClearData.Controllers;
using Assets.Game.Scripts.BattleSystem.States.ClearData.Interfaces;
using Assets.Game.Scripts.BattleSystem.States.EnemySelection;
using Assets.Game.Scripts.BattleSystem.States.EnemySelection.Controller;
using Assets.Game.Scripts.BattleSystem.States.EnemySelection.Interfaces;
using Assets.Game.Scripts.BattleSystem.States.Execution;
using Assets.Game.Scripts.BattleSystem.States.Execution.Controllers;
using Assets.Game.Scripts.BattleSystem.States.Execution.Interfaces;
using Assets.Game.Scripts.BattleSystem.States.Init;
using Assets.Game.Scripts.BattleSystem.States.Init.Controllers;
using Assets.Game.Scripts.BattleSystem.States.Init.Interfaces;
using Assets.Game.Scripts.BattleSystem.States.Loss;
using Assets.Game.Scripts.BattleSystem.States.Loss.Controllers;
using Assets.Game.Scripts.BattleSystem.States.PlayerSelection;
using Assets.Game.Scripts.BattleSystem.States.PlayerSelection.Controllers;
using Assets.Game.Scripts.BattleSystem.States.PlayerSelection.Interfaces;
using Assets.Game.Scripts.BattleSystem.States.TargetSelection;
using Assets.Game.Scripts.BattleSystem.States.TargetSelection.Controllers;
using Assets.Game.Scripts.BattleSystem.States.TargetSelection.Interfaces;
using Assets.Game.Scripts.BattleSystem.States.Waiting;
using Assets.Game.Scripts.BattleSystem.States.Waiting.Controllers;
using Assets.Game.Scripts.BattleSystem.States.Win;
using Assets.Game.Scripts.BattleSystem.States.Win.Controllers;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.Installers
{
    public class BattleStateMachineInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindLogic();

            BindStates();

            Container.Bind<BattleStateMachineData>()
                .AsSingle();
        }
        private void BindStates()
        {
            Container.Bind<InitDependencies>()
                .AsSingle();
            Container.Bind<PlayerSelectionDependencies>()
                .AsSingle();
            Container.Bind<PlayerTargetDependencies>()
                .AsSingle();
            Container.Bind<EnemySelectionDependencies>()
                .AsSingle();
            Container.Bind<ExecuteAbilitiesDependencies>()
                .AsSingle();
            Container.Bind<ClearDataDependencies>()
                .AsSingle();
            Container.Bind<WinDependencies>()
                .AsSingle();
            Container.Bind<LossDependencies>()
                .AsSingle();
            Container.Bind<WaitingDependencies>()
                .AsSingle();
            Container.Bind<BeforeTurnStateDependencies>()
                .AsSingle();
        }

        private void BindLogic()
        {
            BindPlayerSelection();
            BindTargetSelection();
            BindInit();
            BindWaiting();
            BindExecution();
            BindEnemySelection();
            BindClear();
            BindWin();
            BindDeath();
        }
        private void BindWaiting()
        {
            Container.BindInterfacesAndSelfTo<TicksController>()
                .AsSingle();
        }

        private void BindDeath()
        {
            Container.BindInterfacesAndSelfTo<LoadBackSceneDeathController>()
                .AsSingle();
        }

        private void BindWin()
        {
            Container.BindInterfacesAndSelfTo<LoadBackSceneWinController>()
                .AsSingle();
        }

        private void BindClear()
        {
            Container.Bind<IClearStateDataController>()
                .To<ClearStateDataController>()
                .AsSingle();
        }

        private void BindEnemySelection()
        {
            Container.Bind<IEnemyAbilityController>()
                .To<EnemyAbilityController>()
                .AsSingle();
        }

        private void BindInit()
        {
            Container.Bind<ISwitchStateController>()
                .To<SwitchStateController>()
                .AsSingle();

            Container.Bind<IUnitSpawnerInitController>()
                .To<UnitSpawnerInitController>()
                .AsSingle();
        }

        private void BindTargetSelection()
        {
            Container.Bind<IHighlightingTargetSelectionController>()
                .To<HighlightingTargetSelectionController>()
                .AsSingle();

            Container.Bind<ITargetSelector>()
                .To<TargetSelectorController>()
                .AsSingle();

            Container.Bind<ICancelButtonTargetSelectionController>()
                .To<CancelButtonTargetSelectionController>()
                .AsSingle();

            Container.Bind<IInfoTargetSelectionController>()
                .To<InfoTargetSelectionController>()
                .AsSingle();

            Container.Bind<ITargetPointerController>()
                .To<TargetPointerController>()
                .AsSingle();
        }

        private void BindPlayerSelection()
        {
            Container.Bind<IInfoPlayerSelectionController>()
                .To<InfoPlayerSelectionController>()
                .AsSingle();

            Container.Bind<IUnitHighlightingController>()
                .To<UnitHighlightingController>()
                .AsSingle();

            Container.Bind<IPanelController>()
                .To<PanelController>()
                .AsSingle();
        }

        private void BindExecution()
        {
            Container.Bind<ISwitchStateExecutionController>()
                .To<SwitchStateExecutionController>()
                .AsSingle();

            Container.Bind<IInfoExecutionController>()
                .To<InfoExecutionController>()
                .AsSingle();

            Container.Bind<IHighlightingExecutionController>()
                .To<HighlightingExecutionController>()
                .AsSingle();

            Container.Bind<IUnitEvents>()
                .To<UnitEvents>()
                .AsSingle();

            Container.Bind<IAbilitiesExecution>()
                .To<AbilitiesExecution>()
                .AsSingle();
        }
    }
}
