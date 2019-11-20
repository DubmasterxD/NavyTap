using UnityEngine;
using UnityEngine.SceneManagement;

namespace PadTap.Core
{
    public class Scene : MonoBehaviour
    {
        float waitTimeAfterLoading = 1f;

        public void LoadMap(Map map)
        {
            FindObjectOfType<Game>().StartGame(map, waitTimeAfterLoading);
            SceneManager.LoadScene(1);
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}