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

		private Camera cam;

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
			
		}

		public void ShootWeb()
		{
			Vector3 target = Vector3.zero;

			if (GetAttachSource(out target))
			{
				Debug.DrawLine(transform.position, target, Color.white, 1f);
				
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
	}
}
