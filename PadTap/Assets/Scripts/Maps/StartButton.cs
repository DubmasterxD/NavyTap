using UnityEngine;
using PadTap.Core;
using System;
using UnityEngine.UI;

namespace PadTap.Maps
{
    public class StartButton : MonoBehaviour
    {
        public void StartMap()
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.StartGame();
            GetComponent<Button>().interactable = false;
        }
    }
}