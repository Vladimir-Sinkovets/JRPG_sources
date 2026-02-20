using Assets.Game.Scripts.BattleSystem.States.EnemySelection;
using Assets.Game.Scripts.BattleSystem.States.PlayerSelection;
using Assets.Game.Scripts.Common.UniversalStateMachine;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.BattleSystem.States.BeforeTurn
{
    public class BeforeTurnState : State
    {
        private readonly BeforeTurnStateDependencies _dependencies;
        private Coroutine _coroutine;

        public BeforeTurnState(IStateSwitcher stateSwitcher, BeforeTurnStateDependencies dependencies) : base(stateSwitcher)
        {
            _dependencies = dependencies;
        }

        public override void Enter()
        {
            _coroutine = _dependencies.CoroutineManager.StartCoroutine(M());
        }

        private IEnumerator M()
        {
            _dependencies.StateMachineData.selectedUnit.Effects.Tick();

            yield return new WaitForSeconds(0.5f);

            if (_dependencies.StateMachineData.selectedUnit.IsEnemy)
                StateSwitcher.SwitchState<EnemySelectionState>();
            else
                StateSwitcher.SwitchState<PlayerSelectionState>();
        }

        public override void Exit()
        {
            _dependencies.CoroutineManager.StopCoroutine(_coroutine);
        }

        public override void Update() { }
    }
}
