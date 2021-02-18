using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim.Player
{
	public class LegTarget : MonoBehaviour
	{
		private static PlayerController _player;

#if UNITY_EDITOR
		private Vector3[] debugVectors = new Vector3[2];
#endif

		// Start is called before the first frame update
		void Awake()
		{
			if (_player == null) _player = FindObjectOfType<PlayerController>();
			if (_player.showDebugGizmos) GetComponent<MeshRenderer>().enabled = true;
		}

		// Update is called once per frame
		void Update()
		{
			// Raycast in player's down direction to nearest surface
			Vector3 ground = GetGroundPoint(transform.position);

			if (ground != transform.position)
			{
				transform.position = ground;
			}
		}

		private Vector3 GetGroundPoint(Vector3 position)
		{
			Vector3 ground = position;
			// get the down direction relative to our rotation
			Vector3 direction = _player.RelativeDown;
			Vector3 origin = position - direction * _player.legCastOffset;
			LayerMask ownLayer = _player.gameObject.layer;

#if UNITY_EDITOR
			// Debugging tools to see the Raycast
			debugVectors[0] = origin;
			debugVectors[1] = direction;
#endif

			if (Physics.Raycast(origin, direction, out var hit, _player.legCastDist, ownLayer))
			{
				ground = hit.point;
			}

			return ground;
		}

		private void OnDrawGizmos()
		{
#if UNITY_EDITOR
			Gizmos.color = Color.red;
			if (_player != null && _player.showDebugGizmos)
			{
				Gizmos.DrawRay(debugVectors[0], debugVectors[1] * _player.legCastDist);
			}
#endif
		}
	}
}
