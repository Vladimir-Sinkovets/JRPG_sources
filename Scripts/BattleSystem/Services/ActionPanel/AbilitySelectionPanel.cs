using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Services.AbilityAvailabilityCheckers;
using Assets.Game.Scripts.BattleSystem.States;
using Assets.Game.Scripts.BattleSystem.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.Services.ActionPanel
{
    public class AbilitySelectionPanel : MonoBehaviour
    {
        [SerializeField] private Transform _abilitiesContainer;
        [SerializeField] private AbilityButton _abilityPrefab;

        [Header("Description panel")]
        [SerializeField] private Image _abilityImage;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _cost;
        [SerializeField] private TMP_Text _description;

        [Space]
        [SerializeField] private KeyboardActionPanel _actionPanel;

        [Inject] private IAbilityAvailabilityChecker _abilityChecker;
        [Inject] private BattleStateMachineData _stateMachineData;

        private List<AbilityButton> _pool = new List<AbilityButton>();

        private AbilityType _abilityType = AbilityType.Default;

        public void SetDefaultAbilityTypeFilter() => _abilityType = AbilityType.Default;
        public void SetSummoningAbilityTypeFilter() => _abilityType = AbilityType.Summoning;

        public void OnPanelOpened()
        {
            var abilities = _actionPanel.SelectedUnit.Abilities.Collection
                .Where(a => a.Type == _abilityType)
                .ToList();

            for (int i = 0; i < abilities.Count; i++)
            {
                var ability = abilities[i];

                if (i >= _pool.Count)
                {
                    var abilitybutton = Instantiate(_abilityPrefab, _abilitiesContainer);

                    _pool.Add(abilitybutton);
                }

                var isAbilityAvailable = _abilityChecker.IsAvailable(ability, _stateMachineData.selectedUnit, out _);

                _pool[i].Init(ability, isAbilityAvailable);

                _pool[i].OnClick += OnAbilityButtonClickHandler;
                _pool[i].OnSelected += OnAbilityButtonSelectedHandler;
                _pool[i].OnDeselected += OnAbilityButtonDeselectedHandler;
            }

            SetupNavigation(abilities.Count);

            if (abilities.Count > 0)
            {
                EventSystem.current.SetSelectedGameObject(_pool[0].gameObject);
            }
        }

        

        private void SetupNavigation(int abilityCount)
        {
            for (int i = 0; i < abilityCount; i++)
            {
                var selectable = _pool[i].GetComponent<Selectable>();
                var navigation = selectable.navigation;
                navigation.mode = Navigation.Mode.Explicit;

                int nextIndex = (i + 1) % abilityCount;
                int prevIndex = (i - 1 + abilityCount) % abilityCount;

                navigation.selectOnDown = _pool[nextIndex].GetComponent<Selectable>();
                navigation.selectOnUp = _pool[prevIndex].GetComponent<Selectable>();

                navigation.selectOnLeft = null;
                navigation.selectOnRight = null;

                selectable.navigation = navigation;
            }
        }

        public void OnPanelClosed()
        {
            foreach (var ability in _pool)
            {
                ability.Disable();
            }
        }

        private void OnAbilityButtonDeselectedHandler(BattleAbility ability) { }

        private void OnAbilityButtonSelectedHandler(BattleAbility ability)
        {
            _title.text = ability.Name;
            _abilityImage.sprite = ability.Sprite;
            _description.text = ability.DescriptionWithoutCost;
            _cost.text = ability.CostDescription;
        }

        private void OnAbilityButtonClickHandler(BattleAbility ability)
        {
            if (_abilityChecker.IsAvailable(ability, _stateMachineData.selectedUnit, out _) == false)
            {
                // todo: show message
                Debug.LogError($"Ability {ability.name} is unavailable");
            }
            else
            {
                _actionPanel.UseAbility(ability);
            }
        }
    }
}
