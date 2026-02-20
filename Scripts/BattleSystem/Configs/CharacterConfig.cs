using Assets.Game.Scripts.Common.Characters;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Configs.Behaviours;

namespace Assets.Game.Scripts.BattleSystem.Configs
{
    [CreateAssetMenu(fileName = "Enemy_config", menuName = "Battle/Enemy config")]
    public class CharacterConfig : ScriptableObject
    {
        [InlineProperty]
        [HideLabel]
        public Stats Stats;

        [Space]
        public GameObject Prefab;

        [SpritePreview]
        public Sprite Icon;

        [Space]
        [InlineEditor]
        public EnemyBehaviourConfig Behaviour;

        [ShowIf(nameof(Behaviour), null)]
        [ListDrawerSettings(
            AlwaysExpanded = true,
            Draggable = true,
            HideRemoveButton = false,
            HideAddButton = false,
            ShowElementLabels = true)]
        public List<BattleAbility> Abilities;
    }
}
