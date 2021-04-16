using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim
{
    public class Roach : Insect
    {
        private RoachSpawner spawner;

        [SerializeField] private float minDistance = 0.1f;
        [SerializeField] public float moveSpeed = 1f;

        public void AssignSpawner(RoachSpawner spawner)
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
                Debug.Log("Roach collided");
            }
        }

        private void GetNewDestination()
        {

        }
    }
}
