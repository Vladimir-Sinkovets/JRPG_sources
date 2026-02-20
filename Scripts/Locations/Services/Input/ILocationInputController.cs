using System;
using UnityEngine;

namespace Assets.Game.Scripts.Locations.Services.Input
{
    public interface ILocationInputController
    {
        public event Action<Vector2> OnMoving;
        public event Action OnAct;
        public event Action<bool> OnSpeedUp;
        public event Action OnPause;

        void DisableMap();
        void EnableMap();
    }
}
