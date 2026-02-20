using Assets.Game.Scripts.BattleSystem.Unit;
using System;
using TriInspector;
using UnityEngine;

namespace Assets.Game.Scripts.BattleSystem.Effects.Factories
{
    public abstract class BattleEffectFactory : ScriptableObject
    {
        [SerializeField, SpritePreview] protected Sprite Icon;

        [SerializeField] protected int TicksCount;

        public abstract Guid EffectId { get; }

        public abstract BattleEffect GetEffect(BattleUnit target, BattleUnit owner, Guid id = default);
    }
}
