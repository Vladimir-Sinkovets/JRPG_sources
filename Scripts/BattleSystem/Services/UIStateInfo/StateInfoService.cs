using TMPro;
using UnityEngine;

namespace Assets.Game.Scripts.BattleSystem.Services.UIStateInfo
{
    public class StateInfoService : MonoBehaviour, IStateInfoService
    {
        [SerializeField] private TMP_Text _tmp;
        public void ShowInfo(string info)
        {
            _tmp.text = info;
        }
    }
}
