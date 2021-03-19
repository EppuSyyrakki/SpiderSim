using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpiderSim.Game
{
	public abstract class GameState
	{
		private int associatedSceneIndex;

		public virtual GameState Update()
		{
			return null;
		}

		public virtual void OnStateEnter()
		{
			SceneManager.LoadScene(associatedSceneIndex);
		}

		public virtual void OnStateExit()
		{

		}

		public void SetAssociatedScene(int i)
		{
			associatedSceneIndex = i;
		}
	}
}
