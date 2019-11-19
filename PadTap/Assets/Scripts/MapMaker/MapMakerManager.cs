using PadTap.Maps;
using UnityEngine;

public class MapMakerManager : MonoBehaviour
{
    [SerializeField] IndicatorVisualizer indicatorVisualizer = null;
    [SerializeField] Timeline timeline = null;
    [SerializeField] TileSpawner tileSpawner = null;

    public void SetVisibleTiles(int rows, int columns)
    {
        tileSpawner.ShowTiles(rows, columns);
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

    public void ResetPoints()
    {
        timeline.MakeNewList();
    }

    public void CreatePoint(float timePercentage)
    {
        timeline.AddPoint(timePercentage);
    }
}
