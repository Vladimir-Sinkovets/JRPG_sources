using Assets.Game.Scripts.Locations.Services.Input;

namespace Assets.Game.Scripts.Locations.Player.PlayerStateMachine.States.Moving
{
    public class PlayerMovingForwardState : PlayerMovingState
    {
        public PlayerMovingForwardState(PlayerController player, ILocationInputController input) : base(player, input) { }

        public override void Enter()
        {
            base.Enter();

            _view.WalkingForward();
        }

        protected override void SwitchToIdleState()
        {
            base.SwitchToIdleState();

            StateSwitcher.SwitchState<PlayerIdleForwardState>();
        }
    }
}
