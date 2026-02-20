using Assets.Game.Scripts.Common.Input;
using System;
using TriInspector;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Common.Services.StartInputSelectors
{
    public class StartInputSelector : MonoBehaviour
    {
        [SerializeField, EnumToggleButtons] private InputType _selectedInput = InputType.Location;

        [Inject] private IPlayerInputWrapper _input;

        private void Start()
        {
            _input.DisableAllMaps();

            switch (_selectedInput)
            {
                case InputType.None:
                    _input.DisableAllMaps();
                    break;
                case InputType.Battle:
                    _input.EnableBattleMap();
                    break;
                case InputType.Location:
                    _input.EnableLocationMap();
                    break;
                case InputType.UI:
                    _input.EnableUIMap();
                    break;
                default:
                    break;
            }
        }

        private enum InputType
        {
            None,
            Battle,
            Location,
            UI,
        }
    }
}
