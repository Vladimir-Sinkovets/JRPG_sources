using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Unit;
using Assets.Game.Scripts.BattleSystem.Unit.UI;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Game.Scripts.BattleSystem.Services.TokensCostCheckers
{
    public class TokensCostChecker : ITokensCostChecker
    {
        public bool TryFindTokens(BattleUnit target, IEnumerable<AbilityCost> cost, out IEnumerable<TokenIcon> existingTokens)
        {
            existingTokens = Enumerable.Empty<TokenIcon>();

            if (cost == null || !cost.Any())
                return true;

            var costByToken = cost
                .GroupBy(c => c.Token)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Amount)); // todo: may cause problems because of a lot of memory allocation

            var availableTokens = new List<TokenIcon>();
            var unitTokens = target.Tokens.Collection;

            foreach (var costItem in costByToken)
            {
                var tokenType = costItem.Key;
                var requiredAmount = costItem.Value;

                var matchingTokens = unitTokens
                    .Where(t => t.Token == tokenType)
                    .Take(requiredAmount)
                    .ToList();

                if (matchingTokens.Count < requiredAmount)
                    return false;

                availableTokens.AddRange(matchingTokens);
            }

            existingTokens = availableTokens;
            return true;
        }

    }
}
