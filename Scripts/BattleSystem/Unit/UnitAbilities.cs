using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Services.BattlePositions;
using Assets.Game.Scripts.BattleSystem.Unit;
using System;
using System.Collections.Generic;

namespace Assets.Game.Scripts.BattleSystem.Unit
{
    public class UnitAbilities
    {
        public event Action<AbilityData> OnAbilityForUseChanged;

        private List<BattleAbility> _abilities;
        private AbilityData _abilityForUse;

        private BattlePositionsData _battlePositionsData;

        private BattleUnit _unit;

        public BattleUnit Unit { get => _unit; }
        public IEnumerable<BattleAbility> Collection { get => _abilities; }
        public AbilityData AbilityForUse { get => _abilityForUse; }

        public void Init(BattlePositionsData battlePositionsData, IEnumerable<BattleAbility> abilities, BattleUnit unit)
        {
            _unit = unit;
            _battlePositionsData = battlePositionsData;

            _abilities = new();
            _abilities.AddRange(abilities);
        }

        public void SetAbilityForUse(BattleAbility ability, List<BattleUnit> targets)
        {
            var abilityData = new AbilityData(
                ability,
                owner: _unit,
                targets,
                _battlePositionsData);

            _abilityForUse = abilityData;

            OnAbilityForUseChanged?.Invoke(_abilityForUse);
        }

        public void ClearAbilityForUse()
        {
            _abilityForUse = null;

            OnAbilityForUseChanged?.Invoke(_abilityForUse);
        }
    }
}
