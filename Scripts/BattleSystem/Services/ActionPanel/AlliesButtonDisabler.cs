using Assets.Game.Scripts.BattleSystem.Services.AbilityAvailabilityCheckers;
using Assets.Game.Scripts.BattleSystem.Services.BattlePositions;
using Assets.Game.Scripts.BattleSystem.Unit;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.Services.ActionPanel
{
    public class AlliesButtonDisabler : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _text;
        [Space]
        [SerializeField] private Color _disabledTextColor;
        [SerializeField] private Color _disabledImageColor;

        private Color? _normalTextColor = null;
        private Color? _normalImageColor = null;

        [Inject] private BattlePositionsData _battlePositionsData;

        public void Check(BattleUnit selectedUnit)
        {
            if (_normalTextColor == null)
                _normalTextColor = _text.color;

            if (_normalImageColor == null)
                _normalImageColor = _button.image.color;

            if (!selectedUnit.Abilities.Collection.Any(x => x.Type == Abilities.Base.AbilityType.Summoning))
                // почему здесь не используется проверка на доступность чеерез IAbilityAvailabilityChecker
                // потому что там проверка в том числе и на стоимость, а я хочу что бы игрок видел скилл даже ести ему не хватает поинтов на него
                // с проверкой на наличие места тоже самое, пусть игрок увидит сообщение, а не заблокированную кнопку
            {
                _button.enabled = false;

                _text.color = _disabledTextColor;
                _button.image.color = _disabledImageColor;
            }
            else
            {
                _button.enabled = true;

                _text.color = _normalTextColor.Value;
                _button.image.color = _normalImageColor.Value;
            }
        }
    }
}