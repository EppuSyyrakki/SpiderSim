using System;
using System.Collections;
using System.Collections.Generic;
using DitzelGames.FastIK;
using UnityEngine;

namespace SpiderSim.Player
{
	public class Leg : MonoBehaviour
	{
		[SerializeField, Tooltip("The next target object that moves with the spider body")]
		private Transform futureTarget;
		
		private Transform _currentTarget;
		private FastIKFabric _fastIK;
		private PlayerController _player;
		private Vector3 _previousTarget, _nextTarget;
		private Vector3 _stepCenter, _prevRelCenter, _nextRelCenter;
		private float _stepStartTime;

		// Vectors for drawing the debug targets
		private Vector3[] debugVectors = new Vector3[2];
		
		// fields to determine step order
		[HideInInspector]
		public bool canMove;
		private bool _isMoving;
		private Leg oppositeLeg;

		private void Start()
		{
			_fastIK = GetComponent<FastIKFabric>();
			_previousTarget = transform.position;
			_currentTarget = _fastIK.Target;
			_nextTarget = _currentTarget.position;
			_player = FindObjectOfType<PlayerController>();
			InitStepOrder();
		}

		private void Update()
		{
			float distanceToTarget = Vector3.Distance(_currentTarget.position, futureTarget.position);
			
			if (distanceToTarget > _player.stepDistance && canMove)
			{
				AssignStepTargets();
				_isMoving = true;
			}

			if (_isMoving)
			{
				canMove = false;
				float slerpT = (Time.time - _stepStartTime) / _player.stepFreq;
				_currentTarget.position = Vector3.Slerp(_prevRelCenter, _nextRelCenter, slerpT) +_stepCenter;

				if (_currentTarget.position == _nextTarget)
				{
					oppositeLeg.canMove = true;
					_isMoving = false;
				}
			}

#if UNITY_EDITOR
			if (_player != null && _player.showDebugGizmos)
			{
				debugVectors[0] = _currentTarget.position;
				debugVectors[1] = futureTarget.position;
			}
#endif
		}

		private void AssignStepTargets()
		{
			_stepStartTime = Time.time;
			_previousTarget = _currentTarget.position;
			_nextTarget = futureTarget.position;
			_stepCenter = (_previousTarget + _nextTarget) * 0.5f;
			_stepCenter -= _player.transform.up.normalized * _player.stepArc;
			_prevRelCenter = _previousTarget - _stepCenter;
			_nextRelCenter = _nextTarget - _stepCenter;
		}

		private void InitStepOrder()
		{
			string name = gameObject.name;

			if (name.Contains(".R_end"))
			{
				if (name.Contains("2.3") || name.Contains("4.3")) canMove = true;

				FindOppositeLeg(".L");
			}
			else if (name.Contains(".L_end"))
			{
				if (name.Contains("1.3") || name.Contains("3.3")) canMove = true;

				FindOppositeLeg(".R");
			}
		}

		private void FindOppositeLeg(string oppositeSide)
		{
			string str = gameObject.name;
			int index = str.LastIndexOf('.');
			string oppositeName = str.Substring(0, index) + oppositeSide + str.Substring(index + 2);
			oppositeLeg = GameObject.Find(oppositeName).GetComponent<Leg>();
		}

		private void OnDrawGizmos()
		{
#if UNITY_EDITOR
			if (_player != null && _player.showDebugGizmos)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine(debugVectors[0], debugVectors[1]);
			}
#endif
		}
	}
}
