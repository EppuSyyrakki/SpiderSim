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

        void Start()
        {
            objectPooler = ObjectPooler.Instance;
        }


        void Update()
        {
	        //if (Input.GetKeyDown(KeyCode.P))
			//{
			//  Vector3 spawnPoint = new Vector3(Random.Range(1f, 4f), Random.Range(2f, 5f), Random.Range(1f, 4f));
			//   objectPooler.SpawnFromPool("Fly", spawnPoint, Quaternion.identity);
			//}

            obstacles = 
        }

        private Collider[] CheckSurroundings()
        {

        }
    }
}
