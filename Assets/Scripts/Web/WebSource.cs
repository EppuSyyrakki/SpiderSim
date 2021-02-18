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

		private Web _web;
        private bool hasFired;

        private Vector3 target;

		private Camera cam;

        [SerializeField] private float webShootSpeed = 10f;
        private float lerpT = 0;

		private void Awake()
		{
			cam = Camera.main;
		}

		private void Update()
		{
			if (hasFired)
			{
				LerpShot();
			}
		}

		private void LerpShot()
        {
            lerpT += Time.deltaTime * webShootSpeed;
            _web.end = Vector3.Lerp(_web.beginning, target, lerpT);

			Debug.Log(lerpT);

            if (lerpT >= 1)
            {
                hasFired = false;
            }
        }

		public void ShootWeb()
		{
			target = Vector3.zero;

			if (GetAttachSource(out target))
			{
				Debug.DrawLine(transform.position, target, Color.white, 1f);
                GameObject web = Instantiate(webPrefab, transform.position, Quaternion.identity);
                _web = web.GetComponent<Web>();
				_web.SetSource(this);
                hasFired = true;
                lerpT = 0;
            }
			else
			{
				// TODO: shoot into empty space
			}
		}

		public bool GetAttachSource(out Vector3 target)
		{
			target = Vector3.zero;

			if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
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
            if (_web != null)
            {
                _web.attached = true;
                _web = null;
            }
        }
	}
}
