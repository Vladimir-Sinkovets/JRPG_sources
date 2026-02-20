using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Unit;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.Configs.Behaviours
{
    public abstract class EnemyBehaviourConfig : ScriptableObject
    {
        public abstract EnemyBehaviour CreateBehavior(BattleUnit unit, DiContainer container);
    }

    public abstract class EnemyBehaviour : MonoBehaviour
    {
        public abstract void SetAbility(BattleUnit enemy);
        public abstract IEnumerable<BattleAbility> GetAbilities();
        public abstract IEnumerable<BattleAbility> InstantiateAbilities(BattleUnit unit, DiContainer container);
    }
}
