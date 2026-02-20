using UnityEngine;

namespace Assets.Game.Scripts.Locations.Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private const string ForwardWalking = "ForwardWalking";
        private const string BackwardWalking = "BackwardWalking";
        private const string LeftWalking = "LeftWalking";
        private const string RightWalking = "RightWalking";

        private const string ForwardIdle = "ForwardIdle";
        private const string BackwardIdle = "BackwardIdle";
        private const string LeftIdle = "LeftIdle";
        private const string RightIdle = "RightIdle";

        private string _currentState;

        public void WalkingForward() => SetState(ForwardWalking);
        public void WalkingBackward() => SetState(BackwardWalking);
        public void WalkingLeft() => SetState(LeftWalking);
        public void WalkingRight() => SetState(RightWalking);

        public void IdleLeft() => SetState(LeftIdle);
        public void IdleRight() => SetState(RightIdle);
        public void IdleForward() => SetState(ForwardIdle);
        public void IdleBackward() => SetState(BackwardIdle);

        public void Stop()
        {
            _animator.speed = 0;
        }

        private void SetState(string state)
        {
            _animator.speed = 1;

            if (_currentState == state)
                return;

            _currentState = state;

            _animator.Play(state);
        }
    }
}