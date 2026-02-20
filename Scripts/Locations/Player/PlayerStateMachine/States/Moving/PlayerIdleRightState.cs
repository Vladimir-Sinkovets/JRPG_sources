using Assets.Game.Scripts.Locations.Services.Input;

namespace Assets.Game.Scripts.Locations.Player.PlayerStateMachine.States.Moving
{
    public class PlayerIdleRightState : PlayerMovingState
    {
        public PlayerIdleRightState(PlayerController player, ILocationInputController input) : base(player, input) { }

        public override void Enter()
        {
            base.Enter();

            _view.IdleRight();
        }
    }
}
