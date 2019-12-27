using System.Collections;
using UnityEngine;

namespace PadTap.Core
{
    public class GameManager : MonoBehaviour
    {
        public delegate void OnGameStart(Map map);
        public event OnGameStart onGameStart;

        [SerializeField] Map testingMap = null;
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

        private void Update()
        {
            if (!isPlaying)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    StartGame(testingMap, 0);
                }
            }
        }

        public void StartGame(Map map, float timeToStart)
        {
            isPlaying = true;
            StartCoroutine(StartGameAfter(map, timeToStart));
        }

        public IEnumerator StartGameAfter(Map map, float timeToStart)
        {
            yield return new WaitForSeconds(timeToStart);
            onGameStart(map);
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
                Logger.NoComponentFound(typeof(Scene));
            }
        }
    }
}