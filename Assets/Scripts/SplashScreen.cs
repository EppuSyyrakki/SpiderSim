using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpiderSim
{
    public class SplashScreen : MonoBehaviour
    {
        public void GoToMainMenu()
        {
            SceneManager.LoadScene(1);
        }
    }
}
