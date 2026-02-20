using Assets.Game.Scripts.Locations.Configs.Party;
using Assets.Game.Scripts.Locations.PlayerData.PartyData;
using PixelCrushers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Locations.SaveData
{
    public class PartySaver : Saver
    {
        [SerializeField] private Party _party;

        [Inject] private AlliesDataBase _dataBase;

        public override void ApplyData(string s)
        {
            var data = JsonUtility.FromJson<PartySaveData>(s);

            if (data != null)
            {
                var allies = data.AlliesIds.Select(id => _dataBase.GetAllyById(id));

                _party.SetAllies(allies);
            }
        }

        public override string RecordData()
        {
            return JsonUtility.ToJson(new PartySaveData()
            {
                AlliesIds = _party.Allies
                    .Select(allyConfig => _dataBase.GetId(allyConfig))
                    .ToList(),
            });
        }
    }

    public class PartySaveData
    {
        public List<string> AlliesIds;
    }
}