using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.Common.UI.Logs
{
    public class InfoLog : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _message;
        [Space]
        [SerializeField] private CanvasGroup _canvasGroup;

        private float _appearanceDuration;
        private float _disappearanceDuration;

        private float _lifeTime;

        private Vector2 _originalPosition;
        private float _logWidth;
        private Sequence _sequence;

        public void Init(string title, float lifeTime, float appearanceDuration, float disappearanceDuration, string message = "", Sprite sprite = null)
        {
            _disappearanceDuration = disappearanceDuration;
            _appearanceDuration = appearanceDuration;
            
            _title.text = title;
            _message.text = message;
            _lifeTime = lifeTime;
            
            if (sprite != null) _image.sprite = sprite;

            var rectTransform = _canvasGroup.GetComponent<RectTransform>();
            _logWidth = rectTransform.rect.width;
            _originalPosition = rectTransform.anchoredPosition;

            var leftPosition = new Vector2(-_logWidth, _originalPosition.y);
            rectTransform.anchoredPosition = leftPosition;

            _canvasGroup.alpha = 0;
            _sequence = DOTween.Sequence();

            _sequence.Join(_canvasGroup.DOFade(1, _appearanceDuration));
            _sequence.Join(rectTransform.DOAnchorPos(_originalPosition, _appearanceDuration));
            _sequence.SetUpdate(true);

            StartCoroutine(RemoveLog());
        }

        private IEnumerator RemoveLog()
        {
            yield return new WaitForSeconds(_lifeTime);

            _canvasGroup.DOFade(0, _disappearanceDuration)
                .OnComplete(() => Destroy(gameObject));
        }

        private void OnDestroy()
        {
            _sequence.Kill();
            DOTween.Kill(_canvasGroup);
        }
    }
}