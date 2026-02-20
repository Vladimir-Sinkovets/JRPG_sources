using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Game.Scripts.Common.UI.Buttons
{
    [RequireComponent(typeof(Button))]
    public class ButtonAnimation : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerUpHandler,
        ISelectHandler,
        IDeselectHandler
    {
        [Header("Scale Animation")]
        public float hoverScale = 1.05f;
        public float clickScale = 0.95f;
        public float animationDuration = 0.2f;

        private Vector3 _originalScale;
        private Button _button;
        private bool _isSelected;

        void Awake()
        {
            _originalScale = transform.localScale;
            _button = GetComponent<Button>();
        }

        // --- Mouse Events ---
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_button.interactable) return;
            AnimateHover();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isSelected)
            {
                AnimateNormal();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_button.interactable) return;
            AnimateClick();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.hovered.Contains(gameObject)
                || _isSelected)
            {
                AnimateHover();
            }
            else
            {
                AnimateNormal();
            }
        }

        // --- Keyboard/Gamepad Events ---
        public void OnSelect(BaseEventData eventData)
        {
            if (!_button.interactable) return;
            _isSelected = true;
            AnimateHover();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            _isSelected = false;
            AnimateNormal();
        }

        // --- Animation Methods ---
        private void AnimateHover()
        {
            transform.DOKill();
            transform.DOScale(_originalScale * hoverScale, animationDuration)
                .SetEase(Ease.OutBack);
        }

        private void AnimateNormal()
        {
            transform.DOKill();
            transform.DOScale(_originalScale, animationDuration)
                .SetEase(Ease.OutQuad);
        }

        private void AnimateClick()
        {
            transform.DOKill();

            Sequence clickSequence = DOTween.Sequence();
            clickSequence.Append(transform.DOScale(_originalScale * clickScale, animationDuration * 0.3f)
                .SetEase(Ease.OutQuad));
            clickSequence.Append(transform.DOScale(_originalScale * hoverScale, animationDuration * 0.7f)
                .SetEase(Ease.OutBack));
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}