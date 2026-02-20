using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.Common.Services.Coroutines
{
    public interface ICoroutineManager
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
        void StopCoroutine(Coroutine coroutine);
        void StopAll();
    }
}