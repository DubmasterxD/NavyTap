using PadTap.Core;
using PadTap.Maps;
using UnityEngine;

namespace PadTap.MapMaker
{
    public class MapMakerVisualizerManager : MonoBehaviour
    {
        [SerializeField] IndicatorSpeedVisualizer indicatorSpeedVisualizer = null;
        [SerializeField] Timeline timeline = null;
        [SerializeField] TileSpawner tileSpawner = null;

        public void ManualUpdate(Map map, float time, float deltaTime)
        {
            SetVisibleTiles(map.tilesRows, map.tilesColumns);
            ChageThreshold(map.threshold);
            ChangePerfectScore(map.GetPerfectScore(), map.GetPerfectScoreAcceptableDifference());
            ChangeSpeedFromFilespan(map.indicatorLifespan);
            Animate(deltaTime);
        }

        public void SetVisibleTiles(int rows, int columns)
        {
            if (tileSpawner != null)
            {
                tileSpawner.ShowTiles(rows, columns);
            }
        }

        public void ChageThreshold(float threshold)
        {
            if (indicatorSpeedVisualizer != null)
            {
                indicatorSpeedVisualizer.ChangeThreshold(threshold);
            }
        }

        public void ChangePerfectScore(float perfectScore, float perfectScoreDifference)
        {
            if (indicatorSpeedVisualizer != null)
            {
                indicatorSpeedVisualizer.ChangePerfectScore(perfectScore, perfectScoreDifference);
            }
        }

        public void ChangeSpeedFromFilespan(float lifespan)
        {
            if (indicatorSpeedVisualizer != null)
            {
                indicatorSpeedVisualizer.ChangeSpeedFromFilespan(lifespan);
            }
        }

        public void Animate(float deltaTime)
        {
            if (indicatorSpeedVisualizer != null)
            {
                indicatorSpeedVisualizer.AnimateIndicator(deltaTime);
            }
        }

        public void ResetPoints()
        {
            if (timeline != null)
            {
                timeline.ClearList();
            }
        }

        public void CreatePoint(float timePercentage)
        {
            if (timeline != null)
            {
                timeline.AddPoint(timePercentage);
            }
        }

        public void ZoomIn()
        {
            if (timeline != null)
            {
                timeline.ZoomIn();
            }
        }

        public void ZoomOut()
        {
            if (timeline != null)
            {
                timeline.ZoomOut();
            }
        }
    }
}