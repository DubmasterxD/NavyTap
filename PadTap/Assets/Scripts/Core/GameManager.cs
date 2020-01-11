using System.Collections;
using UnityEngine;

namespace PadTap.Core
{
    public class GameManager : MonoBehaviour
    {
        public delegate void OnGameStart(Map map);
        public event OnGameStart onGameStart;

        [SerializeField] Map chosenMap = null;
        private bool isPlaying = false;

        private void Awake()
        {
            if (FindObjectsOfType<GameManager>().Length == 1)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void ChooseMap(Map map)
        {
            chosenMap = map;
        }

        public void StartGame()
        {
            isPlaying = true;
            onGameStart(chosenMap);
        }

        public void GameOver()
        {
            Scene scene = FindObjectOfType<Scene>();
            if (scene != null)
            {
                scene.LoadMenu();
            }
            else
            {
                Debug.LogError(Logger.NoComponentFound(typeof(Scene)));
            }
        }
    }
}