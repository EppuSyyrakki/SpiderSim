using UnityEditor.UI;
using UnityEngine;

namespace SpiderSim.Player.PlayerState
{
	public class FallingState : IPlayerState
	{
		private PlayerController _player;
		private Transform _body;
		private bool _jumped;

		public FallingState(bool jumped = false)
		{
			_jumped = jumped;
		}

		public IPlayerState Update(PlayerInput input)
		{
			Vector3 origin = _body.TransformPoint(_body.up * _player.sphereCastOffset);
			LayerMask ownLayer = _player.gameObject.layer;
#if UNITY_EDITOR
			_player.debugVectors[2] = origin;
#endif
			Collider[] cols = Physics.OverlapSphere(origin, _player.sphereCastRadius, ownLayer);

			foreach (var collider in cols)
			{
				// TODO detect a surface
			}
			
			return null;
		}

		public void OnStateEnter(PlayerController player)
		{
			Debug.Log("Entering falling state");
			_player = player;
			_body = player.body;
			_player.rb.useGravity = true;
			_player.rb.isKinematic = false;

			if (_jumped)
			{
				Vector3 jumpDirection = _body.transform.TransformDirection(_body.transform.up);
				_player.rb.AddForce(jumpDirection * _player.jumpForce);
			}
		}

		public void OnStateExit(PlayerController player)
		{
			player.rb.useGravity = false;
			player.rb.isKinematic = true;
			Debug.Log("Exiting falling state");
		}
	}
}
