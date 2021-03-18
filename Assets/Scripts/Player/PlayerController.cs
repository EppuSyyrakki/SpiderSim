using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpiderSim.Web;

namespace SpiderSim.Player
{
	[DefaultExecutionOrder(-1)]
	public class PlayerController : MonoBehaviour
	{
		private readonly PlayerInput _input = new PlayerInput();
		private bool jumped = false;

		public Spider spider;
		public WebSource webSource;
		public SmoothCamera smoothCam;

		private void Awake()
		{
			webSource = GetComponentInChildren<WebSource>();
			spider.setGroundcheck(true);
		}

		private void Update()
		{
			_input.Update();

			if (_input.Jump == PlayerInput.Button.Down)
			{
				jumped = true;
			}
		}

		private void FixedUpdate()
		{
			// Translate input into camera-relative movement and move the spider
			Vector3 relativeInput = TranslateInput();
			float speed = spider.speed * relativeInput.magnitude;
			spider.Move(relativeInput, speed);

			if (jumped)
			{
				spider.Jump();
				jumped = false;
			}

			// Check the camera target rotation and position
			Quaternion tempCamTargetRotation = smoothCam.getCamTargetRotation();
			Vector3 tempCamTargetPosition = smoothCam.getCamTargetPosition();

			// Turn the spider and set camera position and rotation
			spider.Turn(relativeInput);
			smoothCam.setTargetRotation(tempCamTargetRotation);
			smoothCam.setTargetPosition(tempCamTargetPosition);
		}

		/// <summary>
		/// Translates input into vectors projected relative to spider's body transform.
		/// </summary>
		/// <returns>The translated Vector3</returns>
		private Vector3 TranslateInput() 
		{
	        Vector3 up = spider.transform.up;
	        Vector3 right = spider.transform.right;
	        Vector3 input = 
		        Vector3.ProjectOnPlane(smoothCam.getCameraTarget().forward, up).normalized * _input.Move.z 
		        + (Vector3.ProjectOnPlane(smoothCam.getCameraTarget().right, up).normalized * _input.Move.x);
	        Quaternion fromTo = Quaternion.AngleAxis(Vector3.SignedAngle(up, spider.getGroundNormal(), right), right);
	        input = fromTo * input;
	        float magnitude = input.magnitude;
	        return (magnitude <= 1) ? input : input /= magnitude;
		}
	}
}
