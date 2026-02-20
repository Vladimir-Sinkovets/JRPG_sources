using Assets.Game.Scripts.Common.Characters;
using PixelCrushers;
using UnityEngine;

namespace Assets.Game.Scripts.Locations.PlayerData
{
    public class PlayerStats : MonoBehaviour
    {
        private Stats _stats; // todo: add Data type

        public Stats Stats { get => _stats; }

        public void SetStats(Stats stats) => _stats = stats;


        [ContextMenu("Log state")]
        public void ShowStatsInConsole()
        {
            if (_stats != null)
                Debug.Log($"Stats = {Stats}");
            else
                Debug.LogWarning("Stats is null");
        }
    }
}
