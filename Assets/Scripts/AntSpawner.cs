using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim
{
    public class AntSpawner : MonoBehaviour
    {
        public ObjectPooler objectPooler;

        private List<Ant> ants = new List<Ant>();

        [SerializeField]
        public GameObject[] waypoints;

        private int num = 0;    //Starting waypoint

        [SerializeField] private int amountToSpawn = 10;
        [SerializeField] private float spawnInterval = 1f;

        void Start()
        {
            objectPooler = ObjectPooler.Instance;

            for (int i = 0; i < amountToSpawn; i++)
            {
                Invoke("SpawnAnts", spawnInterval + spawnInterval * i);
            }
        }

        private void SpawnAnts()
        {
            Vector3 spawnPoint = waypoints[num].transform.position;
            IPooledObject antObj = objectPooler.SpawnFromPool("Ant", spawnPoint, Quaternion.identity);
            Ant ant = antObj.GameObject().GetComponent<Ant>();
            ant.AssignSpawner(this);
            ant.AssignWaypoints(waypoints);
            ants.Add(ant);
        }
    }
}
