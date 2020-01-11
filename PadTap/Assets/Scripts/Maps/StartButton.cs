using UnityEngine;
using PadTap.Core;

namespace PadTap.Maps
{
    public class StartButton : MonoBehaviour
    {
        public void StartMap()
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.StartGame();
            gameObject.SetActive(false);
        }
    }
}