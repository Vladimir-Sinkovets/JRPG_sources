using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.BattleSystem.Unit.UI
{
    public class EffectIcon : MonoBehaviour
    {
        [SerializeField] private MMProgressBar _progressBar;
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _timeText;
        [SerializeField] private TMP_Text _iconText;

        public void Init(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public void UpdateValues(string line)
        {
            _iconText.text = line;
        }
    }
}
