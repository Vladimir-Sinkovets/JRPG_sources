using Assets.Game.Scripts.BattleSystem.Unit.UI;
using TriInspector;

namespace Assets.Game.Scripts.BattleSystem.Effects.EffectTypes
{
    public class IncreaseStatisticsBattleEffect : BattleEffect
    {
        private readonly IncreaseStatisticsBattleEffectSettings _settings;

        public IncreaseStatisticsBattleEffect(IncreaseStatisticsBattleEffectSettings settings) : base(settings)
        {
            _settings = settings;
        }

        public override void Init(EffectIcon effectIcon)
        {
            base.Init(effectIcon);

            _settings.Target.Stats.Attack += _settings.AttackBonus;
            _settings.Target.Stats.MagicalAttack += _settings.MagicalAttackBonus;
            _settings.Target.Stats.Defence += _settings.DefenceBonus;
            _settings.Target.Stats.MagicalDefence += _settings.MagicalDefenceBonus;
            _settings.Target.Stats.Speed += _settings.SpeedBonus;

            _settings.Target.Stats.Hp += _settings.HpBonus;
            _settings.Target.Stats.MaxHp += _settings.HpBonus;
        }

        public override void End()
        {
            base.End();

            _settings.Target.Stats.Attack -= _settings.AttackBonus;
            _settings.Target.Stats.MagicalAttack -= _settings.MagicalAttackBonus;
            _settings.Target.Stats.Defence -= _settings.DefenceBonus;
            _settings.Target.Stats.MagicalDefence -= _settings.MagicalDefenceBonus;
            _settings.Target.Stats.Speed -= _settings.SpeedBonus;

            _settings.Target.Stats.MaxHp -= _settings.HpBonus;
        }
    }

    public class IncreaseStatisticsBattleEffectSettings : BattleEffectSettings
    {
        public int HpBonus;
        public int AttackBonus;
        public int MagicalAttackBonus;
        public int DefenceBonus;
        public int MagicalDefenceBonus;
        public int SpeedBonus;

        // todo: idle particles
        // todo: damage particles
    }
}
