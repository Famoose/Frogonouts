using UnityEngine;

namespace Main.Scripts.Player
{
    public class Projection : MonoBehaviour
    {
        [SerializeField] private LineRenderer _line;
        [SerializeField] private int _maxPhysicsFrameIterations = 100;

        public void HideTrajectory()
        {
            _line.enabled = false;
        }

        public void CalculatePosition(Vector3 startPosition, Vector3 initialVelocity)
        {
            var gravity = Physics.gravity;
            var timeStep = Time.fixedDeltaTime;
            var steps = _maxPhysicsFrameIterations;
            
            _line.enabled = true;

            Vector3 currentVelocity = initialVelocity;

            Vector3[] positions = new Vector3[steps];

            _line.positionCount = steps;

            for (int i = 0; i < steps; i++)
            {
                currentVelocity += gravity * timeStep;
                if (i == 0)
                {
                    positions[i] = startPosition + currentVelocity * timeStep;
                }
                else
                {
                    positions[i] = positions[i - 1] + currentVelocity * timeStep;
                }
                _line.SetPosition(i, positions[i]);

            }
        }
    }
}