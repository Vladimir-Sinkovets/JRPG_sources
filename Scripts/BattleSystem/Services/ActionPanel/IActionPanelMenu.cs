using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Unit;
using System;

namespace Assets.Game.Scripts.BattleSystem.Services.ActionPanel
{
    public interface IActionPanelMenu
    {
        event Action<BattleAbility> OnUseButtonClick;

        void Disable();
        void Enable(BattleUnit unit);
    }
}