using UnityEngine;

namespace PadTap {
    public class Game : MonoBehaviour
    {
        private void Update()
        {
            StartGame();
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