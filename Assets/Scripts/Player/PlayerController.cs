using System;
using System.Collections;
using System.Collections.Generic;
using SpiderSim.Player.PlayerState;
using UnityEngine;
using SpiderSim.Web;

namespace SpiderSim.Player
{
	[DefaultExecutionOrder(-1)]
	public class PlayerController : MonoBehaviour
	{
		private readonly PlayerInput _input = new PlayerInput();
		private IPlayerState _state;
		
		public Spider spider;
		public WebSource webSource;
		public SmoothCamera smoothCam;

		private void Awake()
		{
			webSource = GetComponentInChildren<WebSource>();
			// aimReticule.SetActive(false);
			// set Moving as the default state
			_state = new MovingState();
		}

		private void Start()
		{
			// setup the default state
			_state.OnStateEnter(this);
		}

		private void Update()
		{
			_input.Update();

			// Update member classes
			IPlayerState newState = _state.Update(_input);

			// If the state update resulted in a new state, set that as the current state.
			if (newState != null)
			{
				_state.OnStateExit(this);
				_state = newState;
				_state.OnStateEnter(this);
			}
		}

		private void FixedUpdate()
		{
			_state.FixedUpdate();
		}
		
		// Use this from the states to translate an Input to camera-relative movement
		public Vector3 TranslateInput() 
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
