using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim.Player
{
	public class LegTarget : MonoBehaviour
	{
		private PlayerController player;
		private Vector3[] debugVectors = new Vector3[2];
		
		// Start is called before the first frame update
		void Awake()
		{
			player = FindObjectOfType<PlayerController>();
		}

		// Update is called once per frame
		void Update()
		{
			// Raycast in player's down direction to nearest surface
			Vector3 ground = GetGroundPoint();

			if (ground != transform.position)
			{
				transform.position = ground;
			}
		}

		private Vector3 GetGroundPoint()
		{
			Vector3 ground = transform.position;
			// get the down direction relative to our rotation
			Vector3 direction = -player.transform.up.normalized;
			Vector3 origin = transform.position - direction * player.groundRayOffset;
			LayerMask ownLayer = player.gameObject.layer;

			// Debugging tools to see the Raycast
			debugVectors[0] = origin;
			debugVectors[1] = direction;

			if (Physics.Raycast(origin, direction, out var hit, player.groundRayDist, ownLayer))
			{
				ground = hit.point;
			}

			return ground;
		}

		private void OnDrawGizmos()
		{
#if UNITY_EDITOR
			Gizmos.color = Color.red;
			if (player != null && player.showDebugGizmos)
			{
				Gizmos.DrawRay(debugVectors[0], debugVectors[1] * player.groundRayDist);
			}
#endif
		}
	}
}
