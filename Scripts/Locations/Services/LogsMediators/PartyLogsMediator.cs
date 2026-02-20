using Assets.Game.Scripts.Common.UI.Logs;
using Assets.Game.Scripts.Locations.PlayerData.PartyData;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Locations.Services.LogsMediators
{
    public class PartyLogsMediator : MonoBehaviour
    {
        [SerializeField] private LogsManager _logsManager;

        [Inject] private Party _party;

        private void Start()
        {
            _party.OnAllyAdded += OnAllyAddedHandler;
        }

        private void OnAllyAddedHandler(AllyAddedEventArgs args)
        {
            _logsManager.AddLog(
                title: "Ally added",
                message: $"{args.Ally.Name}",
                sprite: args.Ally.Sprite);
        }

        private void OnDestroy()
        {
            _party.OnAllyAdded -= OnAllyAddedHandler;
        }
    }
}