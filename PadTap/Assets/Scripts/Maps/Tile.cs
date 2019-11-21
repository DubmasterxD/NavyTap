using UnityEngine;

namespace PadTap.Maps
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] Transform spawnPoint = null;
        [SerializeField] Threshold threshold;

        public void SetThreshold(float value)
        {
            threshold.SetThreshold(value);
        }

        public void SetPerfectScore(float perfectScore, float perfectScoreDifference)
        {
            threshold.SetPerfectScore(perfectScore, perfectScoreDifference);
        }

        public void Spawn(Indicator toSpawn, float lifespan)
        {
            Indicator indicator = Instantiate(toSpawn, spawnPoint);
            indicator.StartIndicator(lifespan);
        }
    }
}