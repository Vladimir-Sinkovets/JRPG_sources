using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Unit;
using System.Collections.Generic;

namespace Assets.Game.Scripts.BattleSystem.Services.AbilityAvailabilityCheckers
{
    public interface IAbilityAvailabilityChecker
    {
        bool IsAvailable(BattleAbility ability, BattleUnit owner, out IEnumerable<BattleUnit> affordableTargets);
    }
}