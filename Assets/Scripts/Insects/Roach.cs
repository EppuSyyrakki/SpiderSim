using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace SpiderSim
{
    public class Roach : Insect
    {
        private RoachSpawner spawner;

        private List<Collider> obstacles = new List<Collider>();
        [SerializeField] private LayerMask layersToCheck;

        public NavMeshAgent agent;

        [SerializeField] private float targetTreshold = 0.1f;
        [SerializeField] private float minDistance = 0.1f;
        [SerializeField] public float maxDistance = 2f;
        [SerializeField] public float moveSpeed = 1f;

        public Vector3 previousTarget;
        public Vector3 currentTarget;

        public override void Move()
        {
            if (Vector3.Distance(transform.position, currentTarget) < targetTreshold)
            {
                GetNewDestination();
                agent.SetDestination(currentTarget);
            }
        }

        public void AssignSpawner(RoachSpawner spawner)
        {
            this.spawner = spawner;
        }

        public void GetNewDestination()
        {
            Vector3 randomDirection = Random.insideUnitSphere * Random.Range(minDistance, maxDistance);

            randomDirection += transform.position;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomDirection, out hit, maxDistance, 1))
            {
                // Debug.Log("Got new destination");
                previousTarget = currentTarget;
                currentTarget = hit.position;
                agent.SetDestination(currentTarget);
            }
            else
            {
                // Debug.Log("Go to previous target");
                currentTarget = previousTarget;
                agent.SetDestination(currentTarget);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("Web"))
            {
                canMove = false;
            }
            else
            {
                // Debug.Log("Roach collided");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(transform.position, maxDistance);
        }
    }
}
