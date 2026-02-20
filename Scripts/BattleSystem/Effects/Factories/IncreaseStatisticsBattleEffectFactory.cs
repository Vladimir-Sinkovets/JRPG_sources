using Assets.Game.Scripts.BattleSystem.Effects.EffectTypes;
using Assets.Game.Scripts.BattleSystem.Unit;
using System;
using TriInspector;
using UnityEngine;

namespace Assets.Game.Scripts.BattleSystem.Effects.Factories
{
    [CreateAssetMenu(fileName = "Statistics_bonus_effect", menuName = "Battle/Effects/Statistics_bonus")]
    public class IncreaseStatisticsBattleEffectFactory : BattleEffectFactory
    {
        private Guid _defaultId = new("441f080d-1c98-487f-8f57-16c46a7f26c7");

        [SerializeField] private int _hpBonus;
        [SerializeField] private int _attackBonus;
        [SerializeField] private int _magicalAttackBonus;
        [SerializeField, Unit("%")] private int _defenceBouns;
        [SerializeField, Unit("%")] private int _magicalDefenceBonus;
        [SerializeField] private int _speedBonus;

        public override Guid EffectId => _defaultId;

        public override BattleEffect GetEffect(BattleUnit target, BattleUnit owner, Guid id = default)
        {
            if (id == default)
                id = EffectId;

            var settings = new IncreaseStatisticsBattleEffectSettings()
            {
                Id = id,
                Icon = Icon,
                TicksCount = TicksCount,
                Owner = owner,
                Target = target,

                HpBonus = _hpBonus,
                AttackBonus = _attackBonus,
                MagicalAttackBonus = _magicalAttackBonus,
                DefenceBonus = _defenceBouns,
                MagicalDefenceBonus = _magicalDefenceBonus,
                SpeedBonus = _speedBonus,
            };

            return new IncreaseStatisticsBattleEffect(settings);
        }
    }
}
