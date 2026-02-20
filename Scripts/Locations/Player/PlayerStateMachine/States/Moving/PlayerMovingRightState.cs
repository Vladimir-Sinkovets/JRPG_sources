using Assets.Game.Scripts.Locations.Services.Input;

namespace Assets.Game.Scripts.Locations.Player.PlayerStateMachine.States.Moving
{
    public class PlayerMovingRightState : PlayerMovingState
    {
        public PlayerMovingRightState(PlayerController player, ILocationInputController input) : base(player, input) { }

        public override void Enter()
        {
            base.Enter();

            _view.WalkingRight();
        }

        protected override void SwitchToIdleState()
        {
            base.SwitchToIdleState();

            StateSwitcher.SwitchState<PlayerIdleRightState>();
        }
    }
}
