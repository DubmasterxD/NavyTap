using UnityEngine;

namespace PadTap {
    public class Game : MonoBehaviour
    {
        bool isRunning = false;

        private void Update()
        {
            if (!isRunning)
            {
                StartGame();
            }
        }

        private void StartGame()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FindObjectOfType<Spawner>().StartGame();
            }
        }
    }
}