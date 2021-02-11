using UnityEditor.UI;
using UnityEngine;

namespace SpiderSim.Player.PlayerState
{
	public class FallingState : IPlayerState
	{
		private bool _jumped;

		public FallingState(bool jumped = false)
		{
			_jumped = jumped;
		}

		public IPlayerState Update(PlayerController player, PlayerInput input)
		{
			Transform self = player.transform;
			Vector3 origin = self.TransformPoint(self.up * player.sphereCastOffset);
			LayerMask ownLayer = player.gameObject.layer;
			player.debugVectors[2] = origin;
			Collider[] cols = Physics.OverlapSphere(origin, player.sphereCastRadius, ownLayer);

			foreach (var collider in cols)
			{
				// TODO detect a surface
			}
			
			return null;
		}

		public void OnStateEnter(PlayerController player)
		{
			player.rb.useGravity = true;
			player.rb.isKinematic = false;

			if (_jumped)
			{
				Vector3 jumpDirection = player.transform.TransformDirection(player.transform.up);
				player.rb.AddForce(jumpDirection * player.jumpForce);
			}
			Debug.Log("Entering falling state");
		}

		public void OnStateExit(PlayerController player)
		{
			player.rb.useGravity = false;
			player.rb.isKinematic = true;
			Debug.Log("Exiting falling state");
		}
	}
}
