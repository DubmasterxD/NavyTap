using UnityEngine;

namespace PadTap
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] Transform spawnPoint = null;

        public void Spawn(GameObject toSpawn)
        {
            Instantiate(toSpawn, spawnPoint);
        }
    }
}