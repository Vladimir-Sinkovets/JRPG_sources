using System;
using UnityEngine;

namespace Assets.Game.Scripts.BattleSystem.Services.CameraFocusServices
{
    public class CameraFocusService : MonoBehaviour, ICameraFocusService
    {
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _offset;
        [Space]
        [SerializeField] private Transform _primaryTarget;

        private Vector3 _originalTargetPosition;

        private Transform _target;

        private void Awake()
        {
            _originalTargetPosition = _primaryTarget.position;
        }

        private void Update()
        {
            if (_target != null)
            {
                _primaryTarget.position = Vector3.Lerp(_originalTargetPosition, _target.position, _offset);
            }
        }

        public void ResetTarget()
        {
            _target = null;

            _primaryTarget.position = _originalTargetPosition;
        }

        public void SetTarget(Transform transform) => _target = transform;

        public void SetDistance(BattleCameraDistance distance)
        {

        }
    }
}
