using Assets.Game.Scripts.Common.UniversalStateMachine;
using Assets.Game.Scripts.Locations.Services.Input;
using UnityEngine;

namespace Assets.Game.Scripts.Locations.Player.PlayerStateMachine.States.Moving
{
    public abstract class PlayerMovingState : State
    {
        protected readonly ILocationInputController _input;
        protected readonly CharacterController _playerController;
        protected readonly PlayerMovementConfig _movementConfig;
        protected readonly PlayerStateMachineData _data;
        protected readonly PlayerView _view;

        public PlayerMovingState(PlayerController player, ILocationInputController input) : base(player.StateMachine)
        {
            _playerController = player.CharacterController;
            _movementConfig = player.MovementConfig;
            _data = player.StateMachineData;
            _view = player.View;

            _input = input;
        }

        public override void Enter()
        {
            _input.OnMoving += OnMoving;
            _input.OnSpeedUp += OnSpeedUp;
        }

        private void OnSpeedUp(bool isSpeedUp)
        {
            if (isSpeedUp)
                _data.Speed = _movementConfig.speedUp;
            else
                _data.Speed = _movementConfig.speed;
        }

        public override void Exit()
        {
            _input.OnMoving -= OnMoving;
            _input.OnSpeedUp -= OnSpeedUp;
        }

        public override void Update() => MoveCharacter();

        private void MoveCharacter()
        {
            var vector = new Vector3(_data.MovingDirection.x, 0, _data.MovingDirection.y).normalized * _data.Speed;

            if (_playerController.isGrounded == false)
                _data.VerticalVelocity += _movementConfig.gravity * Time.deltaTime;
            else
                _data.VerticalVelocity = -0.1f;


            vector.y = _data.VerticalVelocity;

            var moveVector = Time.deltaTime * vector;

            _playerController.Move(moveVector);
        }

        protected void OnMoving(Vector2 direction)
        {
            _data.MovingDirection = direction;

            if (direction.x > 0)
            {
                StateSwitcher.SwitchState<PlayerMovingRightState>();
                return;
            }
            else if (direction.x < 0)
            {
                StateSwitcher.SwitchState<PlayerMovingLeftState>();
                return;
            }
            else if (direction.y > 0)
            {
                StateSwitcher.SwitchState<PlayerMovingForwardState>();
                return;
            }
            else if (direction.y < 0)
            {
                StateSwitcher.SwitchState<PlayerMovingBackwardState>();
                return;
            }
            else
            {
                SwitchToIdleState();
                return;
            }
        }
        protected virtual void SwitchToIdleState() { }
    }
}