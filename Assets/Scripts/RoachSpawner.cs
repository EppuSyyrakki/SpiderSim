using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SpiderSim
{
    public class RoachSpawner : MonoBehaviour
    {
        public ObjectPooler objectPooler;

        private NavMeshAgent agent;

        private List<Roach> roaches = new List<Roach>();
        private List<Collider> obstacles = new List<Collider>();

        [SerializeField] private int amountToSpawn = 5;

        [SerializeField] private float maxSpawnDistance = 1f;

        [SerializeField] private LayerMask layersToCheck;

        void Start()
        {
            objectPooler = ObjectPooler.Instance;

            for (int i = 0; i < amountToSpawn; i++)
            {
                Vector3 spawnPoint = GetStartingPoint();
                Debug.Log(spawnPoint);
                IPooledObject roachObj = objectPooler.SpawnFromPool("Roach", spawnPoint, Quaternion.identity);
                Roach roach = roachObj.GameObject().GetComponent<Roach>();
                roach.GetNewDestination();
                roach.previousTarget = spawnPoint;
                //roach.AssignSpawner(this);
                roaches.Add(roach);
            }
        }
        void Update()
        {
            CheckSurroundings();
        }

        private void CheckSurroundings()
        {
            obstacles.Clear();
            obstacles.AddRange(Physics.OverlapSphere(transform.position, maxSpawnDistance, layersToCheck));
        }

        public Vector3 GetNewDestination()
        {
            float minDistance = maxSpawnDistance / 3;
            Vector3 localDestination = Random.insideUnitSphere * Random.Range(minDistance, maxSpawnDistance);
            Vector3 destination = transform.TransformPoint(localDestination);

            for (int i = 0; i < 10; i++)
            {
                if (!CheckNewDestination(destination))
                {
                    localDestination = Random.insideUnitSphere * Random.Range(minDistance, maxSpawnDistance);
                    destination = transform.TransformPoint(localDestination);
                }
                else if (CheckNewDestination(destination))
                {
                    break;
                }
            }

            return destination;
        }

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

        public Vector3 GetStartingPoint()
        {
            return transform.position;
        }
    }
}
