using Assets.Game.Scripts.Common.Services.Coroutines;
using Assets.Game.Scripts.BattleSystem.Services.SceneDataAccessor;
using Assets.Game.Scripts.BattleSystem.Unit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.Common.UniversalStateMachine;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.Execution
{
    public class AbilitiesExecution : IAbilitiesExecution
    {
        public event Action OnExecutionEnded;
        public event Action<BattleUnit> OnUnitExecutingStarted;
        public event Action<BattleUnit> OnUnitExecutingEnded;

        [Inject] private readonly ICoroutineManager _coroutineManager;
        [Inject] private readonly BattleStateMachineData _battleStateMachineData;
        [Inject] private readonly IBattleUnitCollection _battleUnitCollection;

        private Coroutine _executingCoroutine;

        public IEnumerable<BattleUnit> Units { get => _battleUnitCollection.Units; }

        public AbilitiesExecution(
            IBattleUnitCollection battleUnitCollection,
            ICoroutineManager coroutineManager,
            BattleStateMachineData battleStateMachineData)
        {
            _coroutineManager = coroutineManager;
            _battleStateMachineData = battleStateMachineData;
            _battleUnitCollection = battleUnitCollection;
        }

        public void Init(ExecuteAbilitiesDependencies dependencies, IStateSwitcher stateSwitcher) { }

        public void Activate()
        {
            StartExecution();
        }

        public void Deactivate()
        {
            _coroutineManager.StopCoroutine(_executingCoroutine);
        }

        private void StartExecution()
        {
            _executingCoroutine = _coroutineManager.StartCoroutine(Execute());
        }

        private IEnumerator Execute()
        {
            yield return null; // этот маленький и хитрый костыль здесь для того,
                               // чтобы все контроллеры смогли подписаться на нужные события
                               // до исполнения этого кода

            var unit = _battleStateMachineData.selectedUnit;

            if (unit.Stats.IsDead)
            {
                Debug.LogError($"Selected unit \"{unit}\" is dead");
            }

            var ability = unit.Abilities.AbilityForUse;

            if (ability != null)
            {
                OnUnitExecutingStarted?.Invoke(unit);

                yield return ExecuteAbility(ability);
            }

            OnUnitExecutingEnded?.Invoke(unit);

            EndState();

            yield break;
        }

        private IEnumerator ExecuteAbility(AbilityData abilityData)
        {
            Debug.Log($"Execute ability: Owner = {abilityData.Owner}, Targets.Count = {abilityData.Targets.Count}");

            yield return abilityData.Ability.Execute(abilityData);
        }

        private void EndState()
        {
            Debug.Log("State \"AbilitiesExecution\" finished");

            OnExecutionEnded?.Invoke();
        }
    }
}
