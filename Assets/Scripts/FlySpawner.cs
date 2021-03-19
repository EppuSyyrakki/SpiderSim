using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace SpiderSim
{
    public class FlySpawner : MonoBehaviour
    {
        public ObjectPooler objectPooler;

        private List<Fly> flies = new List<Fly>();
        private List<Collider> obstacles = new List<Collider>();

        [SerializeField]
        private float maxDistance = 25f;

        [SerializeField]
        private int amountToSpawn = 10;

        [SerializeField]
        private LayerMask layersToCheck;

        void Start()
        {
            objectPooler = ObjectPooler.Instance;

            for (int i = 0; i < amountToSpawn; i++)
            {
	            Vector3 spawnPoint = GetNewDestination();
	            IPooledObject flyObj = objectPooler.SpawnFromPool("Fly", spawnPoint, Quaternion.identity);
	            Fly fly = flyObj.GameObject().GetComponent<Fly>();
	            fly.moveTarget = GetNewDestination();
	            fly.AssignSpawner(this);
                flies.Add(fly);
            }
        }


        void Update()
        {
	        CheckSurroundings();
        }

        private void CheckSurroundings()
        {
            obstacles.Clear();
	        obstacles.AddRange(Physics.OverlapSphere(transform.position, maxDistance, layersToCheck));
        }

        public Vector3 GetNewDestination()
        {
	        float minDistance = maxDistance / 3;
	        Vector3 localDestination = Random.insideUnitSphere * Random.Range(minDistance, maxDistance);
	        Vector3 destination = transform.TransformPoint(localDestination);

            while (!CheckNewDestination(destination))
            {
                localDestination = Random.insideUnitSphere * Random.Range(minDistance, maxDistance);
                destination = transform.TransformPoint(localDestination);
            }

            return destination;
        }

        // Checks if the destination is inside a collider. If it's not inside a collider,
        // returns true; otherwise false.
        private bool CheckNewDestination(Vector3 destination)
        {
            foreach (Collider col in obstacles)
            {
                if (col.bounds.Contains(destination))
                {
                    return false;
                }
            }

            return true;
        }

        private void OnDrawGizmos()
        {
	        Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, maxDistance);
        }
    }
}
