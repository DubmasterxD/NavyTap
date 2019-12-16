using PadTap.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PadTap.MapMaker
{
    public class Timeline : MonoBehaviour
    {
        [SerializeField] TimelinePoint pointPrefab = null;
        [SerializeField] TimelinePoint copiedPrefab = null;
        [SerializeField] Transform space = null;
        [SerializeField] Transform selection = null;
        [SerializeField] Waveform waveform = null;

        int zoom = 1;

        Dictionary<Map.Point, TimelinePoint> points = null;
        Dictionary<Map.Point, TimelinePoint> copied = null;

        public void UpdatePoints(Map map, List<Map.Point> selection)
        {
            if (map != null && map.points != null && map.song != null)
            {
                foreach (Map.Point point in map.points)
                {
                    AddPoint(point, pointPrefab, points, map.song.length);
                }
                DeleteUnnecesaryPoints(map.points);
                if (selection != null)
                {
                    foreach(Map.Point point in selection)
                    {
                        AddPoint(point, copiedPrefab, copied, map.song.length);
                    }
                }
                RefreshPointsScale(zoom);
            }
            else
            {
                if (map == null)
                {
                    Logger.ReceivedNull(typeof(Map));
                }
                if (map.points == null)
                {
                    Logger.ReceivedNull(typeof(List<Map.Point>));
                }
                if (map.song == null)
                {
                    Debug.LogError(typeof(AudioClip) + " in " + map.mapName + " map is null");
                }
            }
        }

        private void AddPoint(Map.Point point, TimelinePoint prefab, Dictionary<Map.Point, TimelinePoint> dict, float songLength)
        {
            float timePercentage = point.time / songLength;
            float positionOnTimeline = timePercentage * space.localScale.x * 10 / zoom;
            if (prefab != null && space != null)
            {
                if (dict == null)
                {
                    dict = new Dictionary<Map.Point, TimelinePoint>();
                }
                if (dict.ContainsKey(point) && dict[point] == null)
                {
                    dict.Remove(point);
                }
                if (!dict.ContainsKey(point))
                {
                    TimelinePoint timelinePoint = Instantiate(prefab, transform);
                    timelinePoint.SetPoint(positionOnTimeline);
                    dict.Add(point, timelinePoint);
                }
            }
            else
            {
                if (prefab == null)
                {
                    Logger.NotAssigned(typeof(TimelinePoint), GetType(), name);
                }
                if (space == null)
                {
                    Logger.NotAssigned(typeof(RectTransform), GetType(), name);
                }
            }
        }

        private void DeleteUnnecesaryPoints(List<Map.Point> mapPoints)
        {
            if (points == null)
            {
                points = new Dictionary<Map.Point, TimelinePoint>();
            }
            if (mapPoints != null)
            {
                List<Map.Point> pointsToRemove = new List<Map.Point>();
                foreach (Map.Point point in points.Keys)
                {
                    if (!mapPoints.Contains(point))
                    {
                        TimelinePoint pointToDelete = points[point];
                        pointsToRemove.Add(point);
                        pointToDelete.DeletePoint();
                    }
                }
                foreach (Map.Point point in pointsToRemove)
                {
                    points.Remove(point);
                }
                TimelinePoint[] childs = transform.GetComponentsInChildren<TimelinePoint>();







                if (childs.Length > points.Count)
                {
                    points.Clear();
                    foreach (TimelinePoint point in childs)
                    {
                        point.DeletePoint();
                    }
                }
            }
            else
            {
                Logger.ReceivedNull(typeof(List<Map.Point>));
            }
        }

        public void UpdateTime(float time, Map map)
        {
            float timelineWidth = space.localScale.x * 10;
            float timePercentage = time / map.song.length;
            float newPosition = -timePercentage * timelineWidth;
            transform.localPosition = new Vector3(newPosition, 0, 0);
        }

        public void ZoomIn()
        {
            RefreshPointsScale(zoom * 2);
        }

        public void ZoomOut()
        {
            if (space.localScale != Vector3.one)
            {
                RefreshPointsScale(zoom / 2);
            }
        }

        private void RefreshPointsScale(int newScale)
        {
            zoom = newScale;
            space.localScale = new Vector3(zoom, 1, 1);
            if (points != null)
            {
                foreach (TimelinePoint point in points.Values)
                {
                    point.transform.localScale = new Vector3(1f / zoom, 1, 1);
                }
            }
            if (copied != null)
            {
                foreach (TimelinePoint point in copied.Values)
                {
                    point.transform.localScale = new Vector3(1f / zoom, 1, 1);
                }
            }
        }

        public void ShowSelection(float selectionStartTime, float selectionEndTime, float songLength)
        {
            if (selection != null)
            {
                if (selectionEndTime > selectionStartTime)
                {
                    selection.gameObject.SetActive(true);
                    float timePercentage = selectionStartTime / songLength;
                    float positionOnTimeline = timePercentage * 10;
                    selection.localPosition = new Vector3(positionOnTimeline, 0, 0);
                    float scale = (selectionEndTime - selectionStartTime) / songLength * 1000;
                    selection.localScale = new Vector3(scale, 1, 1);
                }
                else
                {
                    selection.gameObject.SetActive(false);
                }
            }
            else
            {
                Logger.NotAssigned(typeof(Transform), GetType(), name);
            }
        }

        public void VerticalZoomIn()
        {
            waveform.VerticalZoomIn();
        }

        public void VerticalZoomOut()
        {
            waveform.VerticalZoomOut();
        }

        public void CreateAudioWaveform(AudioClip song)
        {
            waveform.CreateAudioWaveform(song);
        }

        public void MoveUp()
        {
            waveform.MoveUp();
        }

        public void MoveDown()
        {
            waveform.MoveDown();
        }

        public void ResetZoom()
        {
            waveform.ResetZoom();
            RefreshPointsScale(1);
        }
    }
}