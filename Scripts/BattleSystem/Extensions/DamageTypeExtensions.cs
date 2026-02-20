using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using System;

namespace Assets.Game.Scripts.BattleSystem.Extensions
{
    public static class DamageTypeExtensions
    {
        public static string GetName(this DamageType damageType)
        {
            return damageType switch
            {
                DamageType.Magic => "Magical",
                DamageType.Physical => "Physical",
                _ => throw new NotImplementedException(),
            };
        }
    }
}
