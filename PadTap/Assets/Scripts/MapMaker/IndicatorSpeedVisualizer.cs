using PadTap.Maps;
using UnityEngine;

namespace PadTap.MapMaker
{
    public class IndicatorSpeedVisualizer : MonoBehaviour
    {
        [SerializeField] IndicatorVisualizer indicatorVisualizer = null;
        [SerializeField] Threshold threshold = null;

        public void ChangeThreshold(float value)
        {
            if (threshold != null)
            {
                threshold.SetThreshold(value);
            }
        }

        public void ChangePerfectScore(float perfectScore, float perfectScoreDifference)
        {
            if (threshold != null)
            {
                threshold.SetPerfectScore(perfectScore, perfectScoreDifference);
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