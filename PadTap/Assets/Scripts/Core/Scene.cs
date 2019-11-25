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
            Game game = FindObjectOfType<Game>();
            try
            {
                game.StartGame(map, waitTimeAfterLoading);
            }
            catch (System.Exception e)
            {
                Debug.LogError("No object with " + typeof(Game) + " component found!\n" + e);
                return;
            }
            if (SceneManager.sceneCount > mapSceneIndex)
            {
                SceneManager.LoadScene(mapSceneIndex);
            }
            else
            {
                Debug.LogError("No scene with index " + mapSceneIndex + " assigned to build");
            }
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}