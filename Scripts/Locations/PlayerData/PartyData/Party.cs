using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.Locations.Configs.Party;
using System;
using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Locations.PlayerData.PartyData
{
    public class Party : MonoBehaviour
    {
        public event Action<AllyAddedEventArgs> OnAllyAdded;

        [SerializeField, DisableInEditMode, DisableInPlayMode]
        private List<AllyConfig> _allies = new();

        public List<AllyConfig> Allies { get => _allies; }

        [Inject] private AlliesDataBase _alliesDataBase;

        public void AddAlly(AllyConfig ally)
        {
            if (_allies.Contains(ally))
                return;

            _allies.Add(ally);

            OnAllyAdded?.Invoke(new AllyAddedEventArgs()
            {
                Ally = ally,
            });
        }

        public void SetAllies(IEnumerable<AllyConfig> allies)
        {
            _allies = allies.ToList();
        }

        public IEnumerable<BattleAbility> GetSummonAbilities()
        {
            return _allies.Select(a => _alliesDataBase.GetAbilityById(a.Id))
                .Where(a => a != null);
        }
    }

    public class AllyAddedEventArgs
    {
        public AllyConfig Ally;
    }
}