using PadTap.Core;
using PadTap.Maps;
using System.Collections.Generic;
using UnityEngine;

namespace PadTap.MapMaker
{
    public class MapMakerVisualizerManager : MonoBehaviour
    {
        [SerializeField] IndicatorSpeedVisualizer indicatorSpeedVisualizer = null;
        [SerializeField] Timeline timeline = null;
        [SerializeField] Timeline zoomableTimeline = null;
        [SerializeField] TileSpawner tileSpawner = null;

        public void ManualUpdate(Map map, float time, float deltaTime)
        {
            SetVisibleTiles(map.tilesRows, map.tilesColumns);
            ChageThreshold(map.threshold);
            ChangePerfectScore(map.GetPerfectScore(), map.GetPerfectScoreAcceptableDifference());
            ChangeSpeedFromFilespan(map.indicatorLifespan);
            Animate(deltaTime);
            UpdatePoints(map);
            UpdateTime(time, map);
        }

        public void SetVisibleTiles(int rows, int columns)
        {
            if (tileSpawner != null)
            {
                tileSpawner.ShowTiles(rows, columns);
            }
            else
            {
                Logger.NotAssigned(typeof(TileSpawner), GetType(), name);
            }
        }

        public void ChageThreshold(float threshold)
        {
            if (indicatorSpeedVisualizer != null)
            {
                indicatorSpeedVisualizer.ChangeThreshold(threshold);
            }
            else
            {
                Logger.NotAssigned(typeof(IndicatorSpeedVisualizer), GetType(), name);
            }
        }

        public void ChangePerfectScore(float perfectScore, float perfectScoreDifference)
        {
            if (indicatorSpeedVisualizer != null)
            {
                indicatorSpeedVisualizer.ChangePerfectScore(perfectScore, perfectScoreDifference);
            }
            else
            {
                Logger.NotAssigned(typeof(IndicatorSpeedVisualizer), GetType(), name);
            }
        }

        public void ChangeSpeedFromFilespan(float lifespan)
        {
            if (indicatorSpeedVisualizer != null)
            {
                indicatorSpeedVisualizer.ChangeSpeedFromFilespan(lifespan);
            }
            else
            {
                Logger.NotAssigned(typeof(IndicatorSpeedVisualizer), GetType(), name);
            }
        }

        public void Animate(float deltaTime)
        {
            if (indicatorSpeedVisualizer != null)
            {
                indicatorSpeedVisualizer.AnimateIndicator(deltaTime);
            }
            else
            {
                Logger.NotAssigned(typeof(IndicatorSpeedVisualizer), GetType(), name);
            }
        }

        public void UpdatePoints(Map map)
        {
            if (timeline != null)
            {
                timeline.UpdatePoints(map);
            }
            else
            {
                Logger.NotAssigned(typeof(Timeline), GetType(), name);
            }
            if (zoomableTimeline != null)
            {
                zoomableTimeline.UpdatePoints(map);
            }
            else
            {
                Logger.NotAssigned(typeof(Timeline), GetType(), name);
            }
        }

        public void UpdateTime(float time, Map map)
        {
            if (zoomableTimeline != null)
            {
                zoomableTimeline.UpdateTime(time, map);
            }
            else
            {
                Logger.NotAssigned(typeof(Timeline), GetType(), name);
            }
        }

        public void ZoomIn()
        {
            if (zoomableTimeline != null)
            {
                zoomableTimeline.ZoomIn();
            }
            else
            {
                Logger.NotAssigned(typeof(Timeline), GetType(), name);
            }
        }

        public void ZoomOut()
        {
            if (zoomableTimeline != null)
            {
                zoomableTimeline.ZoomOut();
            }
            else
            {
                Logger.NotAssigned(typeof(Timeline), GetType(), name);
            }
        }
    }
}