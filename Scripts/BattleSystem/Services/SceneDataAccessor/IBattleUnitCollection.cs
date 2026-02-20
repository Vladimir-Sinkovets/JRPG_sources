using Assets.Game.Scripts.BattleSystem.Unit;
using System.Collections.Generic;

namespace Assets.Game.Scripts.BattleSystem.Services.SceneDataAccessor
{
    public interface IBattleUnitCollection
    {
        IEnumerable<BattleUnit> Units { get; }
        IEnumerable<BattleUnit> Enemies { get; }
        IEnumerable<BattleUnit> Allies { get; }
        BattleUnit Player { get; }
        void RemoveUnit(BattleUnit unit);
    }
}