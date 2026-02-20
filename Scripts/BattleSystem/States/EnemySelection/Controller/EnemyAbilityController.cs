using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Extensions;
using Assets.Game.Scripts.BattleSystem.Services.AbilityAvailabilityCheckers;
using Assets.Game.Scripts.BattleSystem.Services.SceneDataAccessor;
using Assets.Game.Scripts.BattleSystem.Services.TokensCostCheckers;
using Assets.Game.Scripts.BattleSystem.States.EnemySelection.Interfaces;
using Assets.Game.Scripts.BattleSystem.Unit;
using Assets.Game.Scripts.Common.Extensions;
using Assets.Game.Scripts.Common.UniversalStateMachine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.EnemySelection.Controller
{
    public class EnemyAbilityController : IEnemyAbilityController
    {
        [Inject] private readonly IBattleUnitCollection _battleUnitCollection;
        [Inject] private readonly BattleStateMachineData _battleStateMachineData;
        [Inject] private readonly IAbilityAvailabilityChecker _abilityAvailabilityChecker;
        [Inject] private readonly ITokensCostChecker _tokensCostChecker;

        private IStateSwitcher _stateSwitcher;

        public void Activate()
        {
            var enemy = _battleStateMachineData.selectedUnit;

            if (enemy.Behaviour != null)
            {
                enemy.Behaviour.SetAbility(enemy);
            }
            else
            {
                SetRandomAbility(enemy);
            }

            _stateSwitcher.SwitchState<ExecuteAbilitiesState>();
        }

        public void Deactivate() { }

        public void Init(EnemySelectionDependencies dependencies, IStateSwitcher stateSwitcher)
        {
            _stateSwitcher = stateSwitcher;
        }

        private void SetRandomAbility(BattleUnit enemy)
        {
            var abilityData = SelectAvailableAbility(enemy);

            if (abilityData == default)
                return;

            var randomTargets = abilityData.Ability.TargetsType switch
            {
                TargetsType.Selected => abilityData.Targets.GetRandomElements(Mathf.Min(abilityData.Ability.NumberOfTargets, abilityData.Targets.Count())),
                TargetsType.All => abilityData.Targets,
                TargetsType.Enemies => abilityData.Targets,
                TargetsType.Allies => abilityData.Targets,
                _ => throw new System.NotImplementedException(),
            };
            

            enemy.Abilities.SetAbilityForUse(
                abilityData.Ability,
                randomTargets.ToList());
        }

        private (BattleAbility Ability, IEnumerable<BattleUnit> Targets) SelectAvailableAbility(BattleUnit enemy)
        {
            var availableAbilities = new List<(BattleAbility, IEnumerable<BattleUnit>)>();

            foreach (var ability in enemy.Abilities.Collection)
            {
                if (_abilityAvailabilityChecker.IsAvailable(ability, enemy, out var affordableTargets))
                    availableAbilities.Add((ability, affordableTargets));
            }

            if (!availableAbilities.Any())
                return default;

            return availableAbilities.ElementAt(Random.Range(0, availableAbilities.Count()));
        }
    }
}
