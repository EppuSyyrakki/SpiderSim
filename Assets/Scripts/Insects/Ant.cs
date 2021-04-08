using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim
{
    public class Ant : Insect
    {
        private AntSpawner spawner;

        [HideInInspector]
        public GameObject[] waypoints;

        [SerializeField] private float minDistance = 1f;
        [SerializeField] public float moveSpeed = 10f;

        private int currentTarget;
        private bool goingForward = true;

        public override void Update()
        {
            float distance = Vector3.Distance(gameObject.transform.position, waypoints[currentTarget].transform.position);

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
            gameObject.transform.LookAt(waypoints[currentTarget].transform.position);
            gameObject.transform.position += gameObject.transform.forward * moveSpeed * Time.deltaTime;
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
            else
            {
                Debug.Log("Ant collided");
            }
        }
    }
}
