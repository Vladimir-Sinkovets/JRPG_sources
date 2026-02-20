using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Scripts.BattleSystem.Abilities
{
    public class SpecialAbilityTestClass : BattleAbility
    {
        public override IEnumerator Execute(AbilityData data)
        {
            if (data.Ability.HasUnitCost)
            {
                if (_tokensCostChecker.TryFindTokens(data.Owner, data.Ability.UnitCost, out var tokens) == false)
                {
                    Debug.LogError($"Unit {{{data.Owner}}} does not have enough tokens for {{{data.Ability}}}");

                    yield break;
                }

                if (tokens.Any())
                {
                    yield return _tokenCombinationAnimator.AnimateCombination(tokens, data.Owner);

                    data.Owner.Tokens.RemoveTokens(tokens);
                }
            }

            foreach (var target in data.Targets)
            {
                if (_tokensCostChecker.TryFindTokens(target, data.Ability.TargetCost, out var tokens) == false)
                {
                    Debug.LogError($"Target {{{target}}} does not have enough tokens for {{{data.Ability}}}");

                    yield break;
                }

                yield return _tokenCombinationAnimator.AnimateCombination(tokens, target);

                target.Tokens.RemoveTokens(tokens);

                target.Stats.TakeDamage(225, DamageType.Physical);
            }

            yield break;
        }
    }
}
