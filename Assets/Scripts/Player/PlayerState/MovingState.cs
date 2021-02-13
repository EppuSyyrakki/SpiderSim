using UnityEngine;

namespace SpiderSim.Player.PlayerState
{
	public class MovingState : IPlayerState
	{
		public IPlayerState Update(PlayerController player, PlayerInput input)
		{
			
			bool grounded = false;
			Transform self = player.transform;

			// if we can find ground, align our up to ground's normal
			if (GetGroundNormal(player, out Vector3 normal))
			{
				// TODO recalculate distance from ground
				// TODO Lerp this rotation
				self.up = normal;
				grounded = true;
			}

			// if player has pressed jump this frame, fall with jump
			if (input.Jump == PlayerInput.Button.Down)
			{
				return new FallingState(true);
			}

			// if we can't find ground, fall without jumping
			if (!grounded)
			{
				return new FallingState();
			}

			if (input.Move != Vector3.zero)
			{
				Vector3 moveOffset = self.TransformVector(input.Move * player.groundSpeed * Time.deltaTime);
				self.position += moveOffset;
			}
			
			// TODO: Lerp turn toward move direction

			if (input.AimWeb == PlayerInput.Button.Down)
			{
				return new AimingState();
			}

			return null;
		}

		private bool GetGroundNormal(PlayerController player, out Vector3 normal)
		{
			Transform self = player.transform;
			Vector3 origin = self.TransformPoint(self.up * player.groundRayOffset);
			Vector3 direction = -self.up.normalized;
			LayerMask ownLayer = player.gameObject.layer;
			normal = Vector3.positiveInfinity;

			// Debugging tools to see the Raycast
			player.debugVectors[0] = origin;
			player.debugVectors[1] = direction;

			if (Physics.Raycast(origin, direction, out var hit, player.groundRayDist, ownLayer))
			{
				Debug.Log("Walking on " + hit.transform.gameObject.name);
				normal = hit.normal;
				return true;
			}

			return false;
		}

		public void OnStateEnter(PlayerController player)
		{
			Debug.Log("Entering moving state");
		}

		public void OnStateExit(PlayerController player)
		{
			Debug.Log("Exiting moving state");
		}
	}
}
