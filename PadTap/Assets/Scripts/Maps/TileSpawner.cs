using PadTap.Core;
using System.Collections.Generic;
using UnityEngine;

namespace PadTap.Maps
{
    public class TileSpawner : MonoBehaviour
    {
        [SerializeField] private Tile tilePrefab = null;
        [SerializeField] private Transform spawnPoint = null;
        private float originalTileSize = 2.5f;
        public float tileSize { get; private set; } = 0;
        private float mapSize = 10;

        public List<Tile> tiles { get; private set; } = null;

        private GameManager game = null;

        private void Awake()
        {
            game = FindObjectOfType<GameManager>();
            if (game == null)
            {
                Logger.NoComponentFound(typeof(GameManager));
            }
        }

        private void OnEnable()
        {
            game.onGameStart += StartGame;
        }

        private void OnDisable()
        {
            game.onGameStart -= StartGame;
        }

        private void StartGame(Map map)
        {
            if (map != null)
            {
                ShowTiles(map.tilesRows, map.tilesColumns);
                SetThresholds(map.threshold, map.GetPerfectScore(), map.GetPerfectScoreAcceptableDifference());
            }
            else
            {
                Debug.LogError(typeof(Map) + " received is null");
            }
        }

        public void ShowTiles(int rows, int columns)
        {
            CreateList(rows * columns);
            if (rows > 0 && columns > 0)
            {
                tileSize = mapSize / Mathf.Max(rows, columns);
                for (int row = 0; row < rows; row++)
                {
                    for (int column = 0; column < columns; column++)
                    {
                        int index = column + row * columns;
                        tiles[index].gameObject.SetActive(true);
                        tiles[index].transform.localScale = new Vector3(tileSize / originalTileSize, tileSize / originalTileSize, tileSize / originalTileSize);
                        float posX = -mapSize / 2 + tileSize * column;
                        posX += mapSize / 2 * (1 - Mathf.Clamp01(columns / (float)rows));
                        float posY = mapSize / 2 - tileSize * row;
                        posY -= mapSize / 2 * (1 - Mathf.Clamp01(rows / (float)columns));
                        tiles[index].transform.localPosition = new Vector3(posX, posY);
                    }
                }
            }
            else
            {
                Debug.LogError("Wrong number of rows and/or columns received");
            }
        }

        private void CreateList(int size)
        {
            if (tiles == null)
            {
                tiles = new List<Tile>();
            }
            if (tiles.Count < size)
            {
                if (tilePrefab != null)
                {
                    for (int i = tiles.Count; i < size; i++)
                    {
                        tiles.Add(Instantiate(tilePrefab, spawnPoint));
                    }
                }
                else
                {
                    Logger.NotAssigned(typeof(Tile), GetType(), name);
                }
            }
            foreach (Tile tile in tiles)
            {
                tile.gameObject.SetActive(false);
            }
            for (int i = 0; i < size; i++)
            {
                tiles[i].gameObject.SetActive(true);
            }
        }

        private void SetThresholds(float threshold, float perferctScore, float perfectScoreDifference)
        {
            if (tiles != null)
            {
                foreach (Tile tile in tiles)
                {
                    tile.SetThreshold(threshold);
                    tile.SetPerfectScore(perferctScore, perfectScoreDifference);
                }
            }
        }
    }
}