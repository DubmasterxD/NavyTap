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

        public void SpawnIn(Indicator toSpawn, float lifespan, float time)
        {
            if (spawnPoint != null)
            {
                Indicator indicator = Instantiate(toSpawn, spawnPoint);
                StartCoroutine(indicator.StartIndicatorIn(time, lifespan));
            }
            else
            {
                Logger.NotAssigned(typeof(Transform), GetType(), name);
            }
        }
    }
}