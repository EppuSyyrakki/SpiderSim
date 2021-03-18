using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpiderSim
{
    public class FlySpawner : MonoBehaviour
    {
        public ObjectPooler objectPooler;

        private List<Transform> flies = new List<Transform>();
        private List<Collider> obstacles = new List<Collider>();

        [SerializeField]
        private float maxDistance = 25f;

        [SerializeField]
        private LayerMask layersToCheck;

        void Start()
        {
            objectPooler = ObjectPooler.Instance;
        }


        void Update()
        {
	        CheckSurroundings();

            if (Input.GetKeyDown(KeyCode.P))
			{
				Vector3 spawnPoint = GetNewDestination();
				IPooledObject flyObj = objectPooler.SpawnFromPool("Fly", spawnPoint, Quaternion.identity);
				Fly fly = flyObj.GameObject().GetComponent<Fly>();
				fly.moveTarget = GetNewDestination();
				Debug.DrawLine(fly.transform.position, fly.moveTarget, Color.white, 1f);
                fly.AssignSpawner(this);
            }
	    }

        private void CheckSurroundings()
        {
            obstacles.Clear();
	        obstacles.AddRange(Physics.OverlapSphere(transform.position, maxDistance, layersToCheck));
        }

        public Vector3 GetNewDestination()
        {
	        Vector3 localDestination = Random.insideUnitSphere * Random.Range(0, maxDistance);
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
