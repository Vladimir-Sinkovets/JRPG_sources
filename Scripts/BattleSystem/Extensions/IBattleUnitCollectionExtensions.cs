using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Services.SceneDataAccessor;
using Assets.Game.Scripts.BattleSystem.Unit;
using System.Collections.Generic;

namespace Assets.Game.Scripts.BattleSystem.Extensions
{
    public static class BattleUnitCollectionExtensions
    {
        public static IEnumerable<BattleUnit> GetAvailableUnits(
            this IBattleUnitCollection _battleUnitCollection,
            AvailableTargetsType availableTargetsType,
            BattleUnit owner)
        {
            return availableTargetsType switch
            {
                AvailableTargetsType.Enemies => owner.IsEnemy ? _battleUnitCollection.Allies : _battleUnitCollection.Enemies,
                AvailableTargetsType.Allies => owner.IsEnemy ? _battleUnitCollection.Enemies : _battleUnitCollection.Allies,
                AvailableTargetsType.Self => new List<BattleUnit>() { owner },
                AvailableTargetsType.All => _battleUnitCollection.Units,
                _ => _battleUnitCollection.Units,
                //_ => throw new NotImplementedException()
            };
        }
    }
}
