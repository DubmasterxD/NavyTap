using UnityEngine;

namespace PadTap.Maps
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] Transform spawnPoint = null;
        [SerializeField] Transform perfectScoreMax = null;
        [SerializeField] Transform perfectScoreMin = null;

        public void SetPerfectScore(float perfectScore, float perfectScoreDifference)
        {
            float newMaxPerfectScore = perfectScore + perfectScoreDifference;
            SetScaleOfTransform(perfectScoreMax, newMaxPerfectScore);
            float newMinPerfectScore = perfectScore - perfectScoreDifference;
            SetScaleOfTransform(perfectScoreMin, newMinPerfectScore);
        }

        private void SetScaleOfTransform(Transform transform, float newScale)
        {
            if (transform != null)
            {
                transform.localScale = new Vector3(newScale, newScale, newScale);
            }
            else
            {
                Debug.LogError(Logger.NotAssigned(typeof(Transform), GetType(), name));
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
                Debug.LogError(Logger.NotAssigned(typeof(Transform), GetType(), name));
            }
        }
    }
}