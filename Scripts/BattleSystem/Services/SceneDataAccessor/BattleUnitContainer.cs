using Assets.Game.Scripts.BattleSystem.Unit;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Game.Scripts.BattleSystem.Services.SceneDataAccessor
{
    public class BattleUnitContainer : IBattleUnitCollectionModifier, IBattleUnitCollection
    {
        private readonly IList<BattleUnit> _units;

        public BattleUnitContainer()
        {
            _units = new List<BattleUnit>();
        }

        public IEnumerable<BattleUnit> Units { get => _units.ToList(); }
        public IEnumerable<BattleUnit> Enemies { get => _units.Where(u => u.IsEnemy); }
        public IEnumerable<BattleUnit> Allies { get => _units.Where(u => u.IsEnemy == false); }

        public BattleUnit Player { get; set; }

        public void AddPlayer(BattleUnit player) => Player = player;

        public void AddUnit(BattleUnit unit) => _units.Add(unit);
        public void RemoveUnit(BattleUnit unit) => _units.Remove(unit);
    }
}
