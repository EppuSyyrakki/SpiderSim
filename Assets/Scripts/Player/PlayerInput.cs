using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim.Player
{
	public class PlayerInput
	{
		public enum Button
		{
			None = 0,
			Down,
			Stay,
			Up
		}

		/// <summary>
		/// The movement vector of the player (left stick / WASD).
		/// </summary>
		public Vector3 Move { get; private set; }

		/// <summary>
		/// The look (camera) vector (right stick / mouse).
		/// </summary>
		public Vector3 Look { get; private set; }

		/// <summary>
		/// True if the respective button was pressed on update.
		/// </summary>
		public Button AimWeb { get; private set; }
		public Button ShootWeb { get; private set; }
		public Button AttachWeb { get; private set; }
		public Button CancelWeb { get; private set; }
		public Button Jump { get; private set; }

		public PlayerInput()
		{
			Move = new Vector3();
			Look = new Vector3();
		}

		public void Update()
		{
			Move = GetMoveInput();
			Look = GetLookInput();
			AimWeb = GetButtonInput("AimWeb");
			ShootWeb = GetButtonInput("ShootWeb");
			AttachWeb = GetButtonInput("AttachWeb");
			CancelWeb = GetButtonInput("CancelWeb");
			Jump = GetButtonInput("Jump");
		}

		private static Vector3 GetMoveInput()
		{
			return new Vector3
			{
				x = Input.GetAxis(Names.Input.horizontal),
				z = Input.GetAxis(Names.Input.vertical)
			};
		}

		private static Vector3 GetLookInput()
		{
			return new Vector3
			{
				x = Input.GetAxis("CamHorizontal"),
				y = Input.GetAxis("CamVertical")
			};
		}

		private static Button GetButtonInput(string buttonName)
		{
			if (Input.GetButtonDown(buttonName)) return Button.Down;
			if (Input.GetButtonUp(buttonName)) return Button.Up;
			if (Input.GetButton(buttonName)) return Button.Stay;
			return Button.None;
		}
	}
}
