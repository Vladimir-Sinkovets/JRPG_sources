using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.BattleSystem.Unit.UI
{
    public class HpBar : MonoBehaviour
    {
        [SerializeField] private Image _bar;
        [SerializeField] private Image _background;
        [SerializeField] private TMP_Text _text;

        private BattleUnit _unit;

        public void SetUnit(BattleUnit unit)
        {
            _unit = unit;

            SetupEvents();
        }

        private void OnEnable()
        {
            if (_unit == null)
                return;

            SetupEvents();
        }

        private void SetupEvents()
        {
            _unit.Stats.OnHpChanged += OnHpChangedHandler;
            _unit.Stats.OnDied += OnDiedHandler;
            _unit.Stats.OnMaxHpChanged += OnMaxHpChangedHandler;
        }

        private void OnDisable()
        {
            _unit.Stats.OnHpChanged -= OnHpChangedHandler;
            _unit.Stats.OnDied -= OnDiedHandler;
            _unit.Stats.OnMaxHpChanged -= OnMaxHpChangedHandler;
        }

        private void OnMaxHpChangedHandler(UnitStats stats) => UpdateBar(stats);

        private void OnHpChangedHandler(UnitStats stats, int _, int __) => UpdateBar(stats);

        private void UpdateBar(UnitStats stats)
        {
            _bar.fillAmount = stats.Hp / (float)stats.MaxHp;

            _text.text = $"{stats.Hp}/{stats.MaxHp}";
        }

        private void OnDiedHandler(UnitStats stats)
        {
            _bar.gameObject.SetActive(false);
            _background.gameObject.SetActive(false);
        }
    }
}
