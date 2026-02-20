using Assets.Game.Scripts.BattleSystem.Unit;
using System;
using System.Collections.Generic;

namespace Assets.Game.Scripts.BattleSystem.States.TargetSelection
{
    public interface ITargetSelector : IStateController<PlayerTargetDependencies>
    {
        IEnumerable<BattleUnit> ClickableUnits { get; set; }

        event Action<BattleUnit> OnTargetSelected;
    }
}
