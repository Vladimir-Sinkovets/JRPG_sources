using Assets.Game.Scripts.BattleSystem.Unit;

namespace Assets.Game.Scripts.BattleSystem.Services.SceneDataAccessor
{
    public interface IBattleUnitCollectionModifier
    {
        void AddUnit(BattleUnit unit);
        void AddPlayer(BattleUnit player);
    }
}