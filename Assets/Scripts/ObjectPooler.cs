using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim
{
	/// <summary>
	/// This is a main container for all the Object Pools in the game. It should live on a Game Manager
	/// or other singleton pattern object.
	/// </summary>
	public class ObjectPooler : MonoBehaviour
	{
		/// <summary>
		/// Basic information on a pool. Tag, the prefab it clones from, and the max size.
		/// </summary>
		[System.Serializable]
		public class Pool
		{
			public string tag;
			public GameObject prefab;
			public int size;
		}
		
		/// <summary>
		/// Ensure that only 1 instance of this exists.
		/// </summary>
		public static ObjectPooler Instance;
		
		private void Awake()
		{
			Instance = this;
		}

		// User configurable amount of pools for different game objects
		[SerializeField]
		private List<Pool> pools;

		// The dictionary containing all the pools as Queues
		public Dictionary<string, Queue<GameObject>> poolDictionary;
		
		void Start()
		{
			// Create the dictionary
			poolDictionary = new Dictionary<string, Queue<GameObject>>();

			// Populate all the Queues
			foreach (Pool pool in pools)
			{
				Queue<GameObject> objectPool = new Queue<GameObject>();

				// loop through the size of the pool enqueue instantiate deactivated objects
				for (int i = 0; i < pool.size; i++)
				{
					GameObject obj = Instantiate(pool.prefab);
					obj.SetActive(false);
					objectPool.Enqueue(obj);
				}

				// Add the new queue to the dictionary
				poolDictionary.Add(pool.tag, objectPool);
			}
		}

		/// <summary>
		/// Dequeues and resets an object from the pool corresponding to given tag. Then activates and
		/// returns that GameObject. If tag was not found, return null.
		/// </summary>
		/// <param name="tag">Name of the pool</param>
		/// <param name="position">Position of the new object</param>
		/// <param name="rotation">Rotation of the new object</param>
		/// <returns>The GameObject from the object pool. Null if tag not found</returns>
		public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
		{
			if (!poolDictionary.ContainsKey(tag))
			{
				Debug.LogWarning("Pool with tag " + tag + " doesn't exist");
				return null;
			}

			GameObject objectToSpawn = poolDictionary[tag].Dequeue();
			objectToSpawn.SetActive(true);
			objectToSpawn.transform.position = position;
			objectToSpawn.transform.rotation = rotation;

			IPooledObject pooled = objectToSpawn.GetComponent<IPooledObject>();

			pooled?.Activate();

			poolDictionary[tag].Enqueue(objectToSpawn);

			return objectToSpawn;
		}
	}
}
