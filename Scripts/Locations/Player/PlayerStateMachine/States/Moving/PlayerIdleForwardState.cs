using Assets.Game.Scripts.Locations.Services.Input;

namespace Assets.Game.Scripts.Locations.Player.PlayerStateMachine.States.Moving
{
    public class PlayerIdleForwardState : PlayerMovingState
    {
        public PlayerIdleForwardState(PlayerController player, ILocationInputController input) : base(player, input)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _view.IdleForward();
        }
    }
}
