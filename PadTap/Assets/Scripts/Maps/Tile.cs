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
            else
            {
                Logger.NotAssigned(typeof(Threshold), GetType(), name);
            }
        }

        public void SetPerfectScore(float perfectScore, float perfectScoreDifference)
        {
            if (threshold != null)
            {
                threshold.SetPerfectScoreLimits(perfectScore, perfectScoreDifference);
            }
            else
            {
                Logger.NotAssigned(typeof(Threshold), GetType(), name);
            }
        }

        public void Spawn(Indicator toSpawn, float lifespan)
        {
            if (spawnPoint != null)
            {
                Indicator indicator = Instantiate(toSpawn, spawnPoint);
                indicator.StartIndicator(lifespan);
            }
            else
            {
                Logger.NotAssigned(typeof(Transform), GetType(), name);
            }
        }
    }
}