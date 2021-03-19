using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim.Player
{
	public class NinjaRope : MonoBehaviour
	{
		private PlayerController _player;
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
			Vector3 ownPos = transform.position;
			GameObject webObject = Instantiate(_player.webPrefab, transform.position, Quaternion.identity);
			_currentWeb = webObject.GetComponent<Web>();
			_currentWeb.SetSource(this);
			_currentWeb.SetupWeb(
				ownPos, 
				ownPos, 
				Quaternion.LookRotation(target - ownPos));
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
			newWeb.SetupWeb(_currentWeb.beginning, _currentWeb.end, Quaternion.LookRotation(_currentWeb.end - _currentWeb.beginning), true);

			// TODO: lerp the new web to relative -y direction from transform.position (attach to ground)
			// TODO: this could be done as a coroutine in Web.cs

			Destroy(_currentWeb.gameObject);
		}
	}
}
