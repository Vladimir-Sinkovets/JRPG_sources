using Assets.Game.Scripts.Locations.Configs.Party;
using Assets.Game.Scripts.Locations.PlayerData.PartyData;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Locations.LocationObjects
{
    public class AllyRegistrar : MonoBehaviour
    {
        [SerializeField] private AllyConfig _ally;

        [Inject] private Party _party;

        public void AddAlly() => _party.AddAlly(_ally);
    }
}