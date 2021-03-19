using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim.Player
{
	[DefaultExecutionOrder(-1)]
	public class PlayerController : MonoBehaviour
	{
		private readonly PlayerInput _input = new PlayerInput();

		[Header("External objects")]
		public Spider spider;
		public NinjaRope ninjaRope;
		public SmoothCamera smoothCam;

		[Header("Aiming & Shooting")]
		public float aimDistance = 100f;
		public float aimRotSpeed = 10f;
		[Range(0.1f, 0.5f)]
		public float aimDeadZone = 0.25f;
		public float webShotSpeed = 10f;
		public LayerMask ignoreLayer;
		public GameObject webPrefab;

		[Range(0, 0.33f)]
		public float reticuleYOffset = 0.2f;

		[SerializeField]
		private GameObject _reticule;
		
		private void Awake()
		{
			ninjaRope = GetComponentInChildren<NinjaRope>();
			spider.setGroundcheck(true);
		}

		private void Update()
		{
			_input.Update();

			if (_input.Jump == PlayerInput.Button.Down)
			{
				spider.Jump();
			}

			if (_input.AimWeb == PlayerInput.Button.Down)
			{
				AimingMode(true);
			}
			else if (_input.AimWeb == PlayerInput.Button.Stay)
			{
				Aim();
			}
			else if (_input.AimWeb == PlayerInput.Button.Up)
			{
				AimingMode(false);
			}
		}

		/// <summary>
		/// Disables or enables the aiming reticule and moves it to center of screen with an Y offset.
		/// </summary>
		/// <param name="enabled">Enable or disable the reticule</param>
		private void AimingMode(bool enabled)
		{
			_reticule.SetActive(enabled);

			if (enabled)
			{
				Camera c = smoothCam.Cam;
				Vector3 relativePos = new Vector3(0.5f, 0.5f + reticuleYOffset);
				Vector3 screenPos = new Vector3(c.pixelWidth * relativePos.x, c.pixelHeight * relativePos.y);
				_reticule.transform.position = screenPos;
			}
		}

		private void Aim()
		{
			if (_input.Look != Vector3.zero)
			{

			}
		}

		private Vector3 GetViewportPoint(Vector3 target)
		{
			Camera c = smoothCam.Cam;
			Vector3 ratio = c.WorldToViewportPoint(target);
			return new Vector3(c.pixelWidth * ratio.x, c.pixelHeight * ratio.y);
		}

		private Vector3 GetWorldPoint(Vector3 screenPos)
		{
			Camera c = smoothCam.Cam;
			Vector3 ratio = c.ViewportToWorldPoint(screenPos);
			return new Vector3(c.pixelWidth * ratio.x, c.pixelHeight * ratio.y);
		}

		private void FixedUpdate()
		{
			// Translate input into camera-relative movement and move the spider
			Vector3 relativeInput = TranslateInput();
			float speed = spider.speed * relativeInput.magnitude;
			spider.Move(relativeInput, speed);

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
