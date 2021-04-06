using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpiderSim
{
    [DisallowMultipleComponent]
    public class Fly : Insect
    {
        [SerializeField] private float targetTreshold = 0.1f;

        private FlySpawner spawner;

        public Vector3 moveTarget = Vector3.zero;
        public Vector3 previousTarget;
        public float moveSpeed;
        public float turnSpeed;

        public override void Move()
        {
            Vector3 vTraj = moveTarget - transform.position;


            Quaternion qTargetRotation = Quaternion.LookRotation(vTraj, Vector3.up);
            Quaternion qLimitedRotation = Quaternion.Slerp(transform.rotation, qTargetRotation, turnSpeed * Time.deltaTime);

            transform.rotation = qLimitedRotation;
            transform.position += transform.forward * (moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, moveTarget) < targetTreshold)
            {
                previousTarget = moveTarget;
                moveTarget = spawner.GetNewDestination();
            }
        }

        public void AssignSpawner(FlySpawner spawner)
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
                Debug.Log("Fly collided, get new destination");
            }
        }
    }
}
