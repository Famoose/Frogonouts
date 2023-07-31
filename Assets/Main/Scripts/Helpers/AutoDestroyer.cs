using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

namespace Main.Scripts.Helpers
{
    public class AutoDestroyer : MonoBehaviour
    {
        [SerializeField] private float autoDestroyTime = 1f;
        [SerializeField] private MMFeedbacks destoryFeedbacks;
        public UnityEvent onDestroy = new UnityEvent();

        public void StartAutoDestroyer()
        {
            destoryFeedbacks?.PlayFeedbacks();
            StartCoroutine(AutoDestroy());
        }

        private IEnumerator AutoDestroy()
        {
            yield return new WaitForSeconds(autoDestroyTime);
            onDestroy?.Invoke();
        }
    }
}