using UnityEngine;
using UnityEngine.SceneManagement;

namespace PadTap.Core
{
    public class Scene : MonoBehaviour
    {
        [SerializeField] int mapSceneIndex = 1;

        public void LoadMap(Map map)
        {
            if (SceneManager.sceneCountInBuildSettings > mapSceneIndex)
            {
                SceneManager.LoadScene(mapSceneIndex);
            }
            else
            {
                Debug.LogError("No scene with index " + mapSceneIndex + " assigned to build");
            }
            ChooseMap(map);
        }

        public void ChooseMap(Map map)
        {
            GameManager game = FindObjectOfType<GameManager>();
            if (game != null)
            {
                game.ChooseMap(map);
            }
            else
            {
                Debug.LogError(Logger.NoComponentFound(typeof(GameManager)));
                return;
            }
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}