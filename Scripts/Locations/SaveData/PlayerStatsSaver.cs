using Assets.Game.Scripts.Common.Characters;
using Assets.Game.Scripts.Locations.PlayerData;
using PixelCrushers;
using System;
using UnityEngine;

namespace Assets.Game.Scripts.Locations.SaveData
{
    public class PlayerStatsSaver : Saver
    { 
        [SerializeField] private PlayerStats _playerStats;

        public override void ApplyData(string s)
        {
            var data = JsonUtility.FromJson<Data>(s);

            _playerStats.SetStats(data.Stats);
        }

        public override string RecordData()
        {
            var data = new Data
            {
                Stats = _playerStats.Stats
            };

            return JsonUtility.ToJson(data);
        }

        [Serializable]
        private class Data
        {
            public Stats Stats;
        }
    }
}
