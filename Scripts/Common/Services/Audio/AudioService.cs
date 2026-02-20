using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Game.Scripts.Common.Services.Audio
{
    public class AudioService : MonoBehaviour, IAudioService
    {
        [SerializeField] private AudioSource _audioSourcePrefab;
        [SerializeField] private int _initialPoolSize = 5;
        [Space]
        [SerializeField] private AudioMixerGroup _sfxAudioMixerGroup;

        private Queue<AudioSource> _audioSourcePool = new Queue<AudioSource>();

        private void Awake()
        {
            for (int i = 0; i < _initialPoolSize; i++)
            {
                CreateNewAudioSource();
            }
        }

        public void PlaySFX(AudioClip clip, Vector3 position)
        {
            if (clip == null) return;

            var source = GetAvailableAudioSource();

            source.transform.position = position;
            
            StartCoroutine(PlayClipAndReturnToPool(source, clip));
        }

        public void PlaySFX(AudioClip clip)
        {
            PlaySFX(clip, Vector3.zero);
        }


        private AudioSource CreateNewAudioSource()
        {
            AudioSource newSource = Instantiate(_audioSourcePrefab, transform);

            newSource.gameObject.SetActive(false);
            _audioSourcePool.Enqueue(newSource);
            return newSource;
        }

        private AudioSource GetAvailableAudioSource()
        {
            if (_audioSourcePool.Count == 0)
            {
                CreateNewAudioSource();
            }

            var source = _audioSourcePool.Dequeue();

            source.gameObject.SetActive(true);

            return source;
        }

        private void ReturnAudioSourceToPool(AudioSource source)
        {
            source.Stop();
            source.clip = null;

            source.gameObject.SetActive(false);

            _audioSourcePool.Enqueue(source);
        }

        private System.Collections.IEnumerator PlayClipAndReturnToPool(AudioSource source, AudioClip clip)
        {
            source.clip = clip;
            source.Play();

            yield return new WaitForSeconds(clip.length);

            ReturnAudioSourceToPool(source);
        }
    }

}
