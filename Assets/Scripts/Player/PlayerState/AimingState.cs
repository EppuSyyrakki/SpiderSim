using UnityEngine;

namespace SpiderSim.Player.PlayerState
{
	public class AimingState : IPlayerState
	{
		private PlayerController _player;
		private Transform _body;

		public IPlayerState Update(PlayerInput input)
		{
			// TODO: Aim with CamHorizontal and CamVertical axises
			

			if (input.AimWeb == PlayerInput.Button.Up) 
			{
				return new MovingState();
			}

			if (input.ShootWeb == PlayerInput.Button.Down)
			{
				_player.webSource.ShootWeb();
			}

			return null;
		}

		public void OnStateEnter(PlayerController player)
		{
			Debug.Log("Entering aiming state");
			_player = player;
			_body = player.body;
		}

		public void OnStateExit(PlayerController player)
		{
			Debug.Log("Exiting aiming state");
		}
	}
}
