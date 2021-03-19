using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim.Player
{
	public class NinjaRope : MonoBehaviour
	{
		private PlayerController _player;
		private Vector3 _aimDir;
		private Ray _aim;
		private Web _currentWeb;
		private bool _hasFired;
		private Vector3 _target = Vector3.zero;
		private float _lerpT = 0;

		private void Awake()
		{
			_player = transform.parent.gameObject.GetComponent<PlayerController>();
		}

		private void Update()
		{
			if (_hasFired)
			{
				LerpShot();
			}

			if (_currentWeb != null)
			{
				_currentWeb.beginning = transform.position;
			}
		}

		private void LerpShot()
		{
			_lerpT += Time.deltaTime * _player.webShotSpeed;
			_currentWeb.end = Vector3.Lerp(_currentWeb.beginning, _target, _lerpT);

			if (_lerpT >= 1)
			{
				_hasFired = false;
			}
		}

		public void ShootWeb(Vector3 target)
		{
			if (_currentWeb != null) return;

			_target = target;
			GameObject webObject = Instantiate(_player.webPrefab, transform.position, Quaternion.identity);
			_currentWeb = webObject.GetComponent<Web>();
			_currentWeb.SetSource(this);
			_currentWeb.SetupWeb(transform.position, transform.position);
			_hasFired = true;
			_lerpT = 0;
		}

		public void AttachCurrentWeb()
		{
			if (_currentWeb == null) return;

			_currentWeb.attached = true;
			IPooledObject newFromPool = ObjectPooler.Instance.SpawnFromPool("Web", _currentWeb.beginning, Quaternion.identity);
			Web newWeb = newFromPool.GameObject().GetComponent<Web>();
			newWeb.SetSource(this);
			newWeb.SetupWeb(_currentWeb.beginning, _currentWeb.end, true);
			Destroy(_currentWeb.gameObject);
		}

		public bool Aim(PlayerInput input, out Vector3 target)
		{
			target = Vector3.zero;
			_aimDir = GetAimDirection(input.Look);
			_aim = new Ray(_player.ninjaRope.transform.position, _aimDir);
			Debug.DrawRay(transform.position, _aimDir);

			// Raycast toward aiming direction and set target position if valid target found 
			if (Physics.Raycast(_aim.origin, _aim.direction, out RaycastHit hit, _player.aimDistance, _player.ignoreLayer))
			{
				Debug.Log("Found a " + hit.collider.gameObject.name);
				target = hit.point;
				return true;
			}

			return false;
		}

		private Vector3 GetAimDirection(Vector3 look)
		{
			Vector3 lookEuler = new Vector3(-look.y, look.x);
			Vector3 newDir = Quaternion.Euler(lookEuler) * _aimDir;

			return (newDir * _player.aimRotSpeed * 100f * Time.deltaTime).normalized;
		}
	}
}
