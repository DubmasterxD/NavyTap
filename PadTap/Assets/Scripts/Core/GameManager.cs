using UnityEngine;

namespace NavyTap.Core
{
    public class GameManager : MonoBehaviour
    {
        public delegate void OnPrepareSong(Map map);
        public event OnPrepareSong onPrepareSong;
        public delegate void OnStartSong(Map map);
        public event OnStartSong onStartSong;

        [SerializeField] Map chosenMap = null;
        public bool isPlaying = false;
        public float currentTime = 0;

        private void Awake()
        {
            if (FindObjectsOfType<GameManager>().Length == 1)
            {
                DontDestroyOnLoad(gameObject);
                Application.targetFrameRate = 60;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (isPlaying)
            {
                currentTime += Time.deltaTime;
            }
        }

        public void ChooseMap(Map map)
        {
            chosenMap = map;
        }

        public Map GetChosenMap()
        {
            return chosenMap;
        }

        public void PrepareSong()
        {
            onPrepareSong(chosenMap);
        }

        public void StartSong()
        {
            isPlaying = true;
            onStartSong(chosenMap);
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