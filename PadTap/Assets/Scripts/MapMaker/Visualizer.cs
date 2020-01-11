using PadTap.Core;
using PadTap.Maps;
using System.Collections.Generic;
using UnityEngine;

namespace PadTap.MapMaker
{
    public class Visualizer : MonoBehaviour
    {
        [SerializeField] IndicatorSpeedVisualizer indicatorSpeedVisualizer = null;
        [SerializeField] Timelines timelines = null;
        [SerializeField] TileSpawner tileSpawner = null;

        public void ManualUpdate(Map map, float time, float deltaTime, List<Map.Point> selection)
        {
            SetVisibleTiles(map.tilesRows, map.tilesColumns);
            ChangePerfectScore(map.GetPerfectScore(), map.GetPerfectScoreAcceptableDifference());
            ChangeSpeedFromFilespan(map.indicatorLifespan);
            Animate(deltaTime);
            UpdateTimelinesPoints(map, selection);
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
                Debug.LogError(Logger.NotAssigned(typeof(TileSpawner), GetType(), name));
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
                Debug.LogError(Logger.NotAssigned(typeof(IndicatorSpeedVisualizer), GetType(), name));
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
                Debug.LogError(Logger.NotAssigned(typeof(IndicatorSpeedVisualizer), GetType(), name));
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
                Debug.LogError(Logger.NotAssigned(typeof(IndicatorSpeedVisualizer), GetType(), name));
            }
        }

        public void UpdateTimelinesPoints(Map map, List<Map.Point> points)
        {
            if (timelines != null)
            {
                timelines.UpdatePoints(map, points);
            }
            else
            {
                Debug.LogError(Logger.NotAssigned(typeof(Timelines), GetType(), name));
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
                Debug.LogError(Logger.NotAssigned(typeof(Timelines), GetType(), name));
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
                Debug.LogError(Logger.NotAssigned(typeof(Timelines), GetType(), name));
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
                Debug.LogError(Logger.NotAssigned(typeof(Timelines), GetType(), name));
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
                Debug.LogError(Logger.NotAssigned(typeof(Timelines), GetType(), name));
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
                Debug.LogError(Logger.NotAssigned(typeof(Timelines), GetType(), name));
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
                Debug.LogError(Logger.NotAssigned(typeof(Timelines), GetType(), name));
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
                Debug.LogError(Logger.NotAssigned(typeof(Timelines), GetType(), name));
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
                Debug.LogError(Logger.NotAssigned(typeof(Timelines), GetType(), name));
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
                Debug.LogError(Logger.NotAssigned(typeof(Timelines), GetType(), name));
            }
        }

        public void ResetMap()
        {
            ResetTimelineZoom();
        }

        public void ShowSelection(float selectionStartTime, float selectionEndTime, float songLength)
        {
            if (timelines != null)
            {
                timelines.ShowSelection(selectionStartTime, selectionEndTime, songLength);
            }
            else
            {
                Debug.LogError(Logger.NotAssigned(typeof(Timelines), GetType(), name));
            }
        }
    }
}