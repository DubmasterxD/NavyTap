using UnityEngine;

public class MapMakerManager : MonoBehaviour
{
    [SerializeField] IndicatorVisualizer indicatorVisualizer = null;
    [SerializeField] TileManager tileManager = null;

    public void SetVisibleTiles(int rows, int columns)
    {
        tileManager.ShowTiles(rows, columns);
    }

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
