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
		private Vector3 attachPoint = Vector3.zero;
		private float attachTolerance = 0.1f;

		[SerializeField]
		private LayerMask ignoreLayer;

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

                if (Physics.Linecast(_currentWeb.beginning, _currentWeb.end, out RaycastHit hit, gameObject.layer))
                {
	                if (Vector3.Distance(hit.point, attachPoint) < attachTolerance) return;

                    Debug.Log(hit.collider.gameObject.name);
                    Vector3 midPos = (hit.point - _currentWeb.end) / 2;
                    IPooledObject newFromPool = ObjectPooler.Instance.SpawnFromPool("Web", midPos, Quaternion.identity);
                    Web newWeb = newFromPool.GameObject().GetComponent<Web>();
                    newWeb.SetSource(this);
                    newWeb.SetupWeb(hit.point, _currentWeb.end, Quaternion.LookRotation(_currentWeb.end - hit.point), true);

                    _currentWeb.end = hit.point;
                    attachPoint = hit.point;
				}
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

		public void ShootWeb(RaycastHit hit)
		{
			if (_currentWeb != null) return;

			_target = hit.point;

			// if we hit a web, the hit point must be adjusted to the center of the collider, not surface.
			if (hit.collider.CompareTag(Names.Tags.web))
			{
				var col = hit.collider.gameObject.GetComponent<CapsuleCollider>();
				Vector3 dirToCenter = -hit.normal * col.radius;
				_target += dirToCenter;
			}

			Vector3 ownPos = transform.position;
			GameObject webObject = Instantiate(_player.webPrefab, transform.position, Quaternion.identity);
			_currentWeb = webObject.GetComponent<Web>();
			_currentWeb.SetSource(this);
			_currentWeb.SetupWeb(
				ownPos, 
				ownPos, 
				Quaternion.LookRotation(_target - ownPos));
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
