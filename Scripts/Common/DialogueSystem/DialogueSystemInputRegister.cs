using Assets.Game.Scripts.Common.Input;
using PixelCrushers;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class DialogueSystemInputRegister : MonoBehaviour
{
    protected static bool isRegistered = false;
    private bool didIRegister = false;

    [Inject] private IPlayerInputWrapper _playerInputWrapper;

    void Awake()
    {
        //controls = new MyControls();
    }
    void OnEnable()
    {
        if (!isRegistered)
        {
            StartCoroutine(Register());
        }
    }

    private IEnumerator Register()
    {
        yield return null;

        isRegistered = true;
        didIRegister = true;
        //_controls.Enable();
        //InputDeviceManager.RegisterInputAction("Cancel", _playerInputWrapper.Input.UI.Cancel);
        InputDeviceManager.RegisterInputAction("Interact", _playerInputWrapper.Input.UI.Submit);
        //InputDeviceManager.RegisterInputAction("Navigate", _playerInputWrapper.Input.UI.Navigate);
    }

    void OnDisable()
    {
        if (didIRegister)
        {
            isRegistered = false;
            didIRegister = false;
            //controls.Disable();
            //InputDeviceManager.UnregisterInputAction("Cancel");
            InputDeviceManager.UnregisterInputAction("Interact");
            //InputDeviceManager.UnregisterInputAction("Navigate");
        }
    }

}
