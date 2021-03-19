using System.Collections;
using System.Collections.Generic;
using SpiderSim.Game;
using UnityEngine;

namespace SpiderSim.Game
{
	public class InGameState : GameState
	{
		private bool enterMenu = false;

		public InGameState()
		{
			SetAssociatedScene(Names.Scenes.Kitchen);
		}

		public override GameState Update()
		{
			if (enterMenu)
			{
				return new MenuState();
			}

			return null;
		}

		public override void OnStateEnter()
		{
			base.OnStateEnter();
		}

		public override void OnStateExit()
		{
			base.OnStateExit();
		}

		public void ToMenu()
		{
			enterMenu = true;
		}
	}
}
