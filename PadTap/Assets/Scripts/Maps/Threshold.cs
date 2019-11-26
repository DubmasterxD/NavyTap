using UnityEngine;

namespace PadTap.Maps
{
    public class Threshold : MonoBehaviour
    {
        [SerializeField] Transform threshold = null;
        [SerializeField] Transform perfectScoreMax = null;
        [SerializeField] Transform perfectScoreMin = null;

        public void SetThreshold(float newThreshold)
        {
            SetScaleOfTransform(threshold, newThreshold);
        }

        public void SetPerfectScoreLimits(float newPerfectScore, float newPerfectScoreDifference)
        {
            float newMaxPerfectScore = newPerfectScore + newPerfectScoreDifference;
            SetScaleOfTransform(perfectScoreMax, newMaxPerfectScore);
            float newMinPerfectScore = newPerfectScore - newPerfectScoreDifference;
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
                Logger.NotAssigned(typeof(Transform), GetType(), name);
            }
        }
    }
}