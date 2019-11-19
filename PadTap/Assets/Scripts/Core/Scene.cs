using UnityEngine;
using UnityEngine.SceneManagement;

namespace PadTap.Core
{
    public class Scene : MonoBehaviour
    {
        public void LoadMap(Map map)
        {
            FindObjectOfType<Game>().chosenMap = map;
            SceneManager.LoadScene(1);
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}