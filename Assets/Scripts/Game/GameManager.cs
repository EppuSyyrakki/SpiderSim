using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpiderSim.Game
{
	public class GameManager : MonoBehaviour
	{
		// Test edit, problems with GitKraken
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
			//UpdateMouse();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
				ReturnToMainMenu();
                Cursor.visible = true;
            }
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

		// Updates the lock state of the cursor.
		// None = Normal, can go out of screen
		// Confined = Cursor locked inside the screen
        private void UpdateMouse()
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
				Debug.Log("No lock mode");
                Cursor.lockState = CursorLockMode.None;
            } 
            else if (Input.GetKeyDown(KeyCode.U))
            {
                Debug.Log("Confined lock mode");
				Cursor.lockState = CursorLockMode.Confined;
            }
        }

        private void ReturnToMainMenu()
        {
            SceneManager.LoadScene(0);
        }

		//public void ChangeScene(int levelNumber)
		//{
		//	SceneManager.LoadScene(levelNumber);
		//}

		//public void ReLoadScene()
		//{
		//	SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex));
		//}
	}
}
