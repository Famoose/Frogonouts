using UnityEngine;

namespace Main.Scripts
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;

        public void Init(Vector3 velocity)
        {
            _rb.AddForce(velocity, ForceMode.Impulse);
        }

        public void OnCollisionEnter(Collision col)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
        
    }
}