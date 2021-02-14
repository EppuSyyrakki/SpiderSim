using System.Collections;
using System.Collections.Generic;
using SpiderSim.Player.PlayerState;
using UnityEngine;

namespace SpiderSim.Player
{
	public class PlayerController : MonoBehaviour
	{
		private readonly PlayerInput _input = new PlayerInput();
		private IPlayerState _state;
		
		public float groundSpeed = 100f, turnSpeed = 10f;
		public float groundRayDist = 1f, groundRayOffset = 0.5f;
		public float sphereCastRadius = 2f, sphereCastOffset = 0.75f;
		public float jumpForce = 100f;
		public float stepDistance = 0.75f;
		public bool showDebugGizmos;

		[HideInInspector]
		public Rigidbody rb;
		[HideInInspector]
		public Transform spider;

		// 1 = ground ray origin
		// 2 = ground ray direction
		// 3 = sphere cast origin
		[HideInInspector]
		public Vector3[] debugVectors = new Vector3[3];

		private void Awake()
		{
			rb = GetComponent<Rigidbody>();
			spider = transform.GetChild(0);
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
			// Update member classes
			_input.Update();
			IPlayerState newState = _state.Update(this, _input);

			// If the state update resulted in a new state, set that as the current state.
			if (newState != null)
			{
				_state.OnStateExit(this);
				_state = newState;
				_state.OnStateEnter(this);
			}
		}

		private void OnDrawGizmos()
		{
#if UNITY_EDITOR
			if (showDebugGizmos)
			{
				Gizmos.color = Color.red;
				string stateName = "";

				if (_state != null)
				{
					stateName = _state.GetType().Name;
				}
				switch (stateName)
				{
					case "MovingState":
						Gizmos.DrawRay(debugVectors[0], debugVectors[1] * groundRayDist);
						break;
					case "FallingState":
						Gizmos.DrawWireSphere(debugVectors[2], sphereCastRadius);
						break;
				}
			}
#endif
		}
	}
}
