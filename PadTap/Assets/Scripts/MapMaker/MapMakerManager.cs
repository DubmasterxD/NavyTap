using UnityEngine;

public class MapMakerManager : MonoBehaviour
{
    [SerializeField] IndicatorVisualizer indicatorVisualizer = null;
    
    public void ChangeSpeedFromFilespan(float lifespan)
    {
        indicatorVisualizer.ChangeSpeedFromFilespan(lifespan);
    }

    public void ChageThreshold(float threshold)
    {
        indicatorVisualizer.ChangeThreshold(threshold);
    }

    public void Animate(float deltaTime)
    {
        indicatorVisualizer.Animate(deltaTime);
    }
}
