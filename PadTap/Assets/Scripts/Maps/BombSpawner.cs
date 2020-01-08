using PadTap.Core;
using UnityEngine;

namespace PadTap.Maps
{
    public class BombSpawner : MonoBehaviour
    {
        [SerializeField] GameObject bombPrefab = null;
        [SerializeField] Transform bombs = null;

        public void SpawnBombs(Map map)
        {
            foreach(Map.Point point in map.points)
            {
                GameObject bomb = Instantiate(bombPrefab, bombs);
                bomb.transform.position = point.time * Vector3.one;
            }
        }
    }
}