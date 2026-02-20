using Assets.Game.Scripts.Common.UI.Logs;
using Assets.Game.Scripts.Locations.PlayerData.InventoryData;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Locations.Services.LogsMediators
{
    public class InventoryLogsMediator : MonoBehaviour
    {
        [SerializeField] private LogsManager _logsManager;

        [Inject] private Inventory _inventory;

        private void Start()
        {
            _inventory.OnItemAdded += OnItemAddedHandler;
        }

        private void OnItemAddedHandler(ItemAddedEventArgs args)
        {
            _logsManager.AddLog(
                title: "Item added",
                message: $"{args.Item.Name} x{args.Amount}",
                sprite: args.Item.Sprite);
        }

        private void OnDestroy()
        {
            _inventory.OnItemAdded -= OnItemAddedHandler;
        }
    }
}
