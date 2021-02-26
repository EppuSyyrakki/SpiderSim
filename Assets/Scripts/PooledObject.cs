using UnityEngine;

namespace SpiderSim
{
	public abstract class PooledObject : MonoBehaviour
	{
		public virtual void OnObjectSpawn()
		{
			Debug.Log("PooledObject parent called spawn method");
		}

	}
}

