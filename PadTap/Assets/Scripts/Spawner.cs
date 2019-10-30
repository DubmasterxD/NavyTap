using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PadTap
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] List<Tile> tiles = new List<Tile>();
        [SerializeField] Indicator indicatorPrefab = null;
        [SerializeField] Map map = null;

        Coroutine level = null;

        public Map Map { get => map; }

        private IEnumerator Start()
        {
            SetMap(FindObjectOfType<Game>().chosenMap);
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
            foreach (Tile tile in tiles)
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
                tiles[map.points[index].tileIndex].Spawn(indicatorPrefab, map.indicatorLifespan);
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