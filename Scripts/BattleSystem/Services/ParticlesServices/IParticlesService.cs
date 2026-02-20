using UnityEngine;

namespace Assets.Game.Scripts.BattleSystem.Services.ParticlesServices
{
    public interface IParticlesService
    {
        void Play(ParticleSystem prefab);
    }
}