using System.Collections;
using System.Collections.Generic;
using SpiderSim.Game;
using UnityEngine;

namespace SpiderSim.Game
{
	public class MenuState : GameState
	{
		private bool enterGame = false;

		public override GameState Update()
		{
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

		public void ToGame()
		{
			enterGame = true;
		}
	}
}
