using Assets.Game.Scripts.BattleSystem.Effects.EffectTypes;
using Assets.Game.Scripts.BattleSystem.Unit;
using System;
using UnityEngine;

namespace Assets.Game.Scripts.BattleSystem.Effects.Factories
{
    [CreateAssetMenu(fileName = "Periodic_damage_effect", menuName = "Battle/Effects/Periodic_damage")]
    public class PeriodicDamageBattleEffectFactory : BattleEffectFactory
    {
        [SerializeField] private int PeriodicDamage;

        private Guid _defaultId = new("551f080d-1c98-487f-8f57-16c46a7f26c7");

        public override Guid EffectId => _defaultId;

        public override BattleEffect GetEffect(BattleUnit target, BattleUnit owner, Guid id = default)
        {
            if (id == default)
                id = EffectId;

            var settings = new PeriodicDamageBattleEffectSettings()
            {
                Id = id,
                Icon = Icon,
                TicksCount = TicksCount,
                Owner = owner,
                Target = target,

                PeriodicDamage = PeriodicDamage,
            };

            return new PeriodicDamageBattleEffect(settings);
        }
    }
}
