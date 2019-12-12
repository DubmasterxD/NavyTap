using PadTap.Core;
using PadTap.Maps;
using UnityEngine;

namespace PadTap.MapMaker
{
    public class MapMakerVisualizerManager : MonoBehaviour
    {
        [SerializeField] IndicatorSpeedVisualizer indicatorSpeedVisualizer = null;
        [SerializeField] Timelines timelines = null;
        [SerializeField] TileSpawner tileSpawner = null;

        public void ManualUpdate(Map map, float time, float deltaTime)
        {
            SetVisibleTiles(map.tilesRows, map.tilesColumns);
            ChageThreshold(map.threshold);
            ChangePerfectScore(map.GetPerfectScore(), map.GetPerfectScoreAcceptableDifference());
            ChangeSpeedFromFilespan(map.indicatorLifespan);
            Animate(deltaTime);
            UpdateTimelinesPoints(map);
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

        public void UpdateTimelinesPoints(Map map)
        {
            if (timelines != null)
            {
                timelines.UpdatePoints(map);
            }
            else
            {
                Logger.NotAssigned(typeof(Timelines), GetType(), name);
            }
        }

        public void UpdateTime(float time, Map map)
        {
            if (timelines != null)
            {
                timelines.UpdateTime(time, map);
            }
            else
            {
                Logger.NotAssigned(typeof(Timelines), GetType(), name);
            }
        }

        public void ChangeSong(AudioClip song)
        {
            if (timelines != null)
            {
                timelines.ChangeSong(song);
            }
            else
            {
                Logger.NotAssigned(typeof(Timelines), GetType(), name);
            }
        }

        public void ZoomInTimeline()
        {
            if (timelines != null)
            {
                timelines.ZoomIn();
            }
            else
            {
                Logger.NotAssigned(typeof(Timelines), GetType(), name);
            }
        }

        public void ZoomOutTimeline()
        {
            if (timelines != null)
            {
                timelines.ZoomOut();
            }
            else
            {
                Logger.NotAssigned(typeof(Timelines), GetType(), name);
            }
        }

        public void VerticalZoomInTimeline()
        {
            if (timelines != null)
            {
                timelines.VerticalZoomIn();
            }
            else
            {
                Logger.NotAssigned(typeof(Timelines), GetType(), name);
            }
        }

        public void VerticalZoomOutTimeline()
        {
            if (timelines != null)
            {
                timelines.VerticalZoomOut();
            }
            else
            {
                Logger.NotAssigned(typeof(Timelines), GetType(), name);
            }
        }

        public void MoveUpTimeline()
        {
            if (timelines != null)
            {
                timelines.MoveTimelineUp();
            }
            else
            {
                Logger.NotAssigned(typeof(Timelines), GetType(), name);
            }
        }

        public void MoveDownTimeline()
        {
            if (timelines != null)
            {
                timelines.MoveTimeLineDown();
            }
            else
            {
                Logger.NotAssigned(typeof(Timelines), GetType(), name);
            }
        }

        public void ResetTimelineZoom()
        {
            if (timelines != null)
            {
                timelines.ResetTimelineZoom();
            }
            else
            {
                Logger.NotAssigned(typeof(Timelines), GetType(), name);
            }
        }

        public void ResetMap()
        {
            ResetTimelineZoom();
        }
    }
}