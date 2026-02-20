using Assets.Game.Scripts.Common.Characters;
using TMPro;
using UnityEngine;

namespace Assets.Game.Scripts.Locations.Services.InventoryPanel
{
    public class StatsComparisonPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _statsNames;
        [SerializeField] private TMP_Text _statsValues;


        public void SetStats(Stats stats, Stats statsCompareWith)
        {
            Clear();

            if (stats == null) return;

            string FormatValue(int value, int? compareValue)
            {
                if (compareValue == null)
                    return $"{value}";

                int cmp = compareValue.Value;
                int diff = value - cmp;

                string diffString = diff switch
                {
                    > 0 => $"(+{diff})",
                    < 0 => $"({diff})",
                    _ => ""
                };

                if (value > cmp)
                    return $"<color=blue>{value} {diffString}</color>";
                else if (value < cmp)
                    return $"<color=red>{value} {diffString}</color>";
                else
                    return $"{value}";
            }

            void AddStat(string label, int value, int? compareValue)
            {
                _statsNames.text += label + "\n";
                _statsValues.text += FormatValue(value, compareValue) + "\n";
            }

            AddStat("HP:", stats.Hp, statsCompareWith?.Hp);
            AddStat("ATK:", stats.Attack, statsCompareWith?.Attack);
            AddStat("MATK:", stats.MagicalAttack, statsCompareWith?.MagicalAttack);
            AddStat("DEF:", stats.Defence, statsCompareWith?.Defence);
            AddStat("MDEF:", stats.MagicalDefence, statsCompareWith?.MagicalDefence);
            AddStat("SPD:", stats.Speed, statsCompareWith?.Speed);
        }

        public void Clear()
        {
            _statsNames.text = string.Empty;
            _statsValues.text = string.Empty;
        }
    }
}
