using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PadTap
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] List<Tile> tiles = new List<Tile>();
        [SerializeField] Map map = null;

        Coroutine game = null;

        public Map Map { get => map; }

        public void StartGame()
        {
            game = StartCoroutine(SpawnContinuously());
            SetThresholds(map.threshold);
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
                }
                yield return new WaitForSeconds(map.points[index].time - previousTime);
                tiles[map.points[index].tileIndex].Spawn(map.indicatorPrefab, map.indicatorLifespan);
                index++;
            }
            GameOver();
        }

        private void SetThresholds(float threshold)
        {
            foreach(Tile tile in tiles)
            {
                tile.SetThreshold(threshold);
            }
        }

        public void GameOver()
        {
            StopCoroutine(game);
        }
    }
}