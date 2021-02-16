using System;
using UnityEngine;

namespace SpiderSim.Player.PlayerState
{
	public class MovingState : IPlayerState
	{
		private PlayerController _player;
		private Transform _body;
		
		public IPlayerState Update(PlayerInput input)
		{
			if (input.Move != Vector3.zero && GetGroundNormal(out var normal))
			{
				// must be negative because model is rotated 180 degrees
				Vector3 transformed = -_body.TransformVector(input.Move * _player.groundSpeed * Time.deltaTime);
				Vector3 offset = new Vector3(transformed.x, transformed.y, transformed.z);
				_body.position += offset;
				_body.up = normal;
			}
			
			// TODO: Lerp turn toward move direction

			if (input.AimWeb == PlayerInput.Button.Down)
			{
				return new AimingState();
			}

			// if player has pressed jump this frame, fall with jump
			if (input.Jump == PlayerInput.Button.Down)
			{
				return new FallingState(true);
			}

			return null;
		}

		private bool GetGroundNormal(out Vector3 normal)
		{
			normal = _body.position;
			// get the down direction relative to our rotation
			Vector3 direction = -_body.transform.up.normalized;
			Vector3 origin = normal - direction * _player.groundCastOffset;
			LayerMask ownLayer = _player.gameObject.layer;

#if UNITY_EDITOR
			// Debugging tools to see the Raycast
			_player.debugVectors[0] = origin;
			_player.debugVectors[1] = direction;
#endif

			if (Physics.Raycast(origin, direction, out var hit, _player.groundCastDist, ownLayer))
			{
				normal = hit.normal;
				return true;
			}

			return false;
		}

		private float GetBodyHeight()
		{
			float y = 0;

			foreach (var legTarget in _player.legTargets)
			{
				y += legTarget.transform.position.y;
			}

			return Mathf.Round(y / _player.legTargets.Count * 100f) / 100f;
		}

		public void OnStateEnter(PlayerController player)
		{
			Debug.Log("Entering moving state");
			_player = player;
			_body = player.body;
		}

		public void OnStateExit(PlayerController player)
		{
			Debug.Log("Exiting moving state");
		}
	}
}
