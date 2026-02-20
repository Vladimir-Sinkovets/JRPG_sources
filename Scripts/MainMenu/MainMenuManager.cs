using Assets.Game.Scripts.Common.Services.SceneLoading;
using Assets.Game.Scripts.Common.UI.Panels;
using System.Linq;
using TriInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Assets.Game.Scripts.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        [Scene] public string _testSceneName;
        [Scene] public string _startGameSceneName;
        [Space]
        [SerializeField] private PanelBase _panel;

        [Inject] private ISimpleSceneLoader _simpleSceneLoader;

        private void Start()
        {
            _panel.Show();
            _panel.Select();
        }

        public void StartTestField()
        {
            var sceneName = _testSceneName.Split('/').Last().Replace(".unity", "");

            _simpleSceneLoader.LoadScene(sceneName);

            EventSystem.current.enabled = false;
        }

        public void StartGame()
        {
            var sceneName = _startGameSceneName.Split('/').Last().Replace(".unity", "");

            _simpleSceneLoader.LoadScene(sceneName);

            EventSystem.current.enabled = false;
        }
    }
}