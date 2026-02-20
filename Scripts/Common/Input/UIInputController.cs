using System;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

namespace Assets.Game.Scripts.Common.Input
{
    public class UIInputController : IUIInputController, IInitializable, IDisposable
    {
        public event Action OnCancel;

        [Inject] private IPlayerInputWrapper _inputWrapper;

        public void DisableMap()
        {
            _inputWrapper.DisableUIMap();
        }

        public void EnableMap()
        {
            _inputWrapper.EnableUIMap();
        }

        public void Initialize()
        {
            _inputWrapper.Input.UI.Cancel.performed += OnUICancelHandler;
        }

        private void OnUICancelHandler(InputAction.CallbackContext context) => OnCancel?.Invoke();

        public void Dispose()
        {
            _inputWrapper.Input.UI.Cancel.performed -= OnUICancelHandler;
        }
    }
}
