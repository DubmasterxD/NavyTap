using NavyTap.Core;
using System.Collections.Generic;
using UnityEngine;

namespace NavyTap.Maps
{
    public class BombSpawner : MonoBehaviour
    {
        [SerializeField] Bomb bombPrefab = null;
        [SerializeField] Transform bombsTransfrom = null;
        [SerializeField] int bombsInPool = 5;

        List<Bomb> bombs;
        List<Map.Point> points;
        float currentTime = 0;

        int columns = 1;
        private const float basicFallingSpeed = 17;
        private float fallingSpeed = 17;
        private bool isPlaying = false;
        private GameManager game = null;
        private TileSpawner tileSpawner = null;

        private void Awake()
        {
            game = FindObjectOfType<GameManager>();
            if (game == null)
            {
                Debug.LogError(Logger.NoComponentFound(typeof(GameManager)));
            }
            tileSpawner = FindObjectOfType<TileSpawner>();
            if (tileSpawner == null)
            {
                Debug.LogError(Logger.NoComponentFound(typeof(TileSpawner)));
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
                bombsTransfrom.position -= new Vector3(0, Time.deltaTime *fallingSpeed, 0);
                currentTime += Time.deltaTime;
            }
        }

        private void SongStarted(Map map)
        {
            fallingSpeed = basicFallingSpeed / map.indicatorLifespan;
            isPlaying = true;
            columns = map.tilesColumns;
            SetTimers(map);
            CreateBombsPool(map);
            for(int i=0; i < bombsInPool; i++)
            {
                SetNextBomb();
            }
        }

        private void SetTimers(Map map)
        {
            points = new List<Map.Point>();
            foreach(Map.Point point in map.points)
            {
                points.Add(new Map.Point(point.time + map.indicatorLifespan, point.tileIndex));
            }
        }

        private void CreateBombsPool(Map map)
        {
            bombs = new List<Bomb>();
            for(int i=0; i < bombsInPool; i++)
            {
                bombs.Add(Instantiate(bombPrefab, bombsTransfrom));
            }
        }

        public void SetNextBomb()
        {
            if (points != null)
            {
                if (points.Count > 0)
                {
                    bombs[0].SetBombTimer(points[0].time - currentTime);
                    float positionX = tileSpawner.tiles[points[0].tileIndex].transform.position.x + tileSpawner.tileSize / 2;
                    float positionY = tileSpawner.tiles[points[0].tileIndex].transform.position.y - tileSpawner.tileSize / 2 + (points[0].time - currentTime) * fallingSpeed;
                    bombs[0].transform.position = new Vector3(positionX, positionY, 0);
                    bombs[0].SetIndex(points[0].tileIndex / columns + 1);
                    points.RemoveAt(0);
                    bombs.Add(bombs[0]);
                    bombs.RemoveAt(0);
                }
                else if (bombs.Count > 0)
                {
                    Bomb bomb = bombs[0];
                    bombs.RemoveAt(0);
                    Destroy(bomb);
                }
            }
        }
    }
}