using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

namespace Main.Scripts.Player
{
    public class DeathTracker : MonoBehaviour
    {
        public UnityEvent onDeath;
        
        [SerializeField] private float deathHeight;
        [SerializeField] private float warningTriggerHeight;
        [SerializeField] private MMFeedbacks drownFeedback;

        private bool _isPlaying = false;
        private void Update()
        {
            var height = transform.position.y;
            if (!_isPlaying && height < warningTriggerHeight)
            {
                drownFeedback?.PlayFeedbacks();
                _isPlaying = true;
            }

            if (_isPlaying && height > warningTriggerHeight)
            {
                drownFeedback?.StopFeedbacks();
                drownFeedback?.ResetFeedbacks();
                _isPlaying = false;
            }
            
            if (height < deathHeight)
            {
                onDeath?.Invoke();
            }
        }
    }
}