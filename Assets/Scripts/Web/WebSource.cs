using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SpiderSim.Web
{
	public class WebSource : MonoBehaviour
	{
		[SerializeField]
		private GameObject webPrefab;
		[SerializeField]
		private float webShootSpeed = 10f;

		private Web _web;
        private bool _hasFired;
        private Vector3 _target;
		private Camera _cam;
		private float _lerpT = 0;

		public bool HasActiveWeb => _web != null;
		
		private void Awake()
		{
			_cam = Camera.main;
		}

		private void Update()
		{
			if (_hasFired)
			{
				LerpShot();
			}
		}

		private void LerpShot()
        {
            _lerpT += Time.deltaTime * webShootSpeed;
            _web.end = Vector3.Lerp(_web.beginning, _target, _lerpT);

			Debug.Log(_lerpT);

            if (_lerpT >= 1)
            {
                _hasFired = false;
            }
        }

		public void ShootWeb()
		{
			if (_web != null) return;
			_target = Vector3.zero;

			if (GetAttachSource(out _target))
			{
				Debug.DrawLine(transform.position, _target, Color.white, 1f);
                GameObject web = Instantiate(webPrefab, transform.position, Quaternion.identity);
                _web = web.GetComponent<Web>();
				_web.SetSource(this);
                _hasFired = true;
                _lerpT = 0;
            }
			else
			{
				// TODO: shoot into empty space
			}
		}

		public bool GetAttachSource(out Vector3 target)
		{
			target = Vector3.zero;

			if (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
			{
				target = hit.point;

				if (Physics.Linecast(transform.position, target, out RaycastHit blocking))
				{
					target = blocking.point;
				}
			}

			return target != Vector3.zero;
		}

        public void AttachCurrentWeb()
        {
	        if (_web == null) return;
            _web.attached = true;
	        _web = null;
		}
	}
}
