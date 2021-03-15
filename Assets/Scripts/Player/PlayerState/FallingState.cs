using UnityEditor.UI;
using UnityEngine;

namespace SpiderSim.Player.PlayerState
{
	public class FallingState : IPlayerState
	{
		private PlayerController _player;
		private Spider _spider;

		public IPlayerState Update(PlayerInput input)
		{
			if (input.Jump == PlayerInput.Button.Up)
			{
				return new MovingState();
			}

			return null;
		}

		public void FixedUpdate()
		{
			return;
		}

		public void OnStateEnter(PlayerController player)
		{
			Debug.Log("Entering falling state");
			_player = player;
			_spider = player.spider;
			_spider.setGroundcheck(false);
		}

		public void OnStateExit(PlayerController player)
		{
			Debug.Log("Exiting falling state");
		}
	}
}
