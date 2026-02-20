using System;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Game.Scripts.Common.UI.Panels
{
    [RequireComponent(typeof(PanelBase))]
    public class MenuPanel : MonoBehaviour
    {
        [ListDrawerSettings(AlwaysExpanded =  true)]
        [SerializeField] private MenuButtonSettings[] _settings;
        
        private PanelBase _panel;

        private void Awake()
        {
            _panel = GetComponent<PanelBase>();

            for (int i = 0; i < _settings.Length; i++)
            {
                var index = i;

                _settings[i].Button.onClick.AddListener(() => ActivateTab(index));

                _settings[i].Panel.Hide();
            }
        }

        public void HideAll()
        {
            foreach (var action in _settings)
            {
                action.Panel.Hide();
                action.Panel.UnSelect();
            }
        }

        private void ActivateTab(int index)
        {
            _panel.LastSelected = EventSystem.current.currentSelectedGameObject;

            _settings[index].EventBeforePanelOpened?.Invoke();

            _settings[index].Panel.Show();
            _settings[index].Panel.Select();

            _settings[index].Panel.OnPanelClosed += OnTabPanelClosedHandler;

            _panel.UnSelect();
        }

        private void OnTabPanelClosedHandler(PanelBase panel)
        {
            panel.OnPanelClosed -= OnTabPanelClosedHandler;

            panel.UnSelect();

            _panel.Select();
        }

        [Serializable]
        public class MenuButtonSettings
        {
            public Button Button;
            public PanelBase Panel;
            public UnityEvent EventBeforePanelOpened;
        }
    }
}
