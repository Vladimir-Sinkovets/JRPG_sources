using Assets.Game.Scripts.BattleSystem.Effects;
using Assets.Game.Scripts.BattleSystem.Services.UIPrefabs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Scripts.BattleSystem.Unit
{
    public class UnitEffects
    {
        private BattleUIPrefabs _battleUIPrefabs;

        private List<BattleEffect> _effects;
        private RectTransform _effectsUIContainer;

        public List<BattleEffect> Collection { get => _effects; }

        public void Init(BattleUIPrefabs battleUIPrefabs, UnityEngine.RectTransform effectsUIContainer)
        {
            _effects = new List<BattleEffect>();

            _effectsUIContainer = effectsUIContainer;

            _battleUIPrefabs = battleUIPrefabs;
        }

        public void Tick()
        {
            foreach (var effect in _effects.ToArray())
            {
                effect.Tick();

                if (effect.IsExpired)
                {
                    DeleteEffect(effect);
                }
            }
        }

        public void AddEffect(BattleEffect effect)
        {
            if (effect == null)
                return;

            var oldEffect = _effects.FirstOrDefault(x => x.Id == effect.Id);

            if (oldEffect != null)
            {
                DeleteEffect(oldEffect);
            }

            var effectIcon = UnityEngine.Object.Instantiate(_battleUIPrefabs.EffectUIItem, _effectsUIContainer);

            effect.Init(effectIcon);

            _effects.Add(effect);
        }

        private void DeleteEffect(BattleEffect effect)
        {
            effect.End();

            _effects.Remove(effect);

            UnityEngine.Object.Destroy(effect.EffectIcon.gameObject);
        }
    }
}
