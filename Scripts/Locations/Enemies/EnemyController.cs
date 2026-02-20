using Assets.Game.Scripts.Common.UniversalStateMachine;
using Assets.Game.Scripts.Locations.Enemies.States;
using Assets.Game.Scripts.Locations.Player;
using MoreMountains.Feedbacks;
using TriInspector;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Zenject;

namespace Assets.Game.Scripts.Locations.Enemies
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [Space]
        [SerializeField, OnValueChanged(nameof(SetDataValues))] private float _speed;

        [Title("Patrol settings")]
        [SerializeField, OnValueChanged(nameof(SetDataValues))] private float _visionRange;
        [SerializeField, OnValueChanged(nameof(SetDataValues))] private float _patrolRange;
        [SerializeField, OnValueChanged(nameof(SetDataValues))] private float _patrolAccelleration;
        [SerializeField, OnValueChanged(nameof(SetDataValues))] private float _patrolSpeed;
        [SerializeField, OnValueChanged(nameof(SetDataValues))] private float _minWatingTime;
        [SerializeField, OnValueChanged(nameof(SetDataValues))] private float _maxWatingTime;
        [SerializeField, OnValueChanged(nameof(SetDataValues))] private float _alertDelay;
        [SerializeField, OnValueChanged(nameof(SetDataValues))] private float _activationRange;
        [SerializeField, OnValueChanged(nameof(SetDataValues))] private float _patrolAnimationSpeed = 1.0f;
        [SerializeField] private MMF_Player _attention;

        [Title("Chasing settings")]
        [SerializeField, OnValueChanged(nameof(SetDataValues))] private float _chasingRange;
        [SerializeField, OnValueChanged(nameof(SetDataValues))] private float _stoppingDistance = 0.3f;
        [SerializeField, OnValueChanged(nameof(SetDataValues))] private float _chasingAcceleration;
        [SerializeField, OnValueChanged(nameof(SetDataValues))] private float _chasingSpeed;
        [SerializeField, OnValueChanged(nameof(SetDataValues))] private float _chasingAnimationSpeed = 1.0f;


        [Title("Animation")]
        [SerializeField] private Animator _animator;

        [SerializeField] private string _rightChasingAnimationName = "Right";
        [SerializeField] private string _leftChasingAnimationName = "Left";
        [SerializeField] private string _forwardChasingAnimationName = "Forward";
        [SerializeField] private string _backwardChasingAnimationName = "Backward";
        [Space]
        [SerializeField] private string _rightPatrolAnimationName = "Right";
        [SerializeField] private string _leftPatrolAnimationName = "Left";
        [SerializeField] private string _forwardPatrolAnimationName = "Forward";
        [SerializeField] private string _backwardPatrolAnimationName = "Backward";
        [Space]
        [SerializeField] private string _idleAnimationName = "Idle";

        [Title("Events")]
        [SerializeField] private UnityEvent OnCollision;
        [SerializeField] private UnityEvent OnActivated;
        [SerializeField] private UnityEvent OnDeactivated;

        [Inject] private PlayerController _player;

        private EnemyStateMachineData _data;
        private StateMachine _stateMachine;

        private bool _isDead = false;

        [SerializeField] private Vector3 _originalPosition;

        public void PlayFeedback()
        {
            if (_attention != null)
                _attention.PlayFeedbacks();
        }

        public void Activate() => OnActivated?.Invoke();
        public void Deactivate() => OnDeactivated?.Invoke();

        public void Die()
        {
            _isDead = true;
        }

        private void SetDataValues()
        {
            if (!Application.isPlaying)
                return;

            _data.VisionRange = _visionRange;
            _data.PatrolRange = _patrolRange;
            _data.PatrolAcceleration = _patrolAccelleration;
            _data.PatrolSpeed = _patrolSpeed;
            _data.MinWaitingTime = _minWatingTime;
            _data.MaxWaitingTime = _maxWatingTime;
            _data.ChasingRange = _chasingRange;
            _data.ChasingAcceleration = _chasingAcceleration;
            _data.ChasingSpeed = _chasingSpeed;
            _data.AlertDelay = _alertDelay;
            _data.ActivationRange = _activationRange;
            _data.StoppingDistance = _stoppingDistance;
        }

        private void Start()
        {
            if (_isDead)
            {
                gameObject.SetActive(false);
                
                return;
            }

            _agent.updateRotation = false;

            _data = new EnemyStateMachineData
            {
                NavMeshAgent = _agent,
                PlayerController = _player,
                OriginalPosition = _originalPosition,
                OnCollision = OnСollisionHandler,
            };

            SetDataValues();

            _stateMachine = new StateMachine();

            _stateMachine.AddState(new EnemyPatrolState(_stateMachine, this, _data));
            _stateMachine.AddState(new EnemyChasingState(_stateMachine, this, _data));
            _stateMachine.AddState(new EnemyAlertState(_stateMachine, this, _data));
            _stateMachine.AddState(new EnemyInactiveState(_stateMachine, this, _data));
            _stateMachine.AddState(new EnemyStartState(_stateMachine, this, _data));

            _stateMachine.SetStartState<EnemyStartState>();
        }

        private void OnСollisionHandler()
        {
            OnCollision.Invoke();
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        private void OnDestroy()
        {
            _stateMachine?.Dispose();
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                _originalPosition = transform.position;

            Gizmos.color = Color.darkRed;
            Gizmos.DrawWireSphere(transform.position, _visionRange);
            
            if (Application.isPlaying)
            {
                if (_data == null) return;

                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(_data.OriginalPosition, _patrolRange);

                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_data.OriginalPosition, _chasingRange);
            }
            else
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, _patrolRange);

                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, _chasingRange);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, _activationRange);
        }

        public void SetPatrolAnimation(Vector3 direction)
        {
            if (_animator != null)
            {
                if (direction == Vector3.zero)
                {
                    PlayAnimation(_idleAnimationName);
                    _animator.speed = 1.0f;

                    return;
                }

                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
                {
                    if (direction.x > 0)
                        PlayAnimation(_rightPatrolAnimationName);
                    else
                        PlayAnimation(_leftPatrolAnimationName);
                }
                else
                {
                    if (direction.z > 0)
                        PlayAnimation(_forwardPatrolAnimationName);
                    else
                        PlayAnimation(_backwardPatrolAnimationName);
                }
                PlayAnimation(_leftPatrolAnimationName);

                _animator.speed = _patrolAnimationSpeed;
            }
        }
        public void SetChasingAnimation(Vector3 direction)
        {
            if (_animator != null)
            {
                if (direction == Vector3.zero)
                {
                    PlayAnimation(_idleAnimationName);
                    _animator.speed = 1.0f;

                    return;
                }

                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
                {
                    if (direction.x > 0)
                        PlayAnimation(_rightChasingAnimationName);
                    else
                        PlayAnimation(_leftChasingAnimationName);
                }
                else
                {
                    if (direction.z > 0)
                        PlayAnimation(_forwardChasingAnimationName);
                    else
                        PlayAnimation(_backwardChasingAnimationName);
                }
                PlayAnimation(_leftChasingAnimationName);

                _animator.speed = _chasingAnimationSpeed;
            }
        }

        public void PlayAnimation(string animationName)
        {
            if (!HasAnimation(animationName))
                return;

            _animator.Play(animationName);
        }

        private bool HasAnimation(string animationName)
        {
            if (_animator == null || _animator.runtimeAnimatorController == null)
                return false;

            foreach (var clip in _animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == animationName)
                    return true;
            }

            return false;
        }
    }
}