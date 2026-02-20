using Assets.Game.Scripts.Common.Input;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Zenject;

namespace Assets.Game.Scripts.Locations.Services.Input
{
    public class LocationInputController : ILocationInputController, IInitializable, IDisposable
    {
        public event Action<Vector2> OnMoving;
        public event Action OnAct;
        public event Action<bool> OnSpeedUp;
        public event Action OnPause;

        [Inject] private IPlayerInputWrapper _inputWrapper;

        public void DisableMap() => _inputWrapper.DisableLocationMap();
        public void EnableMap() => _inputWrapper.EnableLocationMap();

        public void Initialize()
        {
            _inputWrapper.Input.Location.Moving.performed += OnMovingHandler;
            _inputWrapper.Input.Location.Moving.canceled += OnMovingHandler;

            _inputWrapper.Input.Location.Act.performed += OnActHandler;

            _inputWrapper.Input.Location.SpeedUp.performed += OnSpeedUpHandler;
            _inputWrapper.Input.Location.SpeedUp.canceled += OnSpeedUpCanceledHandler;

            _inputWrapper.Input.Location.Pause.performed += OnPausehandler;
        }

        private void OnPausehandler(InputAction.CallbackContext context) => OnPause?.Invoke();

        public void Dispose()
        {
            _inputWrapper.Input.Location.Moving.performed -= OnMovingHandler;
            _inputWrapper.Input.Location.Moving.canceled -= OnMovingHandler;

            _inputWrapper.Input.Location.Act.performed -= OnActHandler;

            _inputWrapper.Input.Location.SpeedUp.performed -= OnSpeedUpHandler;
            _inputWrapper.Input.Location.SpeedUp.canceled -= OnSpeedUpCanceledHandler;
        }

        public void OnMovingHandler(InputAction.CallbackContext context) => OnMoving?.Invoke(context.ReadValue<Vector2>());

        public void OnActHandler(InputAction.CallbackContext context) => OnAct?.Invoke();

        public void OnSpeedUpHandler(InputAction.CallbackContext context) => OnSpeedUp?.Invoke(true);
        public void OnSpeedUpCanceledHandler(InputAction.CallbackContext context) => OnSpeedUp?.Invoke(false);
    }
}
