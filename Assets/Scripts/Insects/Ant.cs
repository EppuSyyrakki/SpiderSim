using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpiderSim
{
    public class Ant : Insect
    {
        private AntSpawner spawner;

        [HideInInspector]
        public GameObject[] waypoints;

        [SerializeField] private float minDistance = 1f;

        public float minSpeed = 1f;
        public float maxSpeed = 1.1f;
        private float moveSpeed = 1f;

        private int currentTarget;
        private Vector3 targetVector;
        private bool goingForward = true;

        private float offSet;
        public float minOffSet = 0.1f;
        public float maxOffSet = 0.5f;

        private void Start()
        {
            offSet = Random.Range(minOffSet, maxOffSet);
            moveSpeed = Random.Range(minSpeed, maxSpeed);
        }

        public override void Update()
        {
            SetTargetOffSet();

            float distance = Vector3.Distance(gameObject.transform.position, targetVector);

            if (canMove)
            {
                if (distance > minDistance)
                {
                    Move();
                }
                else
                {
                    if (currentTarget < waypoints.Length - 1 && goingForward)
                    {
                        currentTarget++;

                        if (currentTarget == waypoints.Length - 1)
                        {
                            goingForward = false;
                        }
                    }
                    else
                    {
                        currentTarget--;

                        if (currentTarget == 0)
                        {
                            goingForward = true;
                        }
                    }
                }
            }
        }

        public override void Move()
        {
            SetTargetOffSet();

            gameObject.transform.LookAt(targetVector);
            gameObject.transform.position += gameObject.transform.forward * moveSpeed * Time.deltaTime;
        }

        private void SetTargetOffSet()
        {
            targetVector = waypoints[currentTarget].transform.position;
            targetVector.x += offSet;
            targetVector.z += offSet;
        }

        public void AssignSpawner(AntSpawner spawner)
        {
            this.spawner = spawner;
        }

        public void AssignWaypoints(GameObject[] waypoints)
        {
            this.waypoints = waypoints;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("Web"))
            {
                Debug.Log("Ant collided with web");
                canMove = false;
            }
            else
            {
                Debug.Log("Ant collided");
            }
        }
    }
}
