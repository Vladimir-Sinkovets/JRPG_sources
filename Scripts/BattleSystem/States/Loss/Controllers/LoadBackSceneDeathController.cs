using Assets.Game.Scripts.BattleSystem.Data;
using Assets.Game.Scripts.BattleSystem.Services.LossBattlePanels;
using Assets.Game.Scripts.BattleSystem.States.Loss.Interfaces;
using Assets.Game.Scripts.Common.Services.Coroutines;
using Assets.Game.Scripts.Common.Services.SceneLoading;
using Assets.Game.Scripts.Common.UniversalStateMachine;
using DG.Tweening;
using PixelCrushers;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.Loss.Controllers
{
    public class LoadBackSceneDeathController : ILoadBackSceneLossController
    {
        [SerializeField] private string _mainMenuSceneName = "Main_menu";

        //[Inject] private IBattleEndSceneLoader _sceneLoader;
        //[Inject] private BattleArgs _battleArgs;
        [Inject] private LossBattlePanel _lossBattlePanel;
        [Inject] private ICoroutineManager _coroutineManager;

        public void Activate()
        {
            _lossBattlePanel.Show();

            _lossBattlePanel.OnContinueButtonClicked += OnContinueButtonClickedHandler;

            //var results = new BattleResults()
            //{
            //    IsWon = true,
            //};

            //_sceneLoader.Load(_battleArgs.ReturnSceneName, results);
        }

        private void OnContinueButtonClickedHandler()
        {
            _lossBattlePanel.OnContinueButtonClicked -= OnContinueButtonClickedHandler;

            DOTween.KillAll();

            _coroutineManager.StopAll();

            SaveSystem.LoadScene(_mainMenuSceneName);
        }

        public void Deactivate()
        {
            _lossBattlePanel.OnContinueButtonClicked -= OnContinueButtonClickedHandler;
        }

        public void Init(LossDependencies dependencies, IStateSwitcher stateSwitcher) { }
    }
}
