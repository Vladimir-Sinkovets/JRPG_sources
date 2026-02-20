using Assets.Game.Scripts.BattleSystem.Unit.UI;
using UnityEngine;

namespace Assets.Game.Scripts.BattleSystem.Services.UIPrefabs
{
    public class BattleUIPrefabs : MonoBehaviour
    {
        [SerializeField] private EffectIcon _effectUIItem;

        public EffectIcon EffectUIItem { get => _effectUIItem; }
    }
}
