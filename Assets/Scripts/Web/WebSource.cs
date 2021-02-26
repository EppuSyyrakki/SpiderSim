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

		private Web _webScript;
        private bool _hasFired;
        private Vector3 _target = Vector3.zero;
        private float _lerpT = 0;

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
            _webScript.end = Vector3.Lerp(_webScript.beginning, _target, _lerpT);

            if (_lerpT >= 1)
            {
                _hasFired = false;
            }
        }

		public void ShootWeb(Vector3 target)
		{
			if (_webScript != null) return;

			_target = target;
			GameObject web = Instantiate(webPrefab, transform.position, Quaternion.identity);
            _webScript = web.GetComponent<Web>();
			_webScript.SetSource(this);
            _hasFired = true;
            _lerpT = 0;
		}

		public void AttachCurrentWeb()
        {
	        if (_webScript == null) return;
            _webScript.attached = true;
            ObjectPooler.Instance.SpawnFromPool("Web", transform.position, Quaternion.identity);
            Destroy(_webScript.gameObject);
		}
	}
}
