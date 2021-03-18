using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
			if (Input.GetKeyDown(KeyCode.P))
			{
				Vector3 spawnPoint = GetNewDestination();
				objectPooler.SpawnFromPool("Fly", spawnPoint, Quaternion.identity);
			}

			CheckSurroundings();
        }

        private void CheckSurroundings()
        {
            obstacles.Clear();
	        obstacles.AddRange(Physics.OverlapSphere(transform.position, maxDistance, layersToCheck));
        }

        public Vector3 GetNewDestination()
        {
	        Vector3 destination = Random.insideUnitSphere * Random.Range(0, maxDistance);
            Debug.Log(destination);
            Debug.DrawLine(destination, transform.position);
            return destination;
        }
    }
}
