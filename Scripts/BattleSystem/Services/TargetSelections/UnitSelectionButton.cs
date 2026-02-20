using Assets.Game.Scripts.BattleSystem.Unit;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.BattleSystem.Services.TargetSelections
{
    public class UnitSelectionButton : MonoBehaviour
    {
        public event Action<IEnumerable<BattleUnit>> OnClicked;
        public event Action<IEnumerable<BattleUnit>> OnPointed;
        public event Action<IEnumerable<BattleUnit>> OnUnpointed;

        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _name;
        [Space]
        [SerializeField] private Image _selectionImage;
        [Space]
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _normalColor;

        private List<BattleUnit> _battleUnits = new();

        public void Enable(BattleUnit battleUnit)
        {
            gameObject.SetActive(true);

            _name.text = battleUnit.name; // todo: add real name
            //_image.sprite = sprite; // todo: add sprite

            _battleUnits.Clear();

            _battleUnits.Add(battleUnit);

            _selectionImage.color = _normalColor;
        }

        public void Enable(IEnumerable<BattleUnit> battleUnits, string title)
        {
            gameObject.SetActive(true);

            _name.text = title;

            _battleUnits.Clear();

            _battleUnits.AddRange(battleUnits);

            _selectionImage.color = _normalColor;
        }

        public void Disable()
        {
            _battleUnits.Clear();

            gameObject.SetActive(false);

            _selectionImage.color = _normalColor;
        }

        public void OnClickedHandler()
        {
            _selectionImage.color = _selectedColor;

            OnClicked?.Invoke(_battleUnits);
        }

        public void OnPointedHandler()
        {
            OnPointed?.Invoke(_battleUnits);
        }

        public void OnUnpointedHandler()
        {
            OnUnpointed?.Invoke(_battleUnits);
        }
    }
}