using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim
{
    public class MothSpawner : MonoBehaviour
    {
        public ObjectPooler objectPooler;

        private List<Moth> moths = new List<Moth>();
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
                IPooledObject mothObj = objectPooler.SpawnFromPool("Moth", spawnPoint, Quaternion.identity);

                Moth moth = mothObj.GameObject().GetComponent<Moth>();
                moth.moveTarget = GetNewDestination();
                moth.previousTarget = spawnPoint;
                moth.AssignSpawner(this);
                moths.Add(moth);
            }
        }

        void Update()
        {
            CheckSurroundings();
        }

        public Vector3 GetNewDestination()
        {
            float minDistance = maxDistance / 3;
            Vector3 localDestination = Random.insideUnitSphere * Random.Range(minDistance, maxDistance);
            Vector3 destination = transform.TransformPoint(localDestination);

            bool destinationOK = false;

            for (int i = 0; i < 10; i++)
            {
                if (!CheckNewDestination(destination))
                {
                    localDestination = Random.insideUnitSphere * Random.Range(minDistance, maxDistance);
                    destination = transform.TransformPoint(localDestination);
                }
                else if (CheckNewDestination(destination))
                {
                    destinationOK = true;
                    break;
                }
            }

            if (!destinationOK)
            {
                Debug.Log("Fly couldn't find a spot");
            }

            /*
            while (!CheckNewDestination(destination))
            {
                localDestination = Random.insideUnitSphere * Random.Range(minDistance, maxDistance);
                destination = transform.TransformPoint(localDestination);
            }
            */

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

        private void CheckSurroundings()
        {
            obstacles.Clear();
            obstacles.AddRange(Physics.OverlapSphere(transform.position, maxDistance, layersToCheck));
        }
    }

}
