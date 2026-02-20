using UnityEngine;
using Assets.Game.Scripts.Common.UniversalStateMachine;

namespace Assets.Game.Scripts.Locations.Enemies.States
{
    public class EnemyAlertState : State
    {
        private EnemyStateMachineData _data;
        private EnemyController _controller;

        private float _alertTimer;

        public EnemyAlertState(IStateSwitcher stateSwitcher, EnemyController controller, EnemyStateMachineData data) : base(stateSwitcher)
        {
            _data = data;
            _controller = controller;
        }

        public override void Enter()
        {
            _alertTimer = 0f;

            if (_data.NavMeshAgent != null && _data.NavMeshAgent.isActiveAndEnabled)
            {
                _data.NavMeshAgent.isStopped = true;
            }

            _controller.PlayFeedback();
        }

        public override void Exit() { }

        public override void Update()
        {
            _alertTimer += Time.deltaTime;

            bool canSeePlayer = Vector3.Distance(_data.PlayerController.transform.position, _data.NavMeshAgent.transform.position) <= _data.VisionRange &&
                               Vector3.Distance(_data.OriginalPosition, _data.NavMeshAgent.transform.position) <= _data.PatrolRange;

            if (!canSeePlayer)
            {
                StateSwitcher.SwitchState<EnemyPatrolState>();
                return;
            }

            if (_alertTimer >= _data.AlertDelay)
            {
                StateSwitcher.SwitchState<EnemyChasingState>();
            }
        }
    }
}