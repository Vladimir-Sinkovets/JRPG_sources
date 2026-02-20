using Assets.Game.Scripts.Common.UI.Panels;
using PixelCrushers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Game.Scripts.Locations.Services.PauseManagers
{
    [RequireComponent(typeof(PanelBase))]
    public class PauseSystemPanel : MonoBehaviour
    {
        [SerializeField] private string _mainMenuSceneName = "Main_menu";

        private PanelBase _panel;

        private void Awake()
        {
            _panel = GetComponent<PanelBase>();
        }

        public void OnMenuClick()
        {
            SaveSystem.LoadScene(_mainMenuSceneName);

            EventSystem.current.SetSelectedGameObject(null);

            _panel.UnSelect();
        }
    }
}
