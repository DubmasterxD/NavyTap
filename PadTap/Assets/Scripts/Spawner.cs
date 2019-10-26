using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PadTap
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] Indicator indicatorPrefab = null;
        [SerializeField] List<Tile> tiles = new List<Tile>();
        [SerializeField] Map map = null;

        Coroutine game = null;

        public void StartGame()
        {
            game = StartCoroutine(SpawnContinuously());
        }

        IEnumerator SpawnContinuously()
        {
            int index = 0;
            while (index<map.Points.Count)
            {
                float previousTime = 0;
                if (index != 0)
                {
                    previousTime = map.Points[index - 1].time;
                }
                yield return new WaitForSeconds(map.Points[index].time - previousTime);
                tiles[map.Points[index].tileIndex].Spawn(indicatorPrefab.gameObject);
                index++;
            }
            GameOver();
        }

        public void GameOver()
        {
            StopCoroutine(game);
        }
    }
}