using Assets.Game.Scripts.Common.UniversalStateMachine;
using Assets.Game.Scripts.Locations.Player.PlayerStateMachine;
using Assets.Game.Scripts.Locations.Player.PlayerStateMachine.States.Moving;
using Assets.Game.Scripts.Locations.Services.Input;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Locations.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private PlayerMovementConfig _movementConfig;
        [SerializeField] private PlayerView _view;

        private StateMachine _stateMachine;
        private PlayerStateMachineData _stateMachineData;

        private LocationInputController _input;

        #region Getters
        public PlayerStateMachineData StateMachineData { get => _stateMachineData; }
        public StateMachine StateMachine { get => _stateMachine; }
        public CharacterController CharacterController { get => _characterController; }
        public PlayerMovementConfig MovementConfig { get => _movementConfig; }
        public PlayerView View { get => _view; }
        #endregion

        [Inject]
        private void Construct(LocationInputController input)
        {
            _input = input;
        }

        public void Start()
        {
            _stateMachine = new StateMachine();

            _stateMachineData = new PlayerStateMachineData();

            _stateMachineData.Speed = _movementConfig.speed;

            _stateMachine.AddState(new PlayerMovingLeftState(this, _input));
            _stateMachine.AddState(new PlayerMovingRightState(this, _input));
            _stateMachine.AddState(new PlayerMovingForwardState(this, _input));
            _stateMachine.AddState(new PlayerMovingBackwardState(this, _input));
            _stateMachine.AddState(new PlayerIdleLeftState(this, _input));
            _stateMachine.AddState(new PlayerIdleRightState(this, _input));
            _stateMachine.AddState(new PlayerIdleForwardState(this, _input));
            _stateMachine.AddState(new PlayerIdleBackwardState(this, _input));

            _stateMachine.SetStartState<PlayerIdleBackwardState>();
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        private void OnDestroy()
        {
            _stateMachine.Dispose();
        }
    }
}