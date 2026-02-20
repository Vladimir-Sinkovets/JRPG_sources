using Assets.Game.Scripts.BattleSystem.Unit;

namespace Assets.Game.Scripts.BattleSystem.Services.UnitPointers
{
    public interface IUnitPointer
    {
        void Clear();
        void RemovePointer(BattleUnit unit);
        void SetPointer(BattleUnit unit);
    }
}