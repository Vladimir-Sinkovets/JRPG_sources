using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Scripts.Locations.Services.InventoryPanel
{
    public class AbilityListView : MonoBehaviour
    {
        [SerializeField] private List<AbilityView> _abilityViews;

        private void Start()
        {
            //Clear();
        }

        public void SetAbilities(IEnumerable<BattleAbility> abilities)
        {
            Clear();

            var count = Mathf.Min(_abilityViews.Count, abilities.Count());

            for (int i = 0; i < count; i++)
            {
                var abilityView = _abilityViews.ElementAt(i);

                abilityView.gameObject.SetActive(true);

                abilityView.SetAbility(abilities.ElementAt(i));
            }
        }

        public void Clear()
        {
            foreach (var abilityView in _abilityViews)
                abilityView.gameObject.SetActive(false);
        }
    }
}
