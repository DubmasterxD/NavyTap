using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PadTap
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] Indicator indicatorPrefab = null;
        [SerializeField] List<Tile> tiles = new List<Tile>();

        private void Start()
        {
            StartCoroutine(SpawnContinuously());
        }

        IEnumerator SpawnContinuously()
        {
            while (true)
            {
                tiles[Random.Range(0, tiles.Count)].Spawn(indicatorPrefab.gameObject);
                yield return new WaitForSeconds(1);
            }
        }
    }
}