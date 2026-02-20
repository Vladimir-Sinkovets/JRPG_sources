using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Unit;
using Assets.Game.Scripts.BattleSystem.Unit.UI;
using System.Collections.Generic;

namespace Assets.Game.Scripts.BattleSystem.Services.TokensCostCheckers
{
    public interface ITokensCostChecker
    {
        bool TryFindTokens(BattleUnit target, IEnumerable<AbilityCost> cost, out IEnumerable<TokenIcon> existingTokens);
    }
}
