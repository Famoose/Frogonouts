using System;
using UnityEngine;

namespace Main.Scripts.Helpers
{
    public class Throttle
    {
        float _targetTime = 0;
        
        public void Run(Action callback, float interval)
        {
            if (_targetTime <= Time.time)
            {
                _targetTime = Time.time + interval;
                callback?.Invoke();
            }
        }

        public void ResetTimer()
        {
            _targetTime = 0;
        }
    }
}