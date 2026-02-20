using UnityEngine;

namespace Assets.Game.Scripts.BattleSystem.Services.CameraFocusServices
{
    public interface ICameraFocusService
    {
        void SetTarget(Transform transform);
        void ResetTarget();
        void SetDistance(BattleCameraDistance distance);
    }
}