using Assets.Game.Scripts.Common.UniversalStateMachine;
using UnityEngine;

namespace Assets.Game.Scripts.Locations.Enemies.States
{
    public class EnemyStartState : State
    {
        private EnemyStateMachineData _data;
        private EnemyController _controller;

        public EnemyStartState(IStateSwitcher stateSwitcher, EnemyController controller, EnemyStateMachineData data) : base(stateSwitcher)
        {
            _data = data;
            _controller = controller;
        }

        public override void Enter()
        {
            if (Vector3.Distance(_data.PlayerController.transform.position, _data.NavMeshAgent.transform.position) <= _data.VisionRange &&
                Vector3.Distance(_data.OriginalPosition, _data.NavMeshAgent.transform.position) <= _data.PatrolRange)
            {
                _controller.PlayFeedback();

                StateSwitcher.SwitchState<EnemyAlertState>();

                return;
            }

            StateSwitcher.SwitchState<EnemyPatrolState>();
        }

        public override void Exit() { }

        public override void Update() { }
    }
}