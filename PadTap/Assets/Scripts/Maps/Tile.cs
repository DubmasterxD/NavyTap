using UnityEngine;

namespace PadTap.Maps
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] Transform spawnPoint = null;
        [SerializeField] Threshold threshold = null;

        public void SetThreshold(float value)
        {
            if (threshold != null)
            {
                threshold.SetThreshold(value);
            }
        }

        public void SetPerfectScore(float perfectScore, float perfectScoreDifference)
        {
            if (threshold != null)
            {
                threshold.SetPerfectScore(perfectScore, perfectScoreDifference);
            }
        }

        public void Spawn(Indicator toSpawn, float lifespan)
        {
            if (spawnPoint != null)
            {
                Indicator indicator = Instantiate(toSpawn, spawnPoint);
                indicator.StartIndicator(lifespan);
            }
        }
    }
}