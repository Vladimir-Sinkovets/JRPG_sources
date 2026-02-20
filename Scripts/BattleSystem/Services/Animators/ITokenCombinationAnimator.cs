using Assets.Game.Scripts.BattleSystem.Configs;
using Assets.Game.Scripts.BattleSystem.Unit;
using Assets.Game.Scripts.BattleSystem.Unit.UI;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Game.Scripts.BattleSystem.Services.Animators
{
    public interface ITokenCombinationAnimator
    {
        IEnumerator AnimateCombination(IEnumerable<TokenIcon> comboIcons, BattleUnit targetUnit);
    }
}
