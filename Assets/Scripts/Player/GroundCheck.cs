using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim
{
	public class GroundCheck : MonoBehaviour
	{
		[SerializeField] 
		private float groundOffset = 0f;

		[SerializeField] 
		private float groundCastRadius = 0.1f;

		[SerializeField] 
		private float blockCastRadius = 0.5f;

		[SerializeField] 
		private float blockCastHeight = 0.5f;

		[SerializeField]
		private LayerMask ignoreLayer;
		
		private Vector3 _castPosition;

		/// <summary>
		/// Checks sphere overlap for all colliders for tag Walkable.
		/// </summary>
		/// <returns>True if object tagged Walkable is found</returns>
		public bool IsGrounded()
		{
			Vector3 self = transform.position;
			Vector3 castPos = new Vector3(self.x, self.y + groundOffset, self.z);
			Collider[] cols = Physics.OverlapSphere(castPos, groundCastRadius);

			foreach (Collider col in cols)
			{
				if (col != null) return true;
			}
			return false;
		}

		public bool CanMove(Vector3 pos)
		{
			Vector3 posToCheck = new Vector3(pos.x, pos.y + blockCastHeight, pos.z );
			Collider[] cols = Physics.OverlapSphere(posToCheck, blockCastRadius, ~ignoreLayer);
			_castPosition = posToCheck;

			foreach (var col in cols)
			{
				if (col != null)
				{
					return false;
				}
			}
			return true;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(_castPosition, blockCastRadius);
		}
	}
}

