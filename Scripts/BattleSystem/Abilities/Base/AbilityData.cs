using Assets.Game.Scripts.BattleSystem.Services.BattlePositions;
using Assets.Game.Scripts.BattleSystem.Unit;
using System.Collections.Generic;

namespace Assets.Game.Scripts.BattleSystem.Abilities.Base
{
    public class AbilityData
    {
        public BattleAbility Ability { get; }
        public BattleUnit Owner { get; }
        public List<BattleUnit> Targets { get; }
        public BattlePositionsData BattlePositionsData { get; }

        public AbilityData(BattleAbility ability, BattleUnit owner, List<BattleUnit> targets, BattlePositionsData battlePositionsData)
        {
            Ability = ability;
            Owner = owner;
            Targets = targets;
            BattlePositionsData = battlePositionsData;
        }
    }
}
