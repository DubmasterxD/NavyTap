using PadTap.Maps;
using UnityEngine;

namespace PadTap.MapMaker
{
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

        public void ChangePerfectScore(float perfectScore, float perfectScoreDifference)
        {
            indicatorVisualizer.ChangePerfectScore(perfectScore, perfectScoreDifference);
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
}