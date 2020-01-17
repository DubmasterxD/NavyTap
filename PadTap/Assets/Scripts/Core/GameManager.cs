using System.Collections;
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