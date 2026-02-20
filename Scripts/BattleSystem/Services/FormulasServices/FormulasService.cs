using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Unit;
using System;
using UnityEngine;

namespace Assets.Game.Scripts.BattleSystem.Services.FormulasServices
{
    public class FormulasService
    {
        public int CalculateDamage(Unit.BattleUnit owner, DamageType damageType, int randomDeviation = 5)
        {
            var sameDamage = 0;

            switch (damageType)
            {
                case DamageType.Magic:
                    sameDamage = owner.Stats.MagicalAttack;
                    break;
                case DamageType.Physical:
                    sameDamage = owner.Stats.Attack;
                    break;
                default:
                    Debug.LogError($"Has no switch case for {damageType}");
                    break;
            }

            var offset = sameDamage * UnityEngine.Random.Range(-randomDeviation, randomDeviation) / 100;

            return sameDamage + offset;
        }

        public int ReduceDamageByType(BattleUnit unit, DamageType damageType, int damage)
        {
            var reducedDamage = 0;

            switch (damageType)
            {
                case DamageType.Magic:
                    reducedDamage = ReduceDamage(damage, unit.Stats.MagicalDefence);
                    break;
                case DamageType.Physical:
                    reducedDamage = ReduceDamage(damage, unit.Stats.Defence);
                    break;
                default:
                    Debug.LogError($"Has no switch case for {damageType}");
                    break;
            }

            return reducedDamage;
        }

        private int ReduceDamage(int damage, int defence) => (int)((float)damage * (100 - defence) / 100);
    }
}
