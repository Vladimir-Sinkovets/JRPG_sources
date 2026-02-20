using Assets.Game.Scripts.Common.UI.Panels;
using DG.Tweening;
using System;
using UnityEngine;

namespace Assets.Game.Scripts.Locations.Services.PauseManagers
{
    public class PauseMenuPanelAnimation : PanelAnimation
    {
        [Header("Animation Settings")]
        [SerializeField] private float _animationDuration = 0.7f;
        [SerializeField] private Ease _easeShowType = Ease.OutBack;
        [SerializeField] private Ease _easeHideType = Ease.OutBack;
        [Space]
        [SerializeField] private RectTransform _pauseMenuPanel;

        private Vector2 _originalPosition;
        private float _panelWidth;
        private Sequence _sequence;

        private void Awake()
        {
            _originalPosition = _pauseMenuPanel.anchoredPosition;
            _panelWidth = _pauseMenuPanel.rect.width;
        }

        public override void Show(PanelBase panel, Action onPanelShowed)
        {
            _sequence.Kill(complete: true);

            var panelCanvasGroup = panel.CanvasGroup;

            panelCanvasGroup.gameObject.SetActive(true);
            panelCanvasGroup.interactable = true;
            panelCanvasGroup.blocksRaycasts = true;
            panelCanvasGroup.alpha = 0;

            Vector2 leftPosition = new Vector2(
                -_panelWidth,
                _originalPosition.y
            );
            _pauseMenuPanel.anchoredPosition = leftPosition;
            _sequence = DOTween.Sequence();

            _sequence.Join(panelCanvasGroup.DOFade(1f, _animationDuration));

            _sequence.Join(
                _pauseMenuPanel.DOAnchorPosX(_originalPosition.x, _animationDuration)
                    .SetEase(_easeShowType)
            );

            _sequence.SetUpdate(true);

            onPanelShowed?.Invoke();
        }

        public override void Hide(PanelBase panel, Action onPanelHided)
        {
            _sequence.Kill(complete: true);
            _sequence = DOTween.Sequence();

            var panelCanvasGroup = panel.CanvasGroup;

            panelCanvasGroup.interactable = false;
            panelCanvasGroup.blocksRaycasts = false;

            _sequence.Join(panelCanvasGroup.DOFade(0f, _animationDuration * 0.7f));

            _sequence.Join(
                _pauseMenuPanel.DOAnchorPosX(-_panelWidth, _animationDuration * 0.7f)
                    .SetEase(_easeHideType)
            );

            _sequence.OnComplete(() => {
                _pauseMenuPanel.anchoredPosition = _originalPosition;
                panelCanvasGroup.gameObject.SetActive(false);
            });

            _sequence.SetUpdate(true);

            onPanelHided?.Invoke();
        }

        private void OnDestroy()
        {
            _sequence.Kill(complete: true);
        }
    }
}
