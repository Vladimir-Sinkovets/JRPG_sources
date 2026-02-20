using UnityEngine;
using Assets.Game.Scripts.Common.UniversalStateMachine;
using System;
using Assets.Game.Scripts.Common.Extensions;

namespace Assets.Game.Scripts.Locations.Enemies.States
{
    public class EnemyChasingState : State
    {
        private EnemyStateMachineData _data;
        private EnemyController _controller;

        public EnemyChasingState(IStateSwitcher stateSwitcher, EnemyController controller, EnemyStateMachineData data) : base(stateSwitcher)
        {
            _data = data;
            _controller = controller;
        }

        public override void Enter()
        {
            _data.NavMeshAgent.speed = _data.ChasingSpeed;
            _data.NavMeshAgent.acceleration = _data.ChasingAcceleration;

            _data.NavMeshAgent.isStopped = false;
        }

        public override void Exit()
        {
            if (_data.NavMeshAgent != null && _data.NavMeshAgent.isActiveAndEnabled && _data.NavMeshAgent.isOnNavMesh)
                _data.NavMeshAgent.isStopped = true;
        }

        public override void Update()
        {
            if (_data.PlayerController == null)
            {
                StateSwitcher.SwitchState<EnemyPatrolState>();
                return;
            }

            float distanceToOriginal = Vector3.Distance(_data.OriginalPosition, _data.NavMeshAgent.transform.position);

            if (distanceToOriginal > _data.ChasingRange)
            {
                StateSwitcher.SwitchState<EnemyPatrolState>();
                return;
            }

            float distanceToPlayer = Vector3.Distance(_data.NavMeshAgent.transform.position, _data.PlayerController.transform.position);

            if (distanceToPlayer <= _data.NavMeshAgent.stoppingDistance)
            {
                _data.OnCollision?.Invoke();

                return;
            }

            _data.NavMeshAgent.SetDestination(_data.PlayerController.transform.position);

            _controller.SetChasingAnimation(_data.NavMeshAgent.GetMovementDirection());
        }
    }
}