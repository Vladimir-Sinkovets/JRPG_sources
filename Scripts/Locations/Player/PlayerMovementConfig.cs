using UnityEngine;

namespace Assets.Game.Scripts.Locations.Player
{
    [CreateAssetMenu(fileName = "playerMovement", menuName = "Player/Create movement config")]
    public class PlayerMovementConfig : ScriptableObject
    {
        public float speed;
        public float speedUp;
        public float gravity = -9.8f;
    }
}
