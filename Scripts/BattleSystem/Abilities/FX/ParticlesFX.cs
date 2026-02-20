using UnityEngine;

namespace Assets.Game.Scripts.BattleSystem.Abilities.FX
{
    public class ParticlesFX : MonoBehaviour
    {
        [SerializeField] private float _delay;
        [SerializeField] private ParticleSystem _particleSystem;

        public float Delay => _delay;

        private void Start() => Destroy(gameObject, _particleSystem.main.duration);

        public void Play() => _particleSystem.Play();
    }
}
