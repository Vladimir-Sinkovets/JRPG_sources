using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.BattleSystem.Services.ParticlesServices
{
    public class ParticlesService : MonoBehaviour, IParticlesService
    {
        [SerializeField] private Transform _particlesContainer;

        private readonly Dictionary<ParticleSystem, Queue<ParticleSystem>> _pool = new();
        private readonly Dictionary<ParticleSystem, Coroutine> _returnCoroutines = new();

        public void Play(ParticleSystem prefab)
        {
            if (prefab == null) return;

            if (!_pool.TryGetValue(prefab, out var queue))
            {
                queue = new Queue<ParticleSystem>();
                _pool[prefab] = queue;
            }

            ParticleSystem instance;
            if (queue.Count > 0)
            {
                instance = queue.Dequeue();
            }
            else
            {
                instance = Instantiate(prefab, _particlesContainer);
                var main = instance.main;
                main.stopAction = ParticleSystemStopAction.Callback;
            }

            instance.gameObject.SetActive(true);
            instance.Play();

            if (_returnCoroutines.TryGetValue(instance, out var oldCoroutine))
            {
                StopCoroutine(oldCoroutine);
            }

            _returnCoroutines[instance] = StartCoroutine(ReturnToPoolWhenFinished(instance, prefab));
        }

        private IEnumerator ReturnToPoolWhenFinished(ParticleSystem instance, ParticleSystem prefab)
        {
            yield return new WaitWhile(() => instance.isPlaying);
            ReturnToPool(instance, prefab);
        }

        private void ReturnToPool(ParticleSystem instance, ParticleSystem prefab)
        {
            instance.gameObject.SetActive(false);
            _pool[prefab].Enqueue(instance);
            _returnCoroutines.Remove(instance);
        }
    }
}
