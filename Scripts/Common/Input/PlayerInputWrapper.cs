using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Assets.Game.Scripts.Common.Input
{
    public class PlayerInputWrapper : IPlayerInputWrapper, IInitializable, IDisposable
    {
        private PlayerInput _input;
        private InputActionMap _lastActiveGameMap;

        public event Action OnUICanceled;
        public event Action OnUISubmited;

        public PlayerInput Input => _input;

        public void Initialize()
        {
            _input = new PlayerInput();
            _lastActiveGameMap = _input.UI;

            _input.UI.Cancel.performed += OnUICanceledInvoker;
            _input.UI.Submit.performed += OnUISubmitedInvoker;

            DisableAllMaps();
        }

        private void OnUISubmitedInvoker(InputAction.CallbackContext context)
        {
            OnUISubmited?.Invoke();
        }

        private void OnUICanceledInvoker(InputAction.CallbackContext context)
        {
            OnUICanceled?.Invoke();
        }

        //public void EnableLocationMap()
        //{
        //    _input.Battle.Disable();
        //    _input.UI.Disable();

        //    _input.Location.Enable();
        //    _lastActiveGameMap = _input.Location;
        //}

        //public void EnableBattleMap()
        //{
        //    _input.Location.Disable();
        //    _input.UI.Disable();

        //    _input.Battle.Enable();
        //    _lastActiveGameMap = _input.Battle;
        //}

        //public void EnableUIMap()
        //{
        //    _input.Location.Disable();
        //    _input.Battle.Disable();

        //    _input.UI.Enable();
        //    _lastActiveGameMap = _input.UI;
        //}

        public void DisableUIMap() => _input.UI.Disable();
        public void DisableLocationMap() => _input.Location.Disable();
        public void DisableBattleMap() => _input.Battle.Disable();

        public void EnableUIMap() => _input.UI.Enable();
        public void EnableLocationMap() => _input.Location.Enable();
        public void EnableBattleMap() => _input.Battle.Enable();

        public void EnableLastActiveGameMap()
        {
            if (_lastActiveGameMap != null)
            {
                _lastActiveGameMap.Enable();
            }
        }

        public void DisableAllMaps()
        {
            _input.Location.Disable();
            _input.Battle.Disable();
            _input.UI.Disable();
        }

        public void Dispose()
        {
            _input?.Dispose();
        }
    }
}