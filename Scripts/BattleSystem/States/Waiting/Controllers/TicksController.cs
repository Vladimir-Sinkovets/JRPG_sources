using Assets.Game.Scripts.BattleSystem.States.BeforeTurn;
using Assets.Game.Scripts.BattleSystem.States.Waiting.Interfaces;
using Assets.Game.Scripts.Common.Services.Coroutines;
using Assets.Game.Scripts.BattleSystem.Services.SceneDataAccessor;
using Assets.Game.Scripts.BattleSystem.States;
using Assets.Game.Scripts.BattleSystem.States.EnemySelection;
using Assets.Game.Scripts.BattleSystem.States.Loss;
using Assets.Game.Scripts.BattleSystem.States.PlayerSelection;
using Assets.Game.Scripts.BattleSystem.States.Win;
using Assets.Game.Scripts.BattleSystem.Unit;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Assets.Game.Scripts.Common.UniversalStateMachine;

namespace Assets.Game.Scripts.BattleSystem.States.Waiting.Controllers
{
    public class TicksController : ITicksController
    {
        [Inject] private IBattleUnitCollection _battleUnitCollection;
        [Inject] private ICoroutineManager _coroutineManager;
        [Inject] private BattleStateMachineData _battleStateMachineData;

        private IStateSwitcher _stateSwitcher;
        private Coroutine _coroutine;

        public IEnumerable<BattleUnit> AliveUnits { get => _battleUnitCollection.Units.Where(x => !x.Stats.IsDead); }

        public void Init(WaitingDependencies dependencies, IStateSwitcher stateSwitcher)
        {
            _stateSwitcher = stateSwitcher;
        }

        public void Activate()
        {
            _coroutine = _coroutineManager.StartCoroutine(WaitForTurn());
        }

        private IEnumerator WaitForTurn()
        {
            while (true)
            {
                foreach (var unit in AliveUnits)
                {
                    if (unit.Stats.TurnProgress >= BattleSystemConstants.TurnWaitingProgressMax)
                    {
                        _battleStateMachineData.selectedUnit = unit;

                        _battleStateMachineData.selectedUnit.Stats.ClearTurnProgress();

                        _stateSwitcher.SwitchState<BeforeTurnState>();

                        yield break;
                    }
                }
                 
                foreach (var unit in AliveUnits)
                {
                    unit.UpdateTurnWaitingProgress(BattleSystemConstants.TurnWaitingProgressTickTime);
                }

                if (_battleUnitCollection.Enemies.All(u => u.Stats.IsDead))
                    _stateSwitcher.SwitchState<WinState>();

                if (_battleUnitCollection.Allies.All(u => u.Stats.IsDead))
                    _stateSwitcher.SwitchState<LossState>();

                yield return new WaitForSeconds(BattleSystemConstants.TurnWaitingProgressTickTime);
            }
        }

        public void Deactivate()
        {
            _coroutineManager.StopCoroutine(_coroutine);

            _coroutine = null;
        }
    }
}
