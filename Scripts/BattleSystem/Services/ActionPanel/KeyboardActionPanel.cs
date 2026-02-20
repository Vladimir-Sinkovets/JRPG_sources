using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Unit;
using Assets.Game.Scripts.Common.UI.Panels;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Game.Scripts.BattleSystem.Services.ActionPanel
{
    public class KeyboardActionPanel : MonoBehaviour, IActionPanelMenu
    {
        public event Action<BattleAbility> OnUseButtonClick;

        [Header("Info panel")]
        [SerializeField] private TMP_Text _unitName;
        [SerializeField] private TMP_Text _unitInfo;

        [SerializeField] private PanelBase _menuPanel;

        [SerializeField] private UnityEvent<BattleUnit> OnEnabled;
        [SerializeField] private UnityEvent<BattleUnit> OnDisabled;

        private BattleUnit _selectedUnit;

        public BattleUnit SelectedUnit { get => _selectedUnit; }

        public void Enable(BattleUnit unit)
        {
            OnEnabled?.Invoke(unit);

            _selectedUnit = unit;

            gameObject.SetActive(true);

            _unitName.text = unit.name;
            _unitInfo.text = "Description";

            _menuPanel.Show();
            _menuPanel.Select();
        }

        public void Disable()
        {
            OnDisabled?.Invoke(_selectedUnit);

            _menuPanel.Hide();
            _menuPanel.UnSelect();

            gameObject.SetActive(false);
        }

        public void UseAbility(BattleAbility ability)
        {
            OnUseButtonClick?.Invoke(ability);
        }
    }
}
