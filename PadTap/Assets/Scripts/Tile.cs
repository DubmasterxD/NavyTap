using UnityEngine;

namespace PadTap
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] Transform spawnPoint = null;

        Threshold threshold;

        private void Awake()
        {
            threshold = GetComponentInChildren<Threshold>();
        }

        public void SetThreshold(float threshold)
        {
            this.threshold.SetThreshold(threshold);
        }

        public void Spawn(Indicator toSpawn, float lifespan)
        {
            Indicator indicator = Instantiate(toSpawn, spawnPoint);
            indicator.StartIndicator(lifespan);
        }
    }
}