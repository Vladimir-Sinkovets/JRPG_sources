using System;

namespace Assets.Game.Scripts.Common.Input
{
    public interface IPlayerInputWrapper
    {
        event Action OnUICanceled;
        event Action OnUISubmited;

        PlayerInput Input { get; }

        void DisableAllMaps();
        void DisableBattleMap();
        void DisableLocationMap();
        void DisableUIMap();
        void EnableBattleMap();
        void EnableLocationMap();
        void EnableUIMap();
    }
}