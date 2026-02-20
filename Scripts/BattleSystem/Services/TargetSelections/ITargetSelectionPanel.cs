using Assets.Game.Scripts.BattleSystem.Unit;
using System;
using System.Collections.Generic;

namespace Assets.Game.Scripts.BattleSystem.Services.TargetSelections
{
    public interface ITargetSelectionPanel
    {
        event Action<BattleUnit> OnUnitSelected;
        event Action<BattleUnit> OnUnitPointed;
        event Action<IEnumerable<BattleUnit>> OnTargetsSelected;
        event Action<BattleUnit> OnUnitUnpointed;

        void EnableForGroup(IEnumerable<BattleUnit> battleUnits, string groupName = "");
        void Enable(IEnumerable<BattleUnit> battleUnit, int targetsCount);
        void Disable();
    }
}