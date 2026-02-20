using Assets.Game.Scripts.Common.Input;
using Assets.Game.Scripts.Common.UI.Panels;
using Assets.Game.Scripts.Locations.Player;
using Assets.Game.Scripts.Locations.Services.Input;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.Wrappers;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Locations.Services.PauseManagers
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] private PanelBase _pausePanel;

        [Inject] private ILocationInputController _locationInputController;
        [Inject] private IUIInputController _UIInputController;

        private void Awake()
        {
            _pausePanel.CanvasGroup.gameObject.SetActive(false);

            _locationInputController.OnPause += OnPausedHandler;
        }

        private void OnPausedHandler()
        {
            _pausePanel.Show();
            _pausePanel.Select();

            DialogueManager.SetDialogueSystemInput(false);

            _pausePanel.OnPanelClosed += OnPanelClosedHandler;

            SwitchToUIInputMap();

            PauseGame();
        }

        public void OnPanelClosedHandler(PanelBase panel)
        {
            panel.OnPanelClosed -= OnPanelClosedHandler;

            SwitchToLocationInputMap();

            UnPauseGame();

            StartCoroutine(EnableInputWithDelay());
        }

        private IEnumerator EnableInputWithDelay()
        {
            yield return null;

            DialogueManager.SetDialogueSystemInput(true);
        }

        private void UnPauseGame()
        {
            // todo: add pause service
        }

        private void PauseGame()
        {
            // todo: add pause service
        }

        private void SwitchToUIInputMap()
        {
            _UIInputController.EnableMap();
            _locationInputController.DisableMap();
        }

        private void SwitchToLocationInputMap()
        {
            _UIInputController.DisableMap();
            _locationInputController.EnableMap();
        }

        private void OnDestroy()
        {
            DialogueManager.SetDialogueSystemInput(true);
            _locationInputController.OnPause -= OnPausedHandler;
        }
    }
}