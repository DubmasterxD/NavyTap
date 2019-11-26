using UnityEngine;
using UnityEngine.SceneManagement;

namespace PadTap.Core
{
    public class Scene : MonoBehaviour
    {
        [SerializeField] int mapSceneIndex = 1;
        float waitTimeAfterLoading = 1f;

        public void LoadMap(Map map)
        {
            if (SceneManager.sceneCountInBuildSettings > mapSceneIndex)
            {
                SceneManager.LoadScene(mapSceneIndex);
            }
            else
            {
                Logger.Error("No scene with index " + mapSceneIndex + " assigned to build");
            }
            StartMap(map);
        }

        public void StartMap(Map map)
        {
            Game game = FindObjectOfType<Game>();
            if (game != null)
            {
                game.StartGame(map, waitTimeAfterLoading);
            }
            else
            {
                Logger.NoComponentFound(typeof(Game));
                return;
            }
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}