using Assets.Game.Scripts.Locations.Services.Input;

namespace Assets.Game.Scripts.Locations.Player.PlayerStateMachine.States.Moving
{
    public class PlayerMovingLeftState : PlayerMovingState
    {
        public PlayerMovingLeftState(PlayerController player, ILocationInputController input) : base(player, input) { }

        public override void Enter()
        {
            base.Enter();

            _view.WalkingLeft();
        }

        protected override void SwitchToIdleState()
        {
            base.SwitchToIdleState();

            StateSwitcher.SwitchState<PlayerIdleLeftState>();
        }
    }
}
