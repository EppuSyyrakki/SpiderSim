using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpiderSim
{
    public class FlySpawner : MonoBehaviour
    {
        public ObjectPooler objectPooler;

        private List<Fly> flies = new List<Fly>();
        private List<Collider> obstacles = new List<Collider>();

        [SerializeField]
        private float maxDistance = 25f;

        [SerializeField]
        private int amountToSpawn = 10;

        [SerializeField]
        private LayerMask layersToCheck;

        void Start()
        {
            objectPooler = ObjectPooler.Instance;

            for (int i = 0; i < amountToSpawn; i++)
            {
	            Vector3 spawnPoint = GetNewDestination();
	            IPooledObject flyObj = objectPooler.SpawnFromPool("Fly", spawnPoint, Quaternion.identity);
	            Fly fly = flyObj.GameObject().GetComponent<Fly>();
	            fly.moveTarget = GetNewDestination();
	            fly.AssignSpawner(this);
                flies.Add(fly);
            }
        }


        void Update()
        {
	        CheckSurroundings();
        }

        private void CheckSurroundings()
        {
            obstacles.Clear();
	        obstacles.AddRange(Physics.OverlapSphere(transform.position, maxDistance, layersToCheck));
        }

        public Vector3 GetNewDestination()
        {
	        float minDistance = maxDistance / 3;
	        Vector3 localDestination = Random.insideUnitSphere * Random.Range(minDistance, maxDistance);
	        Vector3 destination = transform.TransformPoint(localDestination);

	        // TODO: Tarkista onko se uusi koordinaatti jonkun listalla olevan colliderin sisällä.

            return destination;
        }

        private void OnDrawGizmos()
        {
	        Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, maxDistance);
        }
    }
}
