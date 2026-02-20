using Assets.Game.Scripts.BattleSystem.Services.AbilityAvailabilityCheckers;
using Assets.Game.Scripts.BattleSystem.Services.ActionPanel;
using Assets.Game.Scripts.BattleSystem.Services.Animators;
using Assets.Game.Scripts.BattleSystem.Services.BattlePositions;
using Assets.Game.Scripts.BattleSystem.Services.CameraFocusServices;
using Assets.Game.Scripts.BattleSystem.Services.FormulasServices;
using Assets.Game.Scripts.BattleSystem.Services.Highlighter;
using Assets.Game.Scripts.BattleSystem.Services.Input;
using Assets.Game.Scripts.BattleSystem.Services.LossBattlePanels;
using Assets.Game.Scripts.BattleSystem.Services.ParticlesServices;
using Assets.Game.Scripts.BattleSystem.Services.SceneDataAccessor;
using Assets.Game.Scripts.BattleSystem.Services.TargetSelections;
using Assets.Game.Scripts.BattleSystem.Services.TokensCostCheckers;
using Assets.Game.Scripts.BattleSystem.Services.UIPrefabs;
using Assets.Game.Scripts.BattleSystem.Services.UIStateInfo;
using Assets.Game.Scripts.BattleSystem.Services.UnitFactory;
using Assets.Game.Scripts.BattleSystem.Services.UnitPointers;
using Assets.Game.Scripts.BattleSystem.Services.WinBattlePanels;
using TriInspector;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.Installers
{
    public class BattleInstaller : MonoInstaller
    {
        [Title("UI")]
        [SerializeField] private StateInfoService _stateInfo;
        [SerializeField] private TargetSelectionPanel _targetSelectionPanel;
        [SerializeField] private KeyboardActionPanel _keyboardActionPanel;
        [SerializeField] private WinBattlePanel _winBattlePanel;
        [SerializeField] private LossBattlePanel _lossBattlePanel;
        [Title("Highlight")]
        [SerializeField] private UnitHighlighter _unitHighlighter;
        [SerializeField] private UnitPointer _unitPointer;
        [Title("Configs")]
        [SerializeField] private BattlePositionsData _battlePositionsData;
        [SerializeField] private BattleUIPrefabs _battleUIPrefabs;
        [Title("VFX")]
        [SerializeField] private CinemachineImpulseListener _impulseListener;
        [SerializeField] private CameraFocusService _cameraFocusService;
        [SerializeField] private ParticlesService _particlesService;

        public override void InstallBindings()
        {
            BindServices();

            BindUI();

            BindVFX();

            BindConfigs();
        }

        private void BindServices()
        {
            Container.BindInterfacesAndSelfTo<BattleUnitContainer>()
                .AsSingle();

            Container.Bind<IUnitHighlighter>()
                .FromInstance(_unitHighlighter);

            Container.BindInterfacesAndSelfTo<UnitPointer>()
                .FromInstance(_unitPointer)
                .AsSingle();

            Container.Bind<IBattleInput>()
                .To<BattleInput>()
                .AsSingle();

            Container.Bind<IBattleUnitFactory>()
                .To<BattleUnitFactory>()
                .AsSingle();

            Container.Bind<ITokenCombinationAnimator>()
                .To<TokenCombinationAnimator>()
                .AsSingle();

            Container.Bind<ITokensCostChecker>()
                .To<TokensCostChecker>()
                .AsSingle();

            Container.Bind<FormulasService>()
                .AsTransient();

            Container.Bind<IAbilityAvailabilityChecker>()
                .To<AbilityAvailabilityChecker>()
                .AsSingle();
        }

        private void BindUI()
        {
            Container.BindInterfacesTo<TargetSelectionPanel>()
                .FromInstance(_targetSelectionPanel);

            Container.Bind<IStateInfoService>()
                .FromInstance(_stateInfo);

            Container.Bind<IActionPanelMenu>()
                .FromInstance(_keyboardActionPanel)
                .AsSingle();

            Container.BindInstance(_winBattlePanel);

            Container.BindInstance(_lossBattlePanel);
        }

        private void BindConfigs()
        {
            Container.BindInstance(_battleUIPrefabs);

            Container.Bind<BattlePositionsData>()
                .FromInstance(_battlePositionsData);
        }

        private void BindVFX()
        {
            Container.Bind<Camera>()
                .FromInstance(Camera.main);

            Container.BindInstance(_impulseListener);

            Container.Bind<ICameraFocusService>()
                .FromInstance(_cameraFocusService)
                .AsSingle();

            Container.Bind<IParticlesService>()
                .FromInstance(_particlesService)
                .AsSingle();
        }
    }
}
