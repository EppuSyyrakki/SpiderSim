using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpiderSim.UI
{
    public class MenuUIController : MonoBehaviour
    {
        public void LoadKitchen()
        {
            Debug.Log("Loading kitchen");
            SceneManager.LoadScene(1);
        }

        public void LoadBathroom()
        {
            Debug.Log("Loading bathroom");
            SceneManager.LoadScene(2);
        }

        public void Quit()
        {
            Debug.Log("Quit pressed");
            Application.Quit();
        }

    }
}
