using System;
using UnityEngine.SceneManagement;
using Zenject;

namespace Assets.Game.Scripts.Common.Services.SceneLoading
{
    public class ZenjectSceneLoaderWrapper
    {
        private readonly ZenjectSceneLoader _loader;

        public ZenjectSceneLoaderWrapper(ZenjectSceneLoader loader)
        {
            _loader = loader;
        }

        public void Load(string sceneName, Action<DiContainer> action = null)
            => _loader.LoadScene(sceneName, LoadSceneMode.Single, (container) => action?.Invoke(container));
    }
}