using UnityEngine;

namespace SpiderSim.Player.PlayerState
{
	public class AimingState : IPlayerState
	{
		private PlayerController _player;
		private Transform _body;
		private Vector3 _aimDir;
		private Ray _aim;
		private GameObject _reticule;
		private Camera _cam;

		public IPlayerState Update(PlayerInput input)
		{
			Vector3 target = Vector3.zero;
			_aimDir = GetAimDirection(input.Look);
			_aim = new Ray(_player.webSource.transform.position, _aimDir);

			// Raycast toward aiming direction and set target position if valid target found
			if (Physics.Raycast(_aim.origin, _aim.direction, out RaycastHit hit, _player.aimDistance))
			{
				_reticule.SetActive(true);
				target = hit.point;
				Vector3 ratio = _cam.WorldToViewportPoint(target);
				Vector3 screenPos = new Vector3(_cam.pixelWidth * ratio.x, _cam.pixelHeight * ratio.y);
				_reticule.transform.position = screenPos;
			}
			else
			{
				_player.aimReticule.SetActive(false);
			}

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
				_player.webSource.ShootWeb(target);
			}

			if (input.AttachWeb == PlayerInput.Button.Down)
			{
				_player.webSource.AttachCurrentWeb();
			}

			return null;
		}

		private Vector3 GetAimDirection(Vector3 look)
		{
			Vector3 lookEuler = new Vector3(-look.y, look.x);
			Vector3 newDir = Quaternion.Euler(lookEuler) * _aimDir;
			return (newDir * _player.aimRotSpeed * 100f * Time.deltaTime).normalized;
		}

		private void RotateBody(Vector3 normal)
		{
			_body.rotation = Quaternion.FromToRotation(_body.up, normal) * _body.rotation;
		}

		private void MoveBody(Vector3 movement, Vector3 normal)
		{
			// must be negative because model is rotated 180 degrees
			Vector3 transformed = -_body.TransformVector(movement * _player.aimGroundSpeed * Time.deltaTime);
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
			_aimDir = _player.RelativeForward;
			_reticule = _player.aimReticule;
			_cam = Camera.main;
		}

		public void OnStateExit(PlayerController player)
		{
			Debug.Log("Exiting aiming state");
			_reticule.SetActive(false);
		}
	}
}
