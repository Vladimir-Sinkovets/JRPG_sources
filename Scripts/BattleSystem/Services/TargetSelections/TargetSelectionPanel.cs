using Assets.Game.Scripts.BattleSystem.Unit;
using Assets.Game.Scripts.Common.UI.Panels;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Game.Scripts.BattleSystem.Services.TargetSelections
{
    public class TargetSelectionPanel : MonoBehaviour, ITargetSelectionPanel
    {
        public event Action<BattleUnit> OnUnitSelected;
        public event Action<BattleUnit> OnUnitPointed;
        public event Action<BattleUnit> OnUnitUnpointed;
        public event Action<IEnumerable<BattleUnit>> OnTargetsSelected;

        [SerializeField] private PanelBase _panel;
        [Space]
        [SerializeField] private UnitSelectionButton _buttonPrefab;
        [SerializeField] private RectTransform _container;

        private List<UnitSelectionButton> _pool = new();

        private int _targetsCount = -1; // кол-во целей, которые надо набрать для завершения хода

        private List<BattleUnit> _selectedUnits = new();

        public void EnableForGroup(IEnumerable<BattleUnit> battleUnits, string groupName = "Group")
        {
            _panel.Show();

            _targetsCount = battleUnits.Count();

            var count = battleUnits.Count();

            var allButton = GetButtonFromPool(0);

            allButton.Enable(battleUnits, groupName);
            allButton.OnClicked += OnClickedHandler;
            allButton.OnPointed += OnPointedHandler;
            allButton.OnUnpointed += OnUnpointedHandler;

            //foreach (var battleUnit in battleUnits)
            //{
            //    OnUnitPointed?.Invoke(battleUnit);
            //}

            EventSystem.current.SetSelectedGameObject(allButton.gameObject);
        }

        public void Enable(IEnumerable<BattleUnit> battleUnits, int targetsCount)
        {
            _panel.Show();

            _targetsCount = Math.Min(targetsCount, battleUnits.Count());

            var count = battleUnits.Count();

            for (int i = 0; i < count; i++)
            {
                var button = GetButtonFromPool(i);

                var unit = battleUnits.ElementAt(i);

                button.Enable(unit);
                button.OnClicked += OnClickedHandler;
                button.OnPointed += OnPointedHandler;
                button.OnUnpointed += OnUnpointedHandler;
            }

            if (count > 0)
                EventSystem.current.SetSelectedGameObject(_pool.First().gameObject);
        }
        private UnitSelectionButton GetButtonFromPool(int i)
        {
            if (i >= _pool.Count)
            {
                var button = Instantiate(_buttonPrefab, _container);

                _pool.Add(button);
            }

            return _pool[i];
        }

        private void OnPointedHandler(IEnumerable<BattleUnit> units)
        {
            foreach (var unit in units)
            {
                OnUnitPointed?.Invoke(unit);
            }
        }
        private void OnUnpointedHandler(IEnumerable<BattleUnit> units)
        {
            foreach (var unit in units)
            {
                OnUnitUnpointed?.Invoke(unit);
            }
        }

        private void OnClickedHandler(IEnumerable<BattleUnit> units)
        {
            foreach (var unit in units)
            {
                if (!_selectedUnits.Contains(unit))
                {
                    _selectedUnits.Add(unit);

                    OnUnitSelected?.Invoke(unit);
                }
            }

            if (_selectedUnits.Count >= _targetsCount)
            {
                OnTargetsSelected?.Invoke(_selectedUnits);
            }
        }

        public void Disable()
        {
            foreach (var button in _pool)
            {
                button.Disable();
                button.OnClicked -= OnClickedHandler;
                button.OnPointed -= OnPointedHandler;
                button.OnUnpointed -= OnUnpointedHandler;
            }

            _selectedUnits.Clear();
            _targetsCount = -1;

            EventSystem.current.SetSelectedGameObject(null);

            _panel.Hide();
            _panel.UnSelect();
        }
    }
}
