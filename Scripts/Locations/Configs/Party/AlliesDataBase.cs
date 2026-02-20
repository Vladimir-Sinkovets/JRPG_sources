using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEngine;

namespace Assets.Game.Scripts.Locations.Configs.Party
{
    [CreateAssetMenu(fileName = "Allies_data_base", menuName = "Battle/Allies_data_base")]
    public class AlliesDataBase : ScriptableObject
    {
        [SerializeField, TableList(AlwaysExpanded = true)] private List<AllyData> _allies;

        public List<AllyConfig> Allies { get => _allies.Select(x => x.Ally).ToList(); }

        public string GetId(AllyConfig allyConfig)
        {
            return _allies.FirstOrDefault(x => x.Ally == allyConfig)?.Ally.Id;
        }

        public AllyConfig GetAllyById(string id)
        {
            return _allies.FirstOrDefault(a => a.Ally.Id == id)?.Ally;
        }

        public BattleAbility GetAbilityById(string id)
        {
            return _allies.FirstOrDefault(a => a.Ally.Id == id)?.SummonAbility;
        }
    }

    [Serializable]
    public class AllyData
    {
        public AllyConfig Ally;
        public BattleAbility SummonAbility;
    }
}