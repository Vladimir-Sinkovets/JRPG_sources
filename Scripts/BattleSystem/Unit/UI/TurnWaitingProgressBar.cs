using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.BattleSystem.Unit.UI
{
    public class TurnWaitingProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _bar;
        [SerializeField] private Image _background;
        [SerializeField] private TMP_Text _text;

        private BattleUnit _unit;

        public void SetUnit(BattleUnit unit) => _unit = unit;

        private void Start()
        {
            _unit.Stats.OnTurnProgressChanged += OnTurnProgressChangedHandler;
            _unit.Stats.OnDied += OnDiedHandler;
        }

        private void OnDiedHandler(UnitStats _)
        {
            _bar.gameObject.SetActive(false);
            _background.gameObject.SetActive(false);
        }

        private void OnTurnProgressChangedHandler(UnitStats unit)
        {
            _bar.fillAmount = (float)unit.TurnProgress / BattleSystemConstants.TurnWaitingProgressMax;

            _text.text = ((int)(_bar.fillAmount * 100)).ToString("") + '%';
        }
    }
}
