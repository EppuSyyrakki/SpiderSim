using UnityEngine;

namespace SpiderSim
{
	public interface IPooledObject
	{
		/// <summary>
		/// Called automatically by ObjectPooler when this object is fetched from the pool.
		/// </summary>
		public void Activate();

		/// <summary>
		/// Call this to deactivate object from pool instead of destroying it
		/// </summary>
		public void Deactivate();
	}
}

