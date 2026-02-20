using System;

namespace Assets.Game.Scripts.Common.Input
{
    public interface IUIInputController
    {
        event Action OnCancel;

        void DisableMap();
        void EnableMap();
    }
}