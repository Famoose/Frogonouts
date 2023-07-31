using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

namespace Main.Scripts.Helpers
{
    public class CollisionEffect : MonoBehaviour
    {

        public UnityEvent onCollision;
        [SerializeField] public MMFeedbacks feedbacks;
            
        private void OnCollisionEnter(Collision other)
        {
            feedbacks?.PlayFeedbacks();
            onCollision?.Invoke();
        }
    }
}
