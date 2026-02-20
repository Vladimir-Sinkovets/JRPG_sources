using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.Common.Services.Coroutines
{
    public class CoroutineManager : MonoBehaviour, ICoroutineManager
    {
        public void StopAll()
        {
            StopAllCoroutines();
        }

        Coroutine ICoroutineManager.StartCoroutine(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }

        void ICoroutineManager.StopCoroutine(Coroutine coroutine)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
        }
    }
}
