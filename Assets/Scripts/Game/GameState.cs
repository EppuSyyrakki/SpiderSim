using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim.Game
{
	public abstract class GameState
	{
		public virtual GameState Update()
		{
			return null;
		}

		public virtual void OnStateEnter()
		{

		}

		public virtual void OnStateExit()
		{

		}
	}
}
