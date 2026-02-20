using Assets.Game.Scripts.BattleSystem.Unit;
using System;
using UnityEngine;

namespace Assets.Game.Scripts.BattleSystem.Effects.EffectTypes
{
    public abstract class BattleEffectSettings
    {
        public int TicksCount;

        public Sprite Icon;

        public Guid Id;

        [NonSerialized] public BattleUnit Owner;
        [NonSerialized] public BattleUnit Target;
    }
}
