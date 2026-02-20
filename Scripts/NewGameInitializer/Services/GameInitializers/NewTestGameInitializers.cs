using Assets.Game.Scripts.Locations.PlayerData;
using Assets.Game.Scripts.NewGameInitializer.Configs.PlayerConfigs;
using PixelCrushers;
using System.Linq;
using TriInspector;
using UnityEditor.SearchService;
using UnityEngine;

namespace Assets.Game.Scripts.NewGameInitializer.Services.GameInitializers
{
    public class NewTestGameInitializers : MonoBehaviour
    {
        [SerializeField] private PlayerStartSettings _startSettings;
        [SerializeField] private PlayerStats _playerData;
        [SerializeField, Scene] private string _startScene;
        [SerializeField] private string _startPoint;


        private void Start()
        {
            SaveSystem.ClearSavedGameData();

            _playerData.SetStats(_startSettings.Stats);

            var sceneName = _startScene.Split('/').Last().Replace(".unity", "");

            PixelCrushers.SaveSystem.LoadScene($"{sceneName}@{_startPoint}");
        }
    }
}
