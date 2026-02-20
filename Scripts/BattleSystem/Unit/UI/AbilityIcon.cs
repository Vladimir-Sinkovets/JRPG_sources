using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.BattleSystem.Unit.UI
{
    public class AbilityIcon : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [Space]
        [SerializeField] private BattleUnit _unit;

        private void Awake()
        {
            _unit.Abilities.OnAbilityForUseChanged += OnAbilityForUseChangedHandler;
        }

        private void OnAbilityForUseChangedHandler(AbilityData data)
        {
            if (data == null)
            {
                _icon.gameObject.SetActive(false);
                return;
            }

            _icon.gameObject.SetActive(true);
            _icon.sprite = data.Ability.Sprite;
        }
    }
}
