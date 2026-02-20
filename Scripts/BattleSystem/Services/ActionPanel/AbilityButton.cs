using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.BattleSystem.UI
{
    public class AbilityButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Selectable _selectable;

        private BattleAbility _ability;

        private bool _isAbilityAvailable;

        public event Action<BattleAbility> OnClick;
        public event Action<BattleAbility> OnSelected;
        public event Action<BattleAbility> OnDeselected;

        public Selectable Selectable { get => _selectable; }

        public void Init(BattleAbility ability, bool isAbilityAvailable)
        {
            // todo: mark as unavailable

            gameObject.SetActive(true);

            _ability = ability;

            _text.text = _ability.Name;

            if (isAbilityAvailable == false)
                _text.text += " unavailable";
        }

        public void Disable()
        {
            gameObject.SetActive(false);

            OnClick = null;
            OnSelected = null;
            OnDeselected = null;
        }

        public void OnClickHandler()
        {
            OnClick.Invoke(_ability);
        }

        public void OnSelectedHandler()
        {
            OnSelected?.Invoke(_ability);
        }

        public void OnDeselectedHandler()
        {
            OnDeselected?.Invoke(_ability);
        }
    }
}
