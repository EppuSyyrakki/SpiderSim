using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim
{
	public class CameraFollower : MonoBehaviour
	{
		[SerializeField]
		private Transform target;

		[SerializeField]
		private Vector3 offsetFromTarget = new Vector3(0, 3, -3);

		[SerializeField]
		private Vector3 lookOffset = Vector3.up;

		[SerializeField]
		private float eventSpeed;

		[SerializeField]
		private float moveSpeed = 0.5f;

		private float _eventTime;

		private void Start()
		{
			_eventTime = Time.time;
		}

		// Update is called once per frame
		void Update()
		{
			float eventRatio = (Time.time - _eventTime) / eventSpeed;
			Vector3 targetPos = target.position + offsetFromTarget;
			transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed);
			Vector3 lookTarget = target.position + lookOffset;
			transform.LookAt(lookTarget);

			if (eventRatio > 1f) _eventTime = Time.time;
		}
	}
}
