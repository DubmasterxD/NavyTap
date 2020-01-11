using PadTap.Core;
using UnityEngine;

namespace PadTap.Maps
{
    public class BombSpawner : MonoBehaviour
    {
        [SerializeField] Bomb bombPrefab = null;
        [SerializeField] Transform bombs = null;

        private const float basicFallingSpeed = 17;
        private float fallingSpeed = 17;
        private bool isPlaying = false;
        private GameManager game = null;

        private void Awake()
        {
            game = FindObjectOfType<GameManager>();
            if (game == null)
            {
                Debug.LogError(Logger.NoComponentFound(typeof(GameManager)));
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

        private void Update()
        {
            if (isPlaying)
            {
                bombs.position -= new Vector3(0, Time.deltaTime *fallingSpeed, 0);
            }
        }

        private void StartGame(Map map)
        {
            fallingSpeed = basicFallingSpeed / map.indicatorLifespan;
            SpawnBombs(map);
            isPlaying = true;
        }

        private void SpawnBombs(Map map)
        {
            foreach(Map.Point point in map.points)
            {
                Bomb bomb = Instantiate(bombPrefab, bombs);
                bomb.SetBombTimer(point.time + map.indicatorLifespan);
                float positionX = FindObjectOfType<TileSpawner>().tiles[point.tileIndex].transform.position.x + FindObjectOfType<TileSpawner>().tileSize / 2;
                float positionY = FindObjectOfType<TileSpawner>().tiles[point.tileIndex].transform.position.y - FindObjectOfType<TileSpawner>().tileSize / 2 + (point.time + map.indicatorLifespan) * fallingSpeed;
                bomb.transform.position = new Vector3(positionX, positionY, 0);
            }
        }
    }
}