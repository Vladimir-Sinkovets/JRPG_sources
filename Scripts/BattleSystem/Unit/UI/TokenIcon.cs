using Assets.Game.Scripts.BattleSystem.Tokens;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.BattleSystem.Unit.UI
{
    public class TokenIcon : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Image _glowEffect;
        [SerializeField] private Image _canvasGroup;

        private Token _token;

        public Token Token { get => _token; }

        public void EnableGlow(bool enable)
        {
            if (_glowEffect != null)
            {
                _glowEffect.gameObject.SetActive(enable);

                if (enable)
                {
                    _glowEffect.transform.DOScale(1.2f, 0.5f).SetLoops(-1, LoopType.Yoyo);
                    _glowEffect.DOFade(0.7f, 0.5f).SetLoops(-1, LoopType.Yoyo);
                }
            }
        }


        public void SetToken(Token token) => _token = token;
        public void SetImage(Sprite sprite) => _image.sprite = sprite;

        private void OnDestroy()
        {
            transform.DOKill();

            if (_glowEffect != null)
            {
                _glowEffect.DOKill();
                _glowEffect.transform.DOKill();
            }
        }
    }
}