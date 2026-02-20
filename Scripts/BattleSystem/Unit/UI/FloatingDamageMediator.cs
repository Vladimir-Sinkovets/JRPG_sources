using MoreMountains.Feedbacks;
using UnityEngine;

namespace Assets.Game.Scripts.BattleSystem.Unit.UI
{
    public class FloatingDamageMediator : MonoBehaviour
    {
        [SerializeField] private MMF_Player _feedback;
        [SerializeField] private BattleUnit _unit;

        // todo: Add floating text for effects and for hp increase

        public void Init(BattleUnit unit)
        {
            _unit = unit;

            _unit.Stats.OnHpDecrease += OnDamagedHandler;
        }

        private void OnDestroy() => _unit.Stats.OnHpDecrease -= OnDamagedHandler;

        private void OnDamagedHandler(UnitStats unit, int damage)
        {
            _feedback.PlayFeedbacks(transform.position, damage);
        }
    }
}
