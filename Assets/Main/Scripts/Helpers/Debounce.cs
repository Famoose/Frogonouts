using System;
using System.Collections;
using UnityEngine;

namespace Main.Scripts.Helpers
{
    public class Debounce
    {
        Action _callback = null;

        Coroutine _corountine = null;
        
        public void Run(Action callback, float interval, MonoBehaviour mono)
        {
            _callback = callback;

            ResetTime(mono);

            _corountine = mono.StartCoroutine(DebounceCorountine(interval));
        }

        public void ResetTime(MonoBehaviour mono)
        {
            if (_corountine != null)
            {
                mono.StopCoroutine(_corountine);
            }
        }

        private IEnumerator DebounceCorountine(float time)
        {
            yield return new WaitForSeconds(time);
            _callback?.Invoke();
        }
    }
}