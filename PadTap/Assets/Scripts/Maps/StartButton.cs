using UnityEngine;
using NavyTap.Core;
using UnityEngine.UI;

namespace NavyTap.Maps
{
    public class StartButton : MonoBehaviour
    {
        public void StartMap()
        {
            GetComponent<Button>().interactable = false;
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.PrepareSong();
        }
    }
}