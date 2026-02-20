using Assets.Game.Scripts.Common.Characters;
using TMPro;
using UnityEngine;

namespace Assets.Game.Scripts.Locations.Services.InventoryPanel
{
    public class StatsPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _statsNames;
        [SerializeField] private TMP_Text _statsValues;

        public void SetStats(Stats stats)
        {
            Clear();

            if (stats.Hp != 0)
            {
                _statsNames.text += "HP:\n";
                _statsValues.text += $"{stats.Hp}\n";
            }

            if (stats.Attack != 0)
            {
                _statsNames.text += "ATK:\n";
                _statsValues.text += $"{stats.Attack}\n"; ;
            }

            if (stats.MagicalAttack != 0)
            {
                _statsNames.text += "MATK:\n";
                _statsValues.text += $"{stats.MagicalAttack}\n"; ;
            }

            if (stats.Defence != 0)
            {
                _statsNames.text += "DEF:\n";
                _statsValues.text += $"{stats.Defence}\n"; ;
            }

            if (stats.MagicalDefence != 0)
            {
                _statsNames.text += "MDEF:\n";
                _statsValues.text += $"{stats.MagicalDefence}\n"; ;
            }

            if (stats.Speed != 0)
            {
                _statsNames.text += "SPD:\n";
                _statsValues.text += $"{stats.Speed}\n"; ;
            }
        }

        public void Clear()
        {
            _statsNames.text = string.Empty;
            _statsValues.text = string.Empty;
        }
    }
}
