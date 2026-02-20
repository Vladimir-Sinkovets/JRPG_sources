using UnityEngine;
using Assets.Game.Scripts.Common.UniversalStateMachine;
using MoreMountains.Feedbacks;

namespace Assets.Game.Scripts.Locations.Enemies.States
{
    public class EnemyInactiveState : State
    {
        private EnemyStateMachineData _data;
        private EnemyController _controller;

        private float _checkInterval = 1f;
        private float _checkTimer;

        public EnemyInactiveState(IStateSwitcher stateSwitcher, EnemyController controller, EnemyStateMachineData data) : base(stateSwitcher)
        {
            _data = data;
            _controller = controller;
        }

        public override void Enter()
        {
            _checkTimer = 0f;

            if (_data.NavMeshAgent != null && _data.NavMeshAgent.isActiveAndEnabled)
            {
                _data.NavMeshAgent.isStopped = true;
                _data.NavMeshAgent.enabled = false;
            }

            _controller.Deactivate();
        }

        public override void Exit()
        {
            if (_data.NavMeshAgent != null)
            {
                _data.NavMeshAgent.enabled = true;
            }

            _controller.Activate();
        }

        public override void Update()
        {
            _checkTimer += Time.deltaTime;
            if (_checkTimer < _checkInterval) return;

            _checkTimer = 0f;

            float distanceToPlayer = Vector3.Distance(_data.PlayerController.transform.position, _data.NavMeshAgent.transform.position);

            if (distanceToPlayer <= _data.ActivationRange)
            {
                StateSwitcher.SwitchState<EnemyPatrolState>();
            }
        }
    }
}