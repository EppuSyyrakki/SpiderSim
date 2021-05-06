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

            Roach[] roaches = FindObjectsOfType<Roach>();

            foreach (var roach in roaches)
            {
                Physics.IgnoreCollision(roach.GetComponent<Collider>(), GetComponent<Collider>());
            }
            
        }

        public override void Update()
        {
            SetTargetOffSet();

            float distance = Vector3.Distance(gameObject.transform.position, targetVector);

            if (canMove)
            {
                // In case collision sends the ant flying
                if (transform.position.y > 3f)
                {
                    Vector3 height = transform.position;
                    height.y = spawner.transform.position.y;
                    transform.position = height;
                }

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
            else
            {
                StopMovement();
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
                canMove = false;
            }
        }
    }
}
