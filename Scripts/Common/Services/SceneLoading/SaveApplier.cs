using PixelCrushers;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Common.Services.SceneLoading
{
    public class SaveApplier : IInitializable
    {
        public void Initialize()
        {
            SaveSystem.ApplySavedGameData();
        }
    }
}
