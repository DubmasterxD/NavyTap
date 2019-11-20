using System;
using System.Collections;
using UnityEngine;

namespace PadTap.Core
{
    public class Game : MonoBehaviour
    {
        public delegate void OnGameStart(Map map);
        public event OnGameStart onGameStart;

        private void Awake()
        {
            if (FindObjectsOfType<Game>().Length == 1)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartGame(Map map, float timeToStart)
        {
            StartCoroutine(StartGameAfter(map, timeToStart));
        }

        public IEnumerator StartGameAfter(Map map, float timeToStart)
        {
            yield return new WaitForSeconds(timeToStart);
            onGameStart(map);
        }

        public void GameOver()
        {
            FindObjectOfType<Scene>().LoadMenu();
        }
    }
}