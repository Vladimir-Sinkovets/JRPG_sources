using Assets.Game.Scripts.BattleSystem.Unit;
using System.Collections.Generic;

namespace Assets.Game.Scripts.BattleSystem.Services.Highlighter
{
    public interface IUnitHighlighter
    {
        void HighlightRange(IEnumerable<BattleUnit> units, HighlighterType type);
        void Highlight(BattleUnit unit, HighlighterType type);
        void RemoveHighlightByType(BattleUnit unit, HighlighterType type);
        void RemoveHighlight(BattleUnit unit);
        void RemoveHighlightsRange(IEnumerable<BattleUnit> units);
        void RemoveHighlightsRangeByType(IEnumerable<BattleUnit> units, HighlighterType type);
        void RemoveAllHighlights();
    }
}
