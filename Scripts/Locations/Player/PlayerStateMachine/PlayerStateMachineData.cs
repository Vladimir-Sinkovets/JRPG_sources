using UnityEngine;

namespace Assets.Game.Scripts.Locations.Player.PlayerStateMachine
{
    public class PlayerStateMachineData
    {
        public Vector2 MovingDirection { get; set; }
        public float Speed { get; set; }
        public float VerticalVelocity { get; set; }
    }
}
