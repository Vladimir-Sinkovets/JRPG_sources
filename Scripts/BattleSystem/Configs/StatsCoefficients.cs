using Assets.Game.Scripts.BattleSystem.Unit;
using System;
using TriInspector;

namespace Assets.Game.Scripts.BattleSystem.Configs
{
    [Serializable]
    public class StatsCoefficients
    {
        [Unit("%")] public int Hp;
        [Unit("%")] public int Attack;
        [Unit("%")] public int MagicalAttack;
        [Unit("%")] public int Defence;
        [Unit("%")] public int MagicalDefence;
        [Unit("%")] public int Speed;

        public static int CalculateDamage(StatsCoefficients statsCoefficients, UnitStats ownerStats)
        {
            return (ownerStats.Hp * statsCoefficients.Hp) / 100 +
                   (ownerStats.MagicalAttack * statsCoefficients.MagicalAttack) / 100 +
                   (ownerStats.Attack * statsCoefficients.Attack) / 100 +
                   (ownerStats.Defence * statsCoefficients.Defence) / 100 +
                   (ownerStats.MagicalDefence * statsCoefficients.MagicalDefence) / 100 +
                   (ownerStats.Speed * statsCoefficients.Speed) / 100;
        }
    }
}
