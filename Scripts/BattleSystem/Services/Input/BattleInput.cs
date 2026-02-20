using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Game.Scripts.BattleSystem.Services.Input
{
    public class BattleInput : IBattleInput, IDisposable
    {
        public event Action<Vector2> OnPointerDown;

        private Vector2 _pointerPosition;

        private readonly PlayerInput _input;

        public BattleInput()
        {
            _input ??= new PlayerInput();

            //_input.Battle.PointerDown.performed += OnPointerDownHandler;
            //_input.Battle.PointerPosition.performed += OnPointerPositionHandler;

            _input.Enable();
        }

        public void Dispose()
        {
            //_input.Battle.PointerDown.performed -= OnPointerDownHandler;
            //_input.Battle.PointerPosition.performed -= OnPointerPositionHandler;
        }

        public void OnPointerDownHandler(InputAction.CallbackContext context)
        {
            OnPointerDown?.Invoke(_pointerPosition);
        }

        public void OnPointerPositionHandler(InputAction.CallbackContext context)
        {
            _pointerPosition = context.ReadValue<Vector2>();
        }
    }
}
