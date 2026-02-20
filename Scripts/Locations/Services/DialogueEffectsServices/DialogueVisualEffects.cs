using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.Locations.Services.DialogueEffectsServices
{
    public class DialogueVisualEffects : MonoBehaviour
    {
        [SerializeField] private Image _blackImage;
        public void FadeIn(float time)
        {
            _blackImage.DOColor(Color.black, time);
        }

        public void FadeOut(float time)
        {
            _blackImage.DOColor(new Color(0, 0, 0, 0), time);
        }
    }
}
