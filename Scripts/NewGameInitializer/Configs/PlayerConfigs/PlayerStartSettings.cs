using Assets.Game.Scripts.Common.Characters;
using TriInspector;
using UnityEngine;

namespace Assets.Game.Scripts.NewGameInitializer.Configs.PlayerConfigs
{
    [CreateAssetMenu(fileName = "Player_start_settings", menuName = "Player/Start settings")]
    public class PlayerStartSettings : ScriptableObject
    {
        [SerializeField]
        [HideLabel]
        [InlineProperty]
        private Stats _stats;

        public Stats Stats { get => _stats; }
    }
}
