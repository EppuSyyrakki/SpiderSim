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

        [SerializeField] private int amountToSpawn = 5;
        [SerializeField] private float walkRadius = 3f;

        void Start()
        {
            objectPooler = ObjectPooler.Instance;

            for (int i = 0; i < amountToSpawn; i++)
            {
                Vector3 spawnPoint = GetNewDestination();
                IPooledObject roachObj = objectPooler.SpawnFromPool("Roach", spawnPoint, Quaternion.identity);
                Roach roach = roachObj.GameObject().GetComponent<Roach>();
                roach.previousTarget = GetNewDestination();
                roach.GetNewDestination();
                roach.AssignSpawner(this);
                roaches.Add(roach);
            }
        }

        public Vector3 GetNewDestination()
        {
            return transform.position;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, walkRadius);
        }
    }
}
