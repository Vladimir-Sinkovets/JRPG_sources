using UnityEngine;

namespace Assets.Game.Scripts.Common.Services.Audio
{
    public interface IAudioService
    {
        void PlaySFX(AudioClip clip);
        void PlaySFX(AudioClip clip, Vector3 position);
    }
}
