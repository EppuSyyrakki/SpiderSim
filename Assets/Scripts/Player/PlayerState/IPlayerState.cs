using UnityEngine;

namespace SpiderSim.Player.PlayerState
{
	public interface IPlayerState
	{
		public IPlayerState Update(PlayerInput input);
		public void OnStateEnter(PlayerController player);
		public void OnStateExit(PlayerController player);
	}
}

