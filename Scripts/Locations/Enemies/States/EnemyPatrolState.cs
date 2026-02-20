using Assets.Game.Scripts.Common.Extensions;
using Assets.Game.Scripts.Common.UniversalStateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Game.Scripts.Locations.Enemies.States
{
    public class EnemyPatrolState : State
    {
        private EnemyStateMachineData _data;
        private EnemyController _controller;

        private Vector3 _currentPatrolPoint;
        private float _waitTime;
        private bool _isWaiting;
        private float _timer;

        private float _inactivityCheckTimer;
        private float _inactivityCheckInterval = 2f;

        public EnemyPatrolState(IStateSwitcher stateSwitcher, EnemyController controller, EnemyStateMachineData data) : base(stateSwitcher)
        {
            _data = data;
            _controller = controller;
        }

        public override void Enter()
        {
            _data.NavMeshAgent.speed = _data.PatrolSpeed;
            _data.NavMeshAgent.acceleration = _data.PatrolAcceleration;

            _data.NavMeshAgent.isStopped = false;

            SetNewPatrolPoint();
            _isWaiting = false;
        }

        public override void Exit()
        {
            if (_data.NavMeshAgent != null && _data.NavMeshAgent.isActiveAndEnabled && _data.NavMeshAgent.isOnNavMesh)
                _data.NavMeshAgent.isStopped = true;
        }

        public override void Update()
        {
            if (Vector3.Distance(_data.PlayerController.transform.position, _data.NavMeshAgent.transform.position) <= _data.VisionRange &&
                Vector3.Distance(_data.OriginalPosition, _data.NavMeshAgent.transform.position) <= _data.PatrolRange)
            {
                _controller.PlayFeedback();

                StateSwitcher.SwitchState<EnemyAlertState>();

                return;
            }

            _inactivityCheckTimer += Time.deltaTime;
            if (_inactivityCheckTimer >= _inactivityCheckInterval)
            {
                _inactivityCheckTimer = 0f;

                if (ShouldDeactivate())
                {
                    StateSwitcher.SwitchState<EnemyInactiveState>();
                    return;
                }
            }

            if (_isWaiting)
            {
                Wait();
            }
            else
            {
                Patrol();
            }

            _controller.SetPatrolAnimation(_data.NavMeshAgent.GetMovementDirection());
        }


        private bool ShouldDeactivate()
        {
            float distanceToPlayer = Vector3.Distance(_data.PlayerController.transform.position, _data.NavMeshAgent.transform.position);
            return distanceToPlayer > _data.ActivationRange;
        }

        private void Patrol()
        {
            if (_data.NavMeshAgent.remainingDistance <= _data.NavMeshAgent.stoppingDistance)
            {
                StartWaiting();
            }
        }

        private void StartWaiting()
        {
            _isWaiting = true;
            _timer = 0f;
            _waitTime = Random.Range(_data.MinWaitingTime, _data.MaxWaitingTime);

            _data.NavMeshAgent.isStopped = true;
        }

        private void Wait()
        {
            _timer += Time.deltaTime;

            if (_timer >= _waitTime)
            {
                _isWaiting = false;
                _data.NavMeshAgent.isStopped = false;
                SetNewPatrolPoint();
            }
        }

        private void SetNewPatrolPoint()
        {
            Vector3 randomPoint = _data.OriginalPosition + Random.insideUnitSphere * _data.PatrolRange;
            randomPoint.y = _data.OriginalPosition.y;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, _data.PatrolRange, NavMesh.AllAreas))
            {
                _currentPatrolPoint = hit.position;
                _data.NavMeshAgent.SetDestination(_currentPatrolPoint);
            }
            else
            {
                _currentPatrolPoint = _data.OriginalPosition;
                _data.NavMeshAgent.SetDestination(_currentPatrolPoint);
            }
        }
    }
}