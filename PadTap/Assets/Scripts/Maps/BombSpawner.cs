using PadTap.Core;
using System.Collections;
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
            game.onStartSong += SongStarted;
        }

        private void OnDisable()
        {
            game.onStartSong -= SongStarted;
        }

        private void Update()
        {
            if (isPlaying)
            {
                bombs.position -= new Vector3(0, Time.deltaTime *fallingSpeed, 0);
            }
        }

        private void SongStarted(Map map)
        {
            fallingSpeed = basicFallingSpeed / map.indicatorLifespan;
            isPlaying = true;
            StartCoroutine(SpawnBombs(map));
        }

        private IEnumerator SpawnBombs(Map map)
        {
            int count = 0;
            foreach(Map.Point point in map.points)
            {
                Bomb bomb = Instantiate(bombPrefab, bombs);
                bomb.SetBombTimer(point.time + map.indicatorLifespan);
                float positionX = FindObjectOfType<TileSpawner>().tiles[point.tileIndex].transform.position.x + FindObjectOfType<TileSpawner>().tileSize / 2;
                float positionY = FindObjectOfType<TileSpawner>().tiles[point.tileIndex].transform.position.y - FindObjectOfType<TileSpawner>().tileSize / 2 + (point.time + map.indicatorLifespan) * fallingSpeed;
                bomb.transform.position = new Vector3(positionX, positionY, 0);
                count++;
                if (count % 10 == 0)
                {
                    yield return null;
                }
            }
        }
    }
}