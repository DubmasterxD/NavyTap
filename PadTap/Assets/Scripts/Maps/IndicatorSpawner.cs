using PadTap.Core;
using System.Collections;
using UnityEngine;

namespace PadTap.Maps
{
    public class IndicatorSpawner : MonoBehaviour
    {
        [SerializeField] Indicator indicatorPrefab = null;
        [SerializeField] Map map = null;

        TileSpawner tileSpawner = null;
        Coroutine level = null;

        public Map Map { get => map; }

        private void Awake()
        {
            tileSpawner = GetComponent<TileSpawner>();
        }

        private IEnumerator Start()
        {
            SetMap(FindObjectOfType<Game>().chosenMap);
            tileSpawner.ShowTiles(map.tilesRows, map.tilesColumns);
            SetThresholds(map.threshold);
            level = StartCoroutine(SpawnContinuously());
            yield return PlaySong(map.song);
            StartCoroutine(EndAfterSongEnds());
        }

        public void GameOver()
        {
            StopCoroutine(level);
            FindObjectOfType<Scene>().LoadMenu();
        }

        private void SetMap(Map map)
        {
            this.map = map;
        }

        private void SetThresholds(float threshold)
        {
            foreach (Tile tile in tileSpawner.tiles)
            {
                tile.SetThreshold(threshold);
            }
        }

        IEnumerator SpawnContinuously()
        {
            int index = 0;
            while (index<map.points.Count)
            {
                float previousTime = 0;
                if (index != 0)
                { 
                    previousTime = map.points[index - 1].time;
                    yield return new WaitForSeconds(map.points[index].time - previousTime);
                }
                else
                {
                    if (GetFirstIndicatorSpawnTime() > 0)
                    {
                        yield return new WaitForSeconds(GetFirstIndicatorSpawnTime());
                    }
                }
                tileSpawner.tiles[map.points[index].tileIndex].Spawn(indicatorPrefab, map.indicatorLifespan);
                index++;
            }
        }

        private IEnumerator PlaySong(AudioClip song)
        {
            if (GetFirstIndicatorSpawnTime() < 0)
            {
                yield return new WaitForSeconds(-GetFirstIndicatorSpawnTime());
            }
            FindObjectOfType<Audio>().Play(song);
        }

        private float GetFirstIndicatorSpawnTime()
        {
            return map.points[0].time - ((1 - map.threshold) / 2 + map.threshold) * map.indicatorLifespan;
        }

        IEnumerator EndAfterSongEnds()
        {
            yield return new WaitForSeconds(map.song.length);
            GameOver();
        }
    }
}