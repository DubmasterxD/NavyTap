using NavyTap.Core;
using System.Collections.Generic;
using UnityEngine;

namespace NavyTap.Maps
{
    public class VisualizationButtons : MonoBehaviour
    {
        [SerializeField] GameObject buttonsHover = null;
        [SerializeField] TapVisualizationButton buttonPrefab = null;
        const float originalTileSize = 0.64f;
        float tileSize = 0;
        const float mapSize = 1.28f;

        List<TapVisualizationButton> buttons;

        Animator anim;
        GameManager game;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            game = FindObjectOfType<GameManager>();
        }

        private void OnEnable()
        {
            game.onPrepareSong += ShowButtons;
        }

        private void OnDisable()
        {
            game.onPrepareSong -= ShowButtons;
        }

        private void Start()
        {
            anim.speed = 0;
            buttons = new List<TapVisualizationButton>();
        }

        private void ShowButtons(Map map)
        {
            SpawnButtons(map);
            OpenButtons();
        }

        private void SpawnButtons(Map map)
        {
            tileSize = mapSize / Mathf.Max(map.tilesRows, map.tilesColumns);
            for (int row=0; row < map.tilesRows; row++)
            {
                for(int column = 0; column < map.tilesColumns; column++)
                {
                    TapVisualizationButton button = Instantiate(buttonPrefab, transform);
                    buttons.Add(button);
                    int index = column + row * map.tilesColumns;
                    button.transform.localScale = new Vector3(tileSize / originalTileSize, tileSize / originalTileSize, tileSize / originalTileSize);
                    float posX = -mapSize / 2 + tileSize * column + tileSize / 2;
                    posX += mapSize / 2 * (1 - Mathf.Clamp01(map.tilesColumns / (float)map.tilesRows));
                    float posY = mapSize / 2 - tileSize * row - tileSize / 2;
                    posY -= mapSize / 2 * (1 - Mathf.Clamp01(map.tilesRows / (float)map.tilesColumns));                    
                    button.transform.localPosition = new Vector3(posX, posY, 0);
                }
            }
        }

        public void TileClicked(int index)
        {
            buttons[index].LightButton();
        }

        private void OpenButtons()
        {
            anim.speed = 1;
        }

        public void StopOpeningButtons()
        {
            Destroy(buttonsHover);
        }
    }
}