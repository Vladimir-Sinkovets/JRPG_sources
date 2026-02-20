using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Extensions;
using Assets.Game.Scripts.BattleSystem.Services.BattlePositions;
using Assets.Game.Scripts.BattleSystem.Services.SceneDataAccessor;
using Assets.Game.Scripts.BattleSystem.Services.TokensCostCheckers;
using Assets.Game.Scripts.BattleSystem.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.Services.AbilityAvailabilityCheckers
{
    public class AbilityAvailabilityChecker : IAbilityAvailabilityChecker
    {
        [Inject] private ITokensCostChecker _tokensCostChecker;
        [Inject] private IBattleUnitCollection _battleUnitCollection;
        [Inject] private BattlePositionsData _battlePositionsData;

        public bool IsAvailable(BattleAbility ability, BattleUnit owner, out IEnumerable<BattleUnit> affordableTargets)
        {
            var canAffordUnitCost = !ability.HasUnitCost || _tokensCostChecker.TryFindTokens(owner, ability.UnitCost, out _);

            var hasValidTargets = HasAffordableTargets(ability, owner, out affordableTargets);

            var canAffordByType = GetCanAffordByType(owner, ability);

            var isLimitReached = IsLimitRiched(ability);

            return canAffordUnitCost && hasValidTargets && canAffordByType && !isLimitReached;
        }

        private bool IsLimitRiched(BattleAbility ability)
        {
            return ability.IsLimitRiched();
        }

        private bool GetCanAffordByType(BattleUnit owner, BattleAbility ability)
        {
            if (ability.Type == AbilityType.Summoning)
            {
                return owner.IsEnemy ?
                    _battlePositionsData.EnemyTeamFreePositions.Any() :
                    _battlePositionsData.PlayerTeamFreePositions.Any();
            }

            return true;
        }

        private bool HasAffordableTargets(BattleAbility ability, BattleUnit owner, out IEnumerable<BattleUnit> affordableTargets)
        {
            var availableTargets = new List<BattleUnit>().AsEnumerable();

            var numberOfTargets = ability.NumberOfTargets;

            switch (ability.TargetsType)
            {
                case TargetsType.Selected:
                    availableTargets = _battleUnitCollection.GetAvailableUnits(ability.AvailableTargetsType, owner)
                        .Where(u => u.Stats.IsDead == false);
                    numberOfTargets = Math.Min(numberOfTargets, availableTargets.Count());
                    break;
                case TargetsType.Allies:
                    availableTargets = (owner.IsEnemy ? _battleUnitCollection.Enemies : _battleUnitCollection.Allies)
                        .Where(u => u.Stats.IsDead == false);
                    numberOfTargets = availableTargets.Count();
                    break;
                case TargetsType.Enemies:
                    availableTargets = (owner.IsEnemy ? _battleUnitCollection.Allies : _battleUnitCollection.Enemies)
                        .Where(u => u.Stats.IsDead == false);
                    numberOfTargets = availableTargets.Count();
                    break;
                case TargetsType.All:
                    availableTargets = _battleUnitCollection.Units
                        .Where(u => u.Stats.IsDead == false);
                    numberOfTargets = availableTargets.Count();
                    break;
                default:
                    Debug.LogError("Not implemented targets type");
                    break;
            }

            var affordableTargetsList = new List<BattleUnit>();

            foreach (var target in availableTargets)
            {
                if (_tokensCostChecker.TryFindTokens(target, ability.TargetCost, out _))
                    affordableTargetsList.Add(target);
            }

            affordableTargets = affordableTargetsList;

            return affordableTargetsList.Count >= numberOfTargets;
        }
    }
}
