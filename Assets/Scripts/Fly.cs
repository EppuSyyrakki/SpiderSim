using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpiderSim
{
    [DisallowMultipleComponent]
    public class Fly : MonoBehaviour, IPooledObject
    {
        [SerializeField] Vector3 movementVector;
        [SerializeField] float period = 2f;
        [SerializeField] private float targetTreshold = 0.1f;

        private float movementFactor;
        private Vector3 startingPos;
        private FlySpawner spawner;

        public Vector3 moveTarget = Vector3.zero;
        public Vector3 previousTarget;
        public float moveSpeed;
        public float turnSpeed;

        private bool canMove = true;

        private Rigidbody rb;
        
        void Update()
        {
            /*
             if (period <= Mathf.Epsilon) { return; }
            float cycles = Time.time / period; // Grows continually from 0

            const float tau = Mathf.PI * 2;
            float rawSinWave = Mathf.Sin(cycles * tau);

            movementFactor = rawSinWave / 2f + 0.5f;
            Vector3 offset = movementFactor * movementVector;
            transform.position = startingPos + offset;
            */

            if (canMove)
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
            else
            {
                // Stops the fly from moving on its own. Outside force can still affect it a bit.
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        public void Activate(Vector3 position, Quaternion rotation)
        {
            gameObject.SetActive(true);
            transform.position = position;
            transform.rotation = rotation;
            rb = GetComponent<Rigidbody>();
        }

        public void Deactivate()
        {
	        gameObject.SetActive(false);
        }

        public GameObject GameObject()
        {
	        return gameObject;
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
