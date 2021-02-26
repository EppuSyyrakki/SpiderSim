using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim
{
    public class FlySpawner : MonoBehaviour
    {
        public ObjectPooler objectPooler;

        void Start()
        {
            objectPooler = ObjectPooler.Instance;
        }


        void Update()
        {
	        if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("Mouse 2 pressed");
	            Vector3 spawnPoint = new Vector3(Random.Range(1f, 4f), Random.Range(2f, 5f), Random.Range(1f, 4f));
                objectPooler.SpawnFromPool("Fly", spawnPoint, Quaternion.identity);
            }
        }
    }
}
