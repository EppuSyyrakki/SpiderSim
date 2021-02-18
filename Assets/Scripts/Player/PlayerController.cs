using System.Collections;
using System.Collections.Generic;
using SpiderSim.Player.PlayerState;
using UnityEngine;
using SpiderSim.Web;

namespace SpiderSim.Player
{
	public class PlayerController : MonoBehaviour
	{
		#region Private fields

		private readonly PlayerInput _input = new PlayerInput();
		private IPlayerState _state;
		
		#endregion
		#region Public fields

		public float groundSpeed = 4f, turnSpeed = 10f;
		public float groundCastDist = 1f, groundCastOffset = 0.5f;
		public float legCastDist = 1.2f, legCastOffset = 0.4f;
		public float sphereCastRadius = 1f, sphereCastOffset = 0.5f;
		public float jumpForce = 100f;
		[Range(0.1f, 1f)]
		public float stepDistance = 0.75f;
		[Range(0.05f, 0.5f)]
		public float stepFreq = 0.5f;
		[Range(0.01f, 1f)]
		public float stepArc = 1f;

		#endregion
		#region Public components
		
		[HideInInspector]
		public Rigidbody rb;
		[HideInInspector]
		public Transform body;
		[HideInInspector]
		public List<LegTarget> legTargets = new List<LegTarget>();
		[HideInInspector]
		public WebSource webSource;

		#endregion

#if UNITY_EDITOR
		#region Debugging tools

		// 1 = ground ray origin
		// 2 = ground ray direction
		// 3 = sphere cast origin
		[HideInInspector]
		public Vector3[] debugVectors = new Vector3[3];
		public bool showDebugGizmos;
		#endregion
#endif

		private void Awake()
		{
			rb = GetComponent<Rigidbody>();
			body = transform.GetChild(0);
			legTargets.AddRange(GetComponentsInChildren<LegTarget>());
			webSource = GetComponentInChildren<WebSource>();
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
			IPlayerState newState = _state.Update(_input);

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
						Gizmos.DrawRay(debugVectors[0], debugVectors[1] * groundCastDist);
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
