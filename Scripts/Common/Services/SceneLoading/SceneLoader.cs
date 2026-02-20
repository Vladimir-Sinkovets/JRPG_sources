using Assets.Game.Scripts.BattleSystem.Data;
using PixelCrushers;
using Zenject;

namespace Assets.Game.Scripts.Common.Services.SceneLoading
{
    public class SceneLoader : IBattleSceneLoader, IBattleEndSceneLoader, ISimpleSceneLoader
    {
        public const string BattleSceneName = "Battle";

        private readonly ZenjectSceneLoaderWrapper _loader;

        public SceneLoader(ZenjectSceneLoaderWrapper loader)
        {
            _loader = loader;
        }

        public void Load(string sceneName, BattleResults results)
        {
            SaveSystem.BeforeSceneChange();
            SaveSystem.RecordSavedGameData();

            _loader.Load(sceneName, container =>
            {
                container.BindInstance(results).AsSingle();

                container.Bind<IInitializable>().FromInstance(new SaveApplier());
            });
        }

        public void LoadBattle(BattleArgs args)
        {
            SaveSystem.BeforeSceneChange();
            SaveSystem.RecordSavedGameData();

            _loader.Load(BattleSceneName, container =>
            {
                container.BindInstance(args).AsSingle();

                //container.Bind<IInitializable>().FromInstance(new SaveApplier());
            });
        }

        public void LoadScene(string sceneName) => _loader.Load(sceneName);
    }
}
