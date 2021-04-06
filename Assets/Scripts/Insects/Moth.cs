using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim
{
    public class Moth : Insect
    {
        [SerializeField] private float targetTreshold = 0.1f;

        private MothSpawner spawner;

        public Vector3 moveTarget = Vector3.zero;
        public Vector3 previousTarget;
        public float moveSpeed;
        public float turnSpeed;
        public bool goingToLamp = false;

        public override void Move()
        {
            Vector3 vTraj = moveTarget - transform.position;

            Quaternion qTargetRotation = Quaternion.LookRotation(vTraj, Vector3.up);
            Quaternion qLimitedRotation = Quaternion.Slerp(transform.rotation, qTargetRotation, turnSpeed * Time.deltaTime);

            transform.rotation = qLimitedRotation;
            transform.position += transform.forward * (moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, moveTarget) < targetTreshold)
            {
                if (goingToLamp)
                {
                    goingToLamp = false;
                    moveTarget = spawner.GetNewDestination();
                }
                else
                {
                    goingToLamp = true;
                    previousTarget = moveTarget;
                    moveTarget = spawner.GetCenterCoordinates();
                }
                
            }
        }

        public void AssignSpawner(MothSpawner spawner)
        {
            this.spawner = spawner;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("Web"))
            {
                canMove = false;
            }
            else
            {
                moveTarget = previousTarget;
                Debug.Log("Moth collided, get new destination");
            }
        }
    }
}
