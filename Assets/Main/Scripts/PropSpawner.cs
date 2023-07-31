using System;
using System.Collections.Generic;
using System.Numerics;
using Main.Scripts.Helpers;
using UnityEngine;
using UnityEngine.Pool;
using Vector3 = UnityEngine.Vector3;

namespace Main.Scripts
{
    public class PropSpawner : MonoBehaviour
    {
        [SerializeField] private List<AutoDestroyer> props;
        [SerializeField] private float offset = 4;
        [SerializeField] private GameObject target;
        
        private ObjectPool<AutoDestroyer> objectPool;
        
        
        // throw an exception if we try to return an existing item, already in the pool
        [SerializeField] private bool collectionCheck = false;

        // extra options to control the pool capacity and maximum size
        [SerializeField] private int defaultCapacity = 20;
        [SerializeField] private int maxSize = 100;

        private Vector3 lastSpawnPosition = Vector3.zero;

        private void Start()
        {
            var targetPosition = target.transform.position;
            for (int i = 1; i < defaultCapacity; i++)
            {
                var prop = objectPool.Get();
                var offsetX = UnityEngine.Random.Range(-offset, offset);
                prop.transform.position = targetPosition + transform.forward * (i * offset) + transform.right * offsetX;
                lastSpawnPosition = targetPosition;
            }
        }

        private void Update()
        {
            //check distance on player and spawn 20 next props in front of player
            var position = target.transform.position;
            var targetForwardPosition = new Vector3(0, 0, position.z);
            if (Vector3.Distance(lastSpawnPosition, targetForwardPosition) > defaultCapacity)
            {
                var targetPositionForward = targetForwardPosition + targetForwardPosition * defaultCapacity;
                for (int i = 0; i < defaultCapacity; i++)
                {
                    var prop = objectPool.Get();
                    var offsetX = UnityEngine.Random.Range(-offset, offset);
                    prop.transform.position = targetPositionForward + transform.forward * (i * offset) + transform.right * offsetX;
                }
                lastSpawnPosition = targetForwardPosition;
            }
        }

        private void Awake()
        {
            objectPool = new ObjectPool<AutoDestroyer>(CreateProjectile,
                OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
                collectionCheck, defaultCapacity, maxSize);
        }
        
        // invoked when creating an item to populate the object pool
        private AutoDestroyer CreateProjectile()
        {
            AutoDestroyer propInstance = Instantiate(props[UnityEngine.Random.Range(0, props.Count)]);
            propInstance.onDestroy.AddListener(() => objectPool.Release(propInstance));
            return propInstance;
        }
        
        // invoked when returning an item to the object pool
        private void OnReleaseToPool(AutoDestroyer pooledObject)
        {
            pooledObject.gameObject.SetActive(false);
        }

        // invoked when retrieving the next item from the object pool
        private void OnGetFromPool(AutoDestroyer pooledObject)
        {
            pooledObject.gameObject.SetActive(true);
        }

        // invoked when we exceed the maximum number of pooled items (i.e. destroy the pooled object)
        private void OnDestroyPooledObject(AutoDestroyer pooledObject)
        {
            Destroy(pooledObject.gameObject);
        }

    }
}