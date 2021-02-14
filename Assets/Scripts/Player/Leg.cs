using System;
using System.Collections;
using System.Collections.Generic;
using DitzelGames.FastIK;
using UnityEngine;

namespace SpiderSim.Player
{
	public class Leg : MonoBehaviour
	{
		[SerializeField]
		private Transform currentTarget, futureTarget;

		private FastIKFabric fastIK;
		private PlayerController _player;
		private Vector3[] debugVectors = new Vector3[2];

		private void Start()
		{
			fastIK = GetComponent<FastIKFabric>();
			currentTarget = fastIK.Target;
			_player = FindObjectOfType<PlayerController>();
		}

		private void Update()
		{
			if (Vector3.Distance(currentTarget.position, futureTarget.position) > _player.stepDistance)
			{
				currentTarget.position = futureTarget.position;
			}

			if (_player != null && _player.showDebugGizmos)
			{
				debugVectors[0] = currentTarget.position;
				debugVectors[1] = futureTarget.position;
			}
		}

		private void OnDrawGizmos()
		{
#if UNITY_EDITOR
			if (_player != null && _player.showDebugGizmos)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine(debugVectors[0], debugVectors[1]);
			}
#endif
		}
	}
}
