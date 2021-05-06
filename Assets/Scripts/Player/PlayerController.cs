using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

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
		public float aimZoneX = 0.2f, aimZoneY = 0.25f;
		public float webShotSpeed = 10f;
		// public LayerMask ignoreLayer;
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

			if (_input.ShootWeb == PlayerInput.Button.Down && ninjaRope.HasCurrentWeb)
			{
				ninjaRope.AttachCurrentWeb();
			}

			if (_input.Jump == PlayerInput.Button.Down)
			{
				spider.Jump();
			}

			if (_input.AimWeb == PlayerInput.Button.Down)
			{
				AimingMode(true);
			}
			else if (_input.AimWeb == PlayerInput.Button.Up)
			{
				AimingMode(false);
			}
			else if (_input.AimWeb == PlayerInput.Button.Stay)
			{
				Aim();
				if (_input.ShootWeb == PlayerInput.Button.Down)
				{
					Shoot();
				}
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

		/// <summary>
		/// Moves the aiming reticule on the GUI according to input
		/// </summary>
		private void Aim()
		{
			if (_input.Look != Vector3.zero)
			{
				Camera c = smoothCam.Cam;
				Vector3 pos = _reticule.transform.position;
				Vector3 newPos = new Vector3(
					pos.x + _input.Look.x * aimRotSpeed * Time.deltaTime,
					pos.y + _input.Look.y * aimRotSpeed * Time.deltaTime);
				Vector3 screenPos = GetReticuleScreenPosition();

				// Restrict reticule position to a relative distance from middle of screen
				if (Mathf.Abs(screenPos.x - 0.5f) < aimZoneX && Mathf.Abs(screenPos.y - 0.5f) < aimZoneY)
				{
					_reticule.transform.position = newPos;
				}
			}
		}

		/// <summary>
		/// Casts a ray from camera towards the aiming reticule. If object found, shoots the web.
		/// </summary>
		private void Shoot()
		{
			Ray ray = smoothCam.Cam.ScreenPointToRay(_reticule.transform.position);

			if (Physics.Raycast(ray, out RaycastHit hit))
			{
				ninjaRope.ShootWeb(hit);
			}

			// TODO if no hit from raycast, shoot into empty space
		}

		/// <summary>
		/// Creates a Vector3 that has x and y values between 0 and 1 to indicate a position on the GUI
		/// from left to right and bottom to top.
		/// </summary>
		/// <returns>The relative position as a Vector3</returns>
		private Vector3 GetReticuleScreenPosition()
		{
			Camera c = smoothCam.Cam;
			Vector3 retPos = _reticule.transform.position;
			return new Vector3(retPos.x / c.pixelWidth, retPos.y / c.pixelHeight);
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
