using System;
using TriInspector;
using UnityEngine;

namespace Assets.Game.Scripts.Common.Characters
{
    [Serializable]
    public class Stats
    {
        public int Hp;
        public int Attack;
        public int MagicalAttack;
        [Unit("%")] public int Defence;
        [Unit("%")] public int MagicalDefence;
        public int Speed;

        public Stats Copy() => (Stats)MemberwiseClone();

        public void Clear()
        {
            Hp = 0;
            Attack = 0;
            MagicalAttack = 0;
            Defence = 0;
            MagicalDefence = 0;
            Speed = 0;
        }

        private static Stats _emptyStats = new();
        public static Stats Empty => _emptyStats;

        public static Stats operator +(Stats a, Stats b)
        {
            return new Stats()
            {
                Hp = a.Hp + b.Hp,
                Attack = a.Attack + b.Attack,
                MagicalAttack = a.MagicalAttack + b.MagicalAttack,
                Defence = a.Defence + b.Defence,
                MagicalDefence = a.MagicalDefence + b.MagicalDefence,
                Speed = a.Speed + b.Speed
            };
        }

        public override string ToString()
        {
            return $"Attack: {Attack}\n" +
                   $"MagicalAttack: {MagicalAttack}\n" +
                   $"Defence: {Defence}\n" +
                   $"MagicalDefence: {MagicalDefence}\n" +
                   $"Speed: {Speed}\n";
        }
    }
}
