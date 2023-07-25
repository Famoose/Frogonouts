using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Scripts
{
    public class Projection : MonoBehaviour
    {
        [SerializeField] private LineRenderer _line;
        [SerializeField] private int _maxPhysicsFrameIterations = 100;
        [SerializeField] private Transform _obstaclesParent;

        private Scene _simulationScene;
        private PhysicsScene _physicsScene;
        private readonly Dictionary<Transform, Transform> _spawnedObjects = new Dictionary<Transform, Transform>();
        private Ball _ghostObj;

        private void Start()
        {
            CreatePhysicsScene();
        }

        private void CreatePhysicsScene()
        {
            _simulationScene =
                SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
            _physicsScene = _simulationScene.GetPhysicsScene();

            foreach (Transform obj in _obstaclesParent)
            {
                var ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
                ghostObj.GetComponent<Renderer>().enabled = false;
                SceneManager.MoveGameObjectToScene(ghostObj, _simulationScene);
                if (!ghostObj.isStatic) _spawnedObjects.Add(obj, ghostObj.transform);
            }
        }

        private void Update()
        {
            foreach (var item in _spawnedObjects)
            {
                item.Value.position = item.Key.position;
                item.Value.rotation = item.Key.rotation;
            }
        }
        
        public void HideTrajectory()
        {
            _line.enabled = false;
        }

        public void SimulateTrajectory(Ball ballPrefab, Vector3 pos, Vector3 velocity)
        {
            _line.enabled = true;

            if (_ghostObj)
            {
                _ghostObj.transform.position = pos;
                _ghostObj.transform.rotation = Quaternion.identity;
            }
            else
            {
                _ghostObj = Instantiate(ballPrefab, pos, Quaternion.identity);
                SceneManager.MoveGameObjectToScene(_ghostObj.gameObject, _simulationScene);
                _ghostObj.GetComponent<Renderer>().enabled = false;
            }
            
            _ghostObj.Init(velocity);

            _line.positionCount = _maxPhysicsFrameIterations;

            for (var i = 0; i < _maxPhysicsFrameIterations; i++)
            {
                _physicsScene.Simulate(Time.fixedDeltaTime);
                _line.SetPosition(i, _ghostObj.transform.position);
            }
        }
    }
}