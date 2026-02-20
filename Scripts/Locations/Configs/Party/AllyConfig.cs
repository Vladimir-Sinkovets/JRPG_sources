using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Configs;
using Assets.Game.Scripts.Common.Characters;
using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace Assets.Game.Scripts.Locations.Configs.Party
{
    [DeclareBoxGroup("Stats")]
    [DeclareHorizontalGroup("Stats/1")]
    [CreateAssetMenu(fileName = "Ally_config", menuName = "Battle/Ally_config")]
    public class AllyConfig : ScriptableObject
    {
        public string Id;
        public string Name;
        public Sprite Sprite;
        public GameObject Prefab;

        [Group("Stats/1")]
        [InlineProperty]
        [HideLabel]
        public Stats Stats;

        [Group("Stats/1")]
        [InlineProperty]
        [LabelWidth(60)]
        public Stats Increase;

        [Title("Abilities")]

        [ListDrawerSettings(AlwaysExpanded = true)]
        [SerializeField] private List<BattleAbility> _baseAbilities;

        [TableList(AlwaysExpanded = true)]
        [SerializeField] private List<AllyLeveLData> _levels;

        public List<BattleAbility> Abilities { get => _baseAbilities; }

        [Button("Sort levels")]
        private void Sort()
        {
            _levels.Sort();
        }

        #region Id generation
        [Button("Generate unique id")]
        private void GenerateId()
        {
            Id = Guid.NewGuid().ToString();
        }
        #endregion
    }

    [Serializable]
    public class AllyLeveLData : IComparable<AllyLeveLData>
    {
        public int Level;
        [ListDrawerSettings(AlwaysExpanded = true)]
        public List<BattleAbility> Abilities;

        public int CompareTo(AllyLeveLData other)
        {
            if (other == null) return 1;
            return Level.CompareTo(other.Level);
        }
    }
}