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
            threshold.localScale = new Vector3(newThreshold, newThreshold, newThreshold);
        }

        public void SetPerfectScore(float newPerfectScore, float newPerfectScoreDifference)
        {
            float newMaxPerfectScore = newPerfectScore + newPerfectScoreDifference;
            float newMinPerfectScore = newPerfectScore - newPerfectScoreDifference;
            perfectScoreMax.localScale = new Vector3(newMaxPerfectScore, newMaxPerfectScore, newMaxPerfectScore);
            perfectScoreMin.localScale = new Vector3(newMinPerfectScore, newMinPerfectScore, newMinPerfectScore);
        }
    }
}