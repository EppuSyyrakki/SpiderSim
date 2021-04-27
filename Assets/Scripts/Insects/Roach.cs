using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace SpiderSim
{
    public class Roach : Insect
    {
        private RoachSpawner spawner;

        private List<Collider> obstacles = new List<Collider>();
        //[SerializeField] private LayerMask layersToCheck;

        public NavMeshAgent agent;

        [SerializeField] private float targetTreshold = 0.1f;
        [SerializeField] private float minDistance = 0.1f;
        [SerializeField] public float maxDistance = 2f;
        [SerializeField] public float moveSpeed = 1f;

        [HideInInspector] public Vector3 previousTarget;
        [HideInInspector] public Vector3 currentTarget;

        private float timer;
        [Tooltip("Fail safe timer in case the roach gets stuck")] public float timerDuration = 2f;

        public void Start()
        {
            timer = timerDuration;
        }

        public override void Move()
        {
            timer -= Time.deltaTime;

            if (Vector3.Distance(transform.position, currentTarget) < targetTreshold || timer < 0)
            {
                GetNewDestination();
                timer = timerDuration;
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

            bool destinationOK = false;

            for (int i = 0; i < 10; i++)
            {
                if (NavMesh.SamplePosition(randomDirection, out hit, maxDistance, 1))
                {
                    previousTarget = currentTarget;
                    currentTarget = hit.position;
                    agent.SetDestination(currentTarget);
                    destinationOK = true;
                    break;
                }
                else
                {
                    randomDirection = Random.insideUnitSphere * Random.Range(minDistance, maxDistance);
                    randomDirection += transform.position;
                }
            }

            if (!destinationOK)
            {
                Debug.Log("Didn't find a new target, go to previous");
                Vector3 temporarySave = currentTarget;
                currentTarget = previousTarget;
                previousTarget = temporarySave;
                agent.SetDestination(currentTarget);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("Web"))
            {
                canMove = false;
            }
            else if (other.collider.CompareTag("Ant"))
            {
                GetNewDestination();
                timer = timerDuration;
            }
            else
            {
                //Debug.Log("Roach collided with: " + other.gameObject.name);
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
