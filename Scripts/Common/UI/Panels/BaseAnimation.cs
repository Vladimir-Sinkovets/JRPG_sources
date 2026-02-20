using Assets.Game.Scripts.Common.UI.Panels;
using DG.Tweening;
using System;
using System.Linq;
using TriInspector;
using UnityEngine;

namespace Assets.Game.Scripts.BattleSystem.Services.TargetSelections
{
    public class BaseAnimation : PanelAnimation
    {
        public enum AnimationDirection
        {
            Left,
            Right,
            Top,
            Bottom
        }

        [Header("Animation Settings")]
        [SerializeField] private float _animationDuration = 0.7f;
        [SerializeField] private Ease _easeShow = Ease.OutBack;
        [SerializeField] private Ease _easeShowFade = Ease.OutBack;
        [SerializeField] private Ease _easeHide = Ease.OutBack;
        [SerializeField] private Ease _easeHideFade = Ease.OutBack;
        [EnumToggleButtons]
        [SerializeField] private AnimationDirection _animationFromDirection = AnimationDirection.Left;

        private Vector2 _originalPosition;
        private Sequence _sequence;

        private void Awake()
        {
            var rectTransform = GetComponent<RectTransform>();
            _originalPosition = rectTransform.anchoredPosition;
        }

        public override void Show(PanelBase panel, Action onPanelShowed)
        {
            var rectTransform = panel.GetComponent<RectTransform>();
            var canvas = panel.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

            float screenOffsetHorizontal = canvas.rect.width * 0.7f;
            float screenOffsetVertical = canvas.rect.height * 0.7f;

            Vector2 startPosition = CalculateStartPosition(
                screenOffsetHorizontal,
                screenOffsetVertical
            );

            rectTransform.anchoredPosition = startPosition;
            panel.CanvasGroup.alpha = 0;

            if (_sequence != null && _sequence.IsActive())
                _sequence.Kill();

            _sequence = DOTween.Sequence();
            _sequence.Join(rectTransform.DOAnchorPos(_originalPosition, _animationDuration).SetEase(_easeShow));
            _sequence.Join(panel.CanvasGroup.DOFade(1, _animationDuration).SetEase(_easeShowFade));

            panel.CanvasGroup.gameObject.SetActive(true);
            onPanelShowed?.Invoke();
        }

        public override void Hide(PanelBase panel, Action onPanelHidden)
        {
            var rectTransform = panel.GetComponent<RectTransform>();
            var canvas = panel.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

            float screenOffsetHorizontal = canvas.rect.width * 0.7f;
            float screenOffsetVertical = canvas.rect.height * 0.7f;

            Vector2 endPosition = CalculateEndPosition(
                screenOffsetHorizontal,
                screenOffsetVertical
            );

            if (_sequence != null && _sequence.IsActive())
                _sequence.Kill();

            _sequence = DOTween.Sequence();
            _sequence.Join(rectTransform.DOAnchorPos(endPosition, _animationDuration).SetEase(_easeHide));
            _sequence.Join(panel.CanvasGroup.DOFade(0, _animationDuration).SetEase(_easeHideFade));

            _sequence.OnComplete(() =>
            {
                panel.CanvasGroup.gameObject.SetActive(false);
                onPanelHidden?.Invoke();
            });
        }

        private Vector2 CalculateStartPosition(float horizontalOffset, float verticalOffset)
        {
            return _animationFromDirection switch
            {
                AnimationDirection.Left => new Vector2(-horizontalOffset, _originalPosition.y),
                AnimationDirection.Right => new Vector2(horizontalOffset, _originalPosition.y),
                AnimationDirection.Top => new Vector2(_originalPosition.x, verticalOffset),
                AnimationDirection.Bottom => new Vector2(_originalPosition.x, -verticalOffset),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private Vector2 CalculateEndPosition(float horizontalOffset, float verticalOffset)
        {
            return _animationFromDirection switch
            {
                AnimationDirection.Left => new Vector2(-horizontalOffset, _originalPosition.y),
                AnimationDirection.Right => new Vector2(horizontalOffset, _originalPosition.y),
                AnimationDirection.Top => new Vector2(_originalPosition.x, verticalOffset),
                AnimationDirection.Bottom => new Vector2(_originalPosition.x, -verticalOffset),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void OnDestroy()
        {
            if (_sequence != null && _sequence.IsActive())
                _sequence.Kill();
        }
    }
}