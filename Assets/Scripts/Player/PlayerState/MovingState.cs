using System;
using UnityEngine;

namespace SpiderSim.Player.PlayerState
{
	public class MovingState : IPlayerState
	{
		private PlayerController _player;
		private Spider _spider;
		
		public IPlayerState Update(PlayerInput input)
		{
			_spider.setGroundcheck(!Input.GetKey(KeyCode.Space));

			if (input.Jump == PlayerInput.Button.Down)
			{
				return new FallingState();
			}
			
            return null;
		}

		public void FixedUpdate()
		{
			// Translate input into camera-relative movement and move the spider
			Vector3 input = _player.TranslateInput();
			float speed = _spider.speed * input.magnitude;
			_spider.Move(input, speed);
			Debug.Log(speed);

			// Check the camera target rotation and position
			Quaternion tempCamTargetRotation = _player.smoothCam.getCamTargetRotation();
			Vector3 tempCamTargetPosition = _player.smoothCam.getCamTargetPosition();

			// Turn the spider and set camera position and rotation
			_player.spider.turn(input);
			_player.smoothCam.setTargetRotation(tempCamTargetRotation);
			_player.smoothCam.setTargetPosition(tempCamTargetPosition);
		}

		public void OnStateEnter(PlayerController player)
		{
			Debug.Log("Entering moving state");
			_player = player;
			_spider = player.spider;
		}

		public void OnStateExit(PlayerController player)
		{
			Debug.Log("Exiting moving state");
		}
	}
}
