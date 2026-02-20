using Assets.Game.Scripts.BattleSystem.Effects.EffectTypes;
using Assets.Game.Scripts.BattleSystem.Unit.UI;
using System;

namespace Assets.Game.Scripts.BattleSystem.Effects
{
    public abstract class BattleEffect
    {
        protected EffectIcon _effectIcon;

        protected int _ticksCount;

        private readonly BattleEffectSettings _settings;

        public Guid Id { get => _settings.Id; }
        public EffectIcon EffectIcon { get => _effectIcon; }
        public bool IsExpired { get; protected set; }

        protected BattleEffect(BattleEffectSettings settings)
        {
            _settings = settings;
        }

        public virtual void Init(EffectIcon effectIcon)
        {
            _effectIcon = effectIcon;

            IsExpired = false;

            UpdateUI();
        }

        public virtual void End() { }

        public virtual void Tick()
        {
            _ticksCount++;

            IsExpired = _ticksCount >= _settings.TicksCount;

            UpdateUI();
        }

        protected virtual void UpdateUI()
        {
            EffectIcon.UpdateValues((_settings.TicksCount - _ticksCount).ToString());
        }
    }
}
