using Assets.Game.Scripts.Common.Input;
using Assets.Game.Scripts.Common.Services.Audio;
using Assets.Game.Scripts.Common.Services.Coroutines;
using Assets.Game.Scripts.Common.Services.SceneLoading;
using Assets.Game.Scripts.Locations.Services.Input;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Common.Installers
{
    public class GlobalServicesInstaller : MonoInstaller
    {
        [SerializeField] private CoroutineManager _coroutineManager;
        [SerializeField] private AudioService _audioService;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SceneLoader>()
                .AsSingle();

            Container.Bind<ZenjectSceneLoaderWrapper>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerInputWrapper>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<UIInputController>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<LocationInputController>()
                .AsSingle();

            Container.Bind<ICoroutineManager>()
                .FromInstance(_coroutineManager);

            Container.Bind<IAudioService>()
                .FromInstance(_audioService);
        }
    }
}
