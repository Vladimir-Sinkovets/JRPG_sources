using Assets.Game.Scripts.Common.Input;
using System;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Assets.Game.Scripts.Common.UI.Panels
{
    public class PanelBase : MonoBehaviour
    {
        public UnityEvent OnShowed;
        public UnityEvent OnHided;

        public event Action<PanelBase> OnPanelClosed;

        [SerializeField] private bool _useFirstSelected = true;
        [ShowIf(nameof(_useFirstSelected)), SerializeField] private GameObject _firstSelected;
        [SerializeField] private CanvasGroup _panel;
        [Space]
        [SerializeField] private bool _hidable = true;
        [SerializeField] private bool _useAnimations = false;
        [ShowIf(nameof(_useAnimations)), SerializeField] private PanelAnimation _panelAnimation;

        public GameObject LastSelected { get; set; }
        public CanvasGroup CanvasGroup { get => _panel; }

        [Inject] private IUIInputController _UIInputController;

        public void Close()
        {
            if (_hidable == false)
                return;

            Hide();

            UnSelect();

            OnPanelClosed?.Invoke(this);

            LastSelected = null;
        }

        public virtual void Show()
        {
            if (_useAnimations)
            {
                _panelAnimation.Show(this, () => InvokeOnShowed());

                return;
            }
            _panel.gameObject.SetActive(true);

            InvokeOnShowed();
        }

        public virtual void Hide()
        {
            if (_useAnimations)
            {
                _panelAnimation.Hide(this, () => InvokeOnHided());

                return;
            }
            _panel.gameObject.SetActive(false);

            InvokeOnHided();
        }

        protected void InvokeOnShowed() => OnShowed?.Invoke();
        protected void InvokeOnHided() => OnHided?.Invoke();

        public virtual void Select()
        {
            _UIInputController.OnCancel += OnCancelHandler;

            if (LastSelected != null && LastSelected.GetComponent<Selectable>().enabled)
                EventSystem.current.SetSelectedGameObject(LastSelected);
            else
            {
                if (_useFirstSelected)
                    EventSystem.current.SetSelectedGameObject(_firstSelected);
            }
        }

        private void OnCancelHandler() => Close();

        public virtual void UnSelect()
        {
            _UIInputController.OnCancel -= OnCancelHandler;
        }
    }
}
