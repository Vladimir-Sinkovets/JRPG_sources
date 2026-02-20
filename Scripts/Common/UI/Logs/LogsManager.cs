using UnityEngine;

namespace Assets.Game.Scripts.Common.UI.Logs
{
    public class LogsManager : MonoBehaviour
    {
        [SerializeField] private InfoLog _logPrefab;
        [SerializeField] private RectTransform _container;
        [Space]
        [SerializeField] private float _appearanceDuration = 0.5f;
        [SerializeField] private float _lifeTime = 8;
        [SerializeField] private float _disappearanceDuration = 1;

        public void AddLog(string title, string message, Sprite sprite = null)
        {
            var log = Instantiate(_logPrefab, _container);

            log.Init(
                title: title,
                message: message,
                lifeTime: _lifeTime,
                appearanceDuration: _appearanceDuration,
                disappearanceDuration: _disappearanceDuration,
                sprite: sprite);
        }
    }
}
