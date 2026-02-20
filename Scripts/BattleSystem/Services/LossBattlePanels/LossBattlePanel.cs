using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Game.Scripts.BattleSystem.Services.LossBattlePanels
{
    public class LossBattlePanel : MonoBehaviour
    {
        public event Action OnContinueButtonClicked;

        [SerializeField] private RectTransform _container;
        [SerializeField] private float _slideDuration = 0.5f;
        [SerializeField] private Ease _easeType = Ease.OutCubic;
        [Space]
        [SerializeField] private GameObject _firstSelected;

        private Vector2 _originalPosition;

        private void Awake()
        {
            _originalPosition = _container.anchoredPosition;

            _container.gameObject.SetActive(false);
        }

        public void Show()
        {
            _container.gameObject.SetActive(true);

            _originalPosition = _container.anchoredPosition;

            gameObject.SetActive(true);

            _container.anchoredPosition = new Vector2(
                -_container.rect.width,
                _originalPosition.y
            );

            _container.DOAnchorPos(_originalPosition, _slideDuration)
                .SetEase(_easeType);

            EventSystem.current.SetSelectedGameObject(_firstSelected);
        }

        public void OnMenuButtonClicked() => OnContinueButtonClicked?.Invoke();
    }
}
