using PadTap.Core;
using System.Collections;
using UnityEngine;

namespace PadTap.Maps
{
    [RequireComponent(typeof(TileSpawner))]
    public class IndicatorSpawner : MonoBehaviour
    {
        [SerializeField] private Indicator indicatorPrefab = null;

        public Map map { get; private set; } = null;

        private GameManager game = null;
        private TileSpawner tileSpawner = null;

        private void Awake()
        {
            game = FindObjectOfType<GameManager>();
            tileSpawner = GetComponent<TileSpawner>();
            if (game == null)
            {
                Logger.NoComponentFound(typeof(GameManager));
            }
        }

        private void OnEnable()
        {
            if (game != null)
            {
                game.onGameStart += StartGame;
            }
        }

        private void OnDisable()
        {
            if (game != null)
            {
                game.onGameStart -= StartGame;
            }
        }

        public void StartGame(Map newMap)
        {
            map = newMap;
            SpawnIndicators();
            if(map!=null)
            {
                StartCoroutine(PlaySong(map.song));
            }
            else
            {
                Debug.LogError(typeof(Map) + " received is null");
            }
        }

        public void GameOver()
        {
            game.GameOver();
        }

        private void SpawnIndicators()
        {
            if (map != null)
            {
                foreach (Map.Point point in map.points)
                {
                    tileSpawner.tiles[point.tileIndex].SpawnIn(indicatorPrefab, map.indicatorLifespan, point.time - map.GetPerfectScore() * map.indicatorLifespan);
                }
            }
            else
            {
                Logger.NotAssigned(typeof(Map), GetType(), name);
            }
        }

        private IEnumerator PlaySong(AudioClip song)
        {
            if (GetFirstIndicatorSpawnTime() < 0)
            {
                yield return new WaitForSeconds(-GetFirstIndicatorSpawnTime());
            }
            FindObjectOfType<Audio>().Play(song);
            if (map != null)
            {
                yield return new WaitForSeconds(map.song.length);
            }
            GameOver();
        }

        private float GetFirstIndicatorSpawnTime()
        {
            if (map != null)
            {
                return map.points[0].time - map.GetPerfectScore() * map.indicatorLifespan;
            }
            else
            {
                Logger.NotAssigned(typeof(Map), GetType(), name);
                return 0;
            }
        }
    }
}