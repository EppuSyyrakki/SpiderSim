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
            SceneManager.LoadScene(1);
            Cursor.visible = false;
        }

        public void LoadBathroom()
        {
            SceneManager.LoadScene(2);
            Cursor.visible = false;
        }

        public void Quit()
        {
            Application.Quit();
        }

    }
}
