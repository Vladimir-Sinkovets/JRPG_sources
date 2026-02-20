using Assets.Game.Scripts.Common.Characters;
using System.Collections.Generic;
using UnityEngine;
using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Configs.Behaviours;

namespace Assets.Game.Scripts.BattleSystem.Data
{
    public class CharacterArgs
    {
        public Stats Stats;

        public GameObject Prefab;

        public List<BattleAbility> Abilities;

        public EnemyBehaviourConfig BehaviourConfig;
    }
}
