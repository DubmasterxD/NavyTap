using UnityEngine;
using PadTap.Core;
using UnityEngine.UI;

namespace PadTap.Maps
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