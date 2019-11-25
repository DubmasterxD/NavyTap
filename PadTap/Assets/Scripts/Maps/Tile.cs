using UnityEngine;

namespace PadTap.Maps
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] Transform spawnPoint = null;
        [SerializeField] Threshold threshold = null;

        public void SetThreshold(float value)
        {
            try
            {
                threshold.SetThreshold(value);
            }
            catch (System.Exception e)
            {
                Debug.LogError("No " + typeof(Threshold) + " assigned to " + GetType() + " in " + name + "\n" + e);
            }
        }

        public void SetPerfectScore(float perfectScore, float perfectScoreDifference)
        {
            try
            {
                threshold.SetPerfectScoreLimits(perfectScore, perfectScoreDifference);
            }
            catch (System.Exception e)
            {
                Debug.LogError("No " + typeof(Threshold) + " assigned to " + GetType() + " in " + name + "\n" + e);
            }
        }

        public void Spawn(Indicator toSpawn, float lifespan)
        {
            try
            {
                Indicator indicator = Instantiate(toSpawn, spawnPoint);
                indicator.StartIndicator(lifespan);
            }
            catch (System.Exception e)
            {
                Debug.LogError("No " + typeof(Transform) + " assigned to " + GetType() + " in " + name + "\n" + e);
            }
        }
    }
}