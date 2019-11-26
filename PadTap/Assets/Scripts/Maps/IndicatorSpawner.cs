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

        private Game game = null;
        private TileSpawner tileSpawner = null;
        private Coroutine spawning = null;

        private void Awake()
        {
            game = FindObjectOfType<Game>();
            tileSpawner = GetComponent<TileSpawner>();
            if (game == null)
            {
                Logger.NoComponentFound(typeof(Game));
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
            spawning = StartCoroutine(SpawnContinuously());
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
            StopCoroutine(spawning);
            game.GameOver();
        }

        private IEnumerator SpawnContinuously()
        {
            if (map != null)
            {
                int index = 0;
                while (index < map.GetPointsCount())
                {
                    yield return WaitToSpawnIndicator(index);
                    int tileIndexToSpawn = map.points[index].tileIndex;
                    if (indicatorPrefab != null)
                    {
                        tileSpawner.tiles[tileIndexToSpawn].Spawn(indicatorPrefab, map.indicatorLifespan);
                    }
                    index++;
                }
            }
            else
            {
                Logger.NotAssigned(typeof(Map), GetType(), name);
            }
        }

        private IEnumerator WaitToSpawnIndicator(int index)
        {
            if (map != null)
            {
                if (index != 0)
                {
                    float previousTime = map.points[index - 1].time;
                    yield return new WaitForSeconds(map.points[index].time - previousTime);
                }
                else
                {
                    if (GetFirstIndicatorSpawnTime() > 0)
                    {
                        yield return new WaitForSeconds(GetFirstIndicatorSpawnTime());
                    }
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