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

        float movementFactor;

        Vector3 startingPos;

        public Transform moveTowards;
        public float moveSpeed;
        public float turnSpeed;

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

            Vector3 vTraj = moveTowards.position - transform.position;


            Quaternion qTargetRotation = Quaternion.LookRotation(vTraj, Vector3.up);
            Quaternion qLimitedRotation = Quaternion.Slerp(transform.rotation, qTargetRotation, turnSpeed * Time.deltaTime);

            transform.rotation = qLimitedRotation;
            transform.position += transform.forward * (moveSpeed * Time.deltaTime);
        }

        public void Activate(Vector3 position, Quaternion rotation)
        {
            gameObject.SetActive(true);
            startingPos = position;
            transform.rotation = rotation;
            movementVector = new Vector3(Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3));
            period *= Random.Range(0.75f, 1.25f);
        }

        public void Deactivate()
        {
	        gameObject.SetActive(false);
        }

        public GameObject GameObject()
        {
	        return gameObject;
        }

        private void OnCollisionEnter(Collision other)
        {
	        Debug.Log("Fly collided"); 
	        Deactivate();
        }
    }
}
