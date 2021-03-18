using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpiderSim.Game
{
	public class GameManager : MonoBehaviour
	{
		private static GameManager instance = null;
		private static GameState state = null;

		public static GameManager Instance
		{
			get
			{
				if (instance == null)
				{
					Debug.Log("Game Manager Instance not found! Add it to the scene from the Prefabs folder");
				}

				return instance;
			}
		}

		void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
			else if (instance != this)
			{
				Destroy(gameObject);
				return;
			}

			DontDestroyOnLoad(gameObject);
			state = new InGameState();
		}

		private void Update()
		{
			UpdateState();
		}

		private static void UpdateState()
		{
			GameState newState = state.Update();

			if (newState != null)
			{
				state.OnStateExit();
				state = newState;
				state.OnStateEnter();
			}
		}


		public void ChangeScene(int levelNumber)
		{
			SceneManager.LoadScene(levelNumber);
		}

		public void ReLoadScene()
		{
			SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex));
		}
	}
}
