using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Unit.UI;
using System;

namespace Assets.Game.Scripts.BattleSystem.Effects.EffectTypes
{
    public class PeriodicDamageBattleEffect : BattleEffect
    {
        private readonly PeriodicDamageBattleEffectSettings _settings;

        public PeriodicDamageBattleEffect(PeriodicDamageBattleEffectSettings settings) : base(settings)
        {
            _settings = settings;
        }

        public override void Init(EffectIcon effectIcon)
        {
            base.Init(effectIcon);

            _effectIcon.Init(_settings.Icon);
        }

        public override void Tick()
        {
            base.Tick();

            _settings.Target.Stats.TakeDamage(_settings.PeriodicDamage, _settings.DamageType);
        }
    }

    [Serializable]
    public class PeriodicDamageBattleEffectSettings : BattleEffectSettings
    {
        public int PeriodicDamage;
        public DamageType DamageType;

        // todo: idle particles
        // todo: damage particles
        public PeriodicDamageBattleEffectSettings Copy() => (PeriodicDamageBattleEffectSettings)this.MemberwiseClone();
    }
}
