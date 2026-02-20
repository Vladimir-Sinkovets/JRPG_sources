using UnityEngine;

namespace Assets.Game.Scripts.BattleSystem.Unit.UI
{
    public class UnitUI : MonoBehaviour
    {
        [SerializeField] private HpBar _hpBar;
        [SerializeField] private TurnWaitingProgressBar _turnWaitingProgressBar;
        [SerializeField] private UnitTokens _unitTokens;
        [SerializeField] private RectTransform _effectUIContainer;
        [SerializeField] private RectTransform canvas;

        public RectTransform EffectUIContainer { get => _effectUIContainer; }
        public UnitTokens Tokens { get => _unitTokens; }
        public RectTransform Canvas { get => canvas; }

        public void Init(BattleUnit unit)
        {
            _hpBar.SetUnit(unit);
            _turnWaitingProgressBar.SetUnit(unit);
            _unitTokens.SetUnit(unit);
        }
    }
}