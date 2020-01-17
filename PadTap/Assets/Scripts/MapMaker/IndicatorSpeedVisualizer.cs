using NavyTap.Maps;
using UnityEngine;

namespace NavyTap.MapMaker
{
    public class IndicatorSpeedVisualizer : MonoBehaviour
    {
        [SerializeField] IndicatorVisualizer indicatorVisualizer = null;
        [SerializeField] Transform perfectScoreMax = null;
        [SerializeField] Transform perfectScoreMin = null;

        public void ChangePerfectScore(float perfectScore, float perfectScoreDifference)
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

        public void ChangeSpeedFromFilespan(float lifespan)
        {
            if (indicatorVisualizer != null)
            {
                indicatorVisualizer.ChangeSpeedFromFilespan(lifespan);
            }
        }

        public void AnimateIndicator(float deltaTime)
        {
            if (indicatorVisualizer != null)
            {
                indicatorVisualizer.AnimateIndicator(deltaTime);
            }
        }
    }
}