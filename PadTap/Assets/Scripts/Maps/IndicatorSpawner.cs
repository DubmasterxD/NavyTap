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
                Debug.LogError(Logger.NoComponentFound(typeof(GameManager)));
            }
        }

        private void OnEnable()
        {
            if (game != null)
            {
                game.onStartSong += SongStarted;
            }
        }

        private void OnDisable()
        {
            if (game != null)
            {
                game.onStartSong -= SongStarted;
            }
        }

        public void SongStarted(Map newMap)
        {
            map = newMap;
            StartCoroutine(SpawnIndicators());
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

        private IEnumerator SpawnIndicators()
        {
            if (map != null)
            {
                int count = 0;
                foreach (Map.Point point in map.points)
                {
                    tileSpawner.tiles[point.tileIndex].SpawnIndicator(indicatorPrefab, map.indicatorLifespan, point.time + (1 - map.GetPerfectScore()) * map.indicatorLifespan);
                    count++;
                    if (count % 10 == 0)
                    {
                        yield return null;
                    }
                }
            }
            else
            {
                Debug.LogError(Logger.NotAssigned(typeof(Map), GetType(), name));
            }
        }

        private IEnumerator PlaySong(AudioClip song)
        {
            //if (GetFirstIndicatorSpawnTime() < 0)
            //{
            //    yield return new WaitForSeconds(-GetFirstIndicatorSpawnTime());
            //}
            yield return new WaitForSeconds(map.indicatorLifespan);
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
                Debug.LogError(Logger.NotAssigned(typeof(Map), GetType(), name));
                return 0;
            }
        }
    }
}