using UnityEngine;

namespace SpiderSim.Player.PlayerState
{
	public class AimingState : IPlayerState
	{
		private PlayerController _player;
		private Transform _body;

		public IPlayerState Update(PlayerInput input)
		{
			if (input.Move != Vector3.zero)
			{
				Vector3 normal = GetGroundNormal();
				RotateBody(normal);
				MoveBody(input.Move, normal);
			}

			if (input.AimWeb == PlayerInput.Button.Up) 
			{
				return new MovingState();
			}

			if (input.ShootWeb == PlayerInput.Button.Down)
			{
				_player.webSource.ShootWeb();
			}

			return null;
		}

		private void RotateBody(Vector3 normal)
		{
			_body.rotation = Quaternion.FromToRotation(_body.up, normal) * _body.rotation;
		}

		private void MoveBody(Vector3 movement, Vector3 normal)
		{
			// must be negative because model is rotated 180 degrees
			Vector3 transformed = -_body.TransformVector(movement * _player.aimSpeedRatio * Time.deltaTime);
			Vector3 offset = Vector3.ProjectOnPlane(transformed, normal);
			_body.position += offset;
		}

		private Vector3 GetGroundNormal()
		{
			Vector3 normal = _body.position;
			// get the down direction relative to our rotation
			Vector3 direction = -_body.transform.up.normalized;
			Vector3 origin = normal - direction * _player.groundCastOffset;
			LayerMask ownLayer = _player.gameObject.layer;

			if (Physics.Raycast(origin, direction, out var hit, _player.groundCastDist, ownLayer))
			{
				normal = hit.normal;
			}

			return normal;
		}

		public void OnStateEnter(PlayerController player)
		{
			Debug.Log("Entering aiming state");
			_player = player;
			_body = player.body;
		}

		public void OnStateExit(PlayerController player)
		{
			Debug.Log("Exiting aiming state");
		}
	}
}
