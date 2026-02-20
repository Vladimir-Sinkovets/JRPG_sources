using Assets.Game.Scripts.BattleSystem.Unit;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Game.Scripts.BattleSystem.Services.BattlePositions
{
    public class BattlePositionsData : MonoBehaviour
    {
        [SerializeField] private Transform middlePosition;
        [SerializeField] private Transform[] _leftPositions;
        [SerializeField] private Transform[] _rightPositions;

        private List<PositionData> _playerTeamPositionData;
        private List<PositionData> _enemyTeamPositionData;

        private void Awake()
        {
            _playerTeamPositionData = new();
            
            foreach (var position in _leftPositions)
            {
                _playerTeamPositionData.Add(new()
                {
                    Holder = null,
                    Transform = position,
                });
            }
            
            _enemyTeamPositionData = new();

            foreach (var position in _rightPositions)
            {
                _enemyTeamPositionData.Add(new()
                {
                    Holder = null,
                    Transform = position,
                });
            }
        }

        public IEnumerable<Transform> PlayerTeamFreePositions { get => _playerTeamPositionData.Where(x => x.Holder == null).Select(x => x.Transform); }
        public IEnumerable<Transform> EnemyTeamFreePositions { get => _enemyTeamPositionData.Where(x => x.Holder == null).Select(x => x.Transform); }

        public void SetPositionHolder(Transform transform, BattleUnit unit)
        {
            PositionData data;

            if ((data = _enemyTeamPositionData.FirstOrDefault(x => x.Transform == transform)) != null)
            {
                if (data.Holder != null)
                    Debug.LogWarning($"Position {data.Transform} already has a holder {data.Holder}");

                data.Holder = unit;
            }
            else if ((data = _playerTeamPositionData.FirstOrDefault(x => x.Transform == transform)) != null)
            {
                if (data.Holder != null)
                    Debug.LogWarning($"Position {data.Transform} already has a holder {data.Holder}");

                data.Holder = unit;
            }
            else
            {
                Debug.LogWarning($"There is no {transform} position in the list");
            }
        }

        public void RemovePositionHolder(Transform transform)
        {
            PositionData data;

            if ((data = _enemyTeamPositionData.FirstOrDefault(x => x.Transform == transform)) != null)
            {
                data.Holder = null;
            }
            else if ((data = _playerTeamPositionData.FirstOrDefault(x => x.Transform == transform)) != null)
            {
                data.Holder = null;
            }
        }

        public void RemovePositionHolder(BattleUnit unit)
        {
            PositionData data;

            if ((data = _enemyTeamPositionData.FirstOrDefault(x => x.Holder == unit)) != null)
            {
                data.Holder = null;
            }
            else if ((data = _playerTeamPositionData.FirstOrDefault(x => x.Holder == unit)) != null)
            {
                data.Holder = null;
            }
        }

        public Transform MiddlePosition { get => middlePosition; }

        private class PositionData
        {
            public BattleUnit Holder;
            public Transform Transform;
        }
    }
}
