using Assets.Game.Scripts.BattleSystem.States.BeforeTurn;
using Assets.Game.Scripts.BattleSystem.States.Waiting;
using Assets.Game.Scripts.BattleSystem.States;
using Assets.Game.Scripts.BattleSystem.States.ClearData;
using Assets.Game.Scripts.BattleSystem.States.EnemySelection;
using Assets.Game.Scripts.BattleSystem.States.Execution;
using Assets.Game.Scripts.BattleSystem.States.Init;
using Assets.Game.Scripts.BattleSystem.States.Loss;
using Assets.Game.Scripts.BattleSystem.States.PlayerSelection;
using Assets.Game.Scripts.BattleSystem.States.Win;
using UnityEngine;
using Zenject;
using Assets.Game.Scripts.Common.UniversalStateMachine;

public class Battle : MonoBehaviour
{
    [SerializeField] private bool _isDebug = false;

    private StateMachine _stateMachine;

    [Inject] private PlayerSelectionDependencies _playerDependencies;
    [Inject] private ExecuteAbilitiesDependencies _executionDependencies;
    [Inject] private InitDependencies _initDependencies;
    [Inject] private PlayerTargetDependencies _playerTargetDependencies;
    [Inject] private EnemySelectionDependencies _enemySelectionDependencies;
    [Inject] private ClearDataDependencies _clearDataDependencies;
    [Inject] private LossDependencies _playerDeathDependencies;
    [Inject] private WinDependencies _winDependencies;
    [Inject] private WaitingDependencies _waitingDependencies;
    [Inject] private BeforeTurnStateDependencies _beforeTurnStateDependencies;

    private void Start()
    {
        InitStateMachine();
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    private void InitStateMachine()
    {
        _stateMachine = new StateMachine();
        _stateMachine.IsDebug = _isDebug;

        _stateMachine.AddState(new InitState(_stateMachine, _initDependencies));
        _stateMachine.AddState(new WaitingState(_stateMachine, _waitingDependencies));
        _stateMachine.AddState(new PlayerSelectionState(_stateMachine, _playerDependencies));
        _stateMachine.AddState(new PlayerTargetSelectionState(_stateMachine, _playerTargetDependencies));
        _stateMachine.AddState(new EnemySelectionState(_stateMachine, _enemySelectionDependencies));
        _stateMachine.AddState(new ExecuteAbilitiesState(_stateMachine, _executionDependencies));
        _stateMachine.AddState(new ClearDataState(_stateMachine, _clearDataDependencies));
        _stateMachine.AddState(new LossState(_stateMachine, _playerDeathDependencies));
        _stateMachine.AddState(new WinState(_stateMachine, _winDependencies));
        _stateMachine.AddState(new BeforeTurnState(_stateMachine, _beforeTurnStateDependencies));

        _stateMachine.SetStartState<InitState>();
    }

    private void OnDestroy()
    {
        _stateMachine.Dispose();
    }
}
