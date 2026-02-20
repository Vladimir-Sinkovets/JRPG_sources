using Assets.Game.Scripts.BattleSystem.Data;
using Assets.Game.Scripts.BattleSystem.Services.WinBattlePanels;
using Assets.Game.Scripts.BattleSystem.States.Win.Interfaces;
using Assets.Game.Scripts.Common.Services.Coroutines;
using Assets.Game.Scripts.Common.Services.SceneLoading;
using Assets.Game.Scripts.Common.UniversalStateMachine;
using DG.Tweening;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.Win.Controllers
{
    public class LoadBackSceneWinController : ILoadBackSceneWinController
    {
        [Inject] private readonly IBattleEndSceneLoader _sceneLoader;
        [Inject] private readonly BattleArgs _battleArgs;
        [Inject] private readonly WinBattlePanel _endBattlePanel;
        [Inject] private readonly ICoroutineManager _coroutineManager;

        public void Activate()
        {
            _endBattlePanel.Show();

            _endBattlePanel.OnContinueButtonClicked += OnContinueButtonClickedHandler;
        }

        private void OnContinueButtonClickedHandler()
        {
            _endBattlePanel.OnContinueButtonClicked -= OnContinueButtonClickedHandler;

            DOTween.KillAll();

            _coroutineManager.StopAll();

            var results = new BattleResults()
            {
                Id = _battleArgs.Id,
                IsWon = true,
            };

            _sceneLoader.Load(_battleArgs.ReturnSceneName, results);
        }

        public void Deactivate()
        {
            _endBattlePanel.OnContinueButtonClicked -= OnContinueButtonClickedHandler;
        }

        public void Init(WinDependencies dependencies, IStateSwitcher stateSwitcher) { }
    }
}
