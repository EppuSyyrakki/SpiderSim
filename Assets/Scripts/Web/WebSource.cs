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

		private Web _currentWeb;
        private bool _hasFired;
        private Vector3 _target = Vector3.zero;
        private float _lerpT = 0;

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
            _lerpT += Time.deltaTime * webShootSpeed;
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
			GameObject webObject = Instantiate(webPrefab, transform.position, Quaternion.identity);
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
	}
}
