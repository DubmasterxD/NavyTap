using PadTap.Maps;
using UnityEngine;

public class IndicatorVisualizer : MonoBehaviour
{
    [SerializeField] Indicator indicator = null;
    [SerializeField] Threshold threshold = null;

    private float indicatorSpeed = 1f;
    private float indicatorTime = 0f;

    public void ChangeSpeedFromFilespan(float lifespan)
    {
        indicatorSpeed = 1 / lifespan;
    }

    public void ChangeThreshold(float value)
    {
        if (threshold != null)
        {
            threshold.SetThreshold(value);
        }
    }

    public void Animate(float deltaTime)
    {
        AnimateIndicator(deltaTime);
    }

    private void AnimateIndicator(float deltaTime)
    {
        indicatorTime += deltaTime * indicatorSpeed;
        if (indicatorTime > 1)
        {
            indicatorTime -= 1;
        }
        indicator.GetComponentInChildren<SpriteRenderer>().transform.localScale = new Vector3(indicatorTime, indicatorTime, indicatorTime);
    }
}
