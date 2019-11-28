using PadTap.Core;
using System.Collections.Generic;
using UnityEngine;

namespace PadTap.MapMaker
{
    public class Timeline : MonoBehaviour
    {
        [SerializeField] TimelinePoint pointPrefab = null;
        [SerializeField] RectTransform rect = null;
        
        Dictionary<Map.Point, TimelinePoint> points = null;

        public void UpdatePoints(Map map)
        {
            if (map != null && map.points != null && map.song != null)
            {
                foreach (Map.Point point in map.points)
                {
                    AddPoint(point, map.song.length);
                }
                DeleteUnnecesaryPoints(map.points);
            }
            else
            {
                if (map == null)
                {
                    Debug.LogError(typeof(Map) + " received is null");
                }
                if (map.points == null)
                {
                    Debug.LogError(typeof(List<Map.Point>) + " in " + map.name + " map is null");
                }
                if (map.song == null)
                {
                    Debug.LogError(typeof(AudioClip) + " in " + map.name + " map is null");
                }
            }
        }

        private void AddPoint(Map.Point point, float songLength)
        {
            float timePercentage = point.time / songLength;
            if (pointPrefab != null && rect != null)
            {
                if (points == null)
                {
                    points = new Dictionary<Map.Point, TimelinePoint>();
                }
                if (!points.ContainsKey(point))
                {
                    TimelinePoint timelinePoint = Instantiate(pointPrefab, transform);
                    float timelineWidth = rect.rect.width * rect.localScale.x;
                    float positionOnTimeline = timePercentage * timelineWidth;
                    timelinePoint.SetPoint(positionOnTimeline);
                    points.Add(point, timelinePoint);
                }
            }
            else
            {
                if(pointPrefab == null)
                {
                    Logger.NotAssigned(typeof(TimelinePoint), GetType(), name);
                }
                if(rect == null)
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
                foreach (Map.Point point in points.Keys)
                {
                    if (!mapPoints.Contains(point))
                    {
                        points[point].DeletePoint();
                        points.Remove(point);
                    }
                }
            }
            else
            {
                Debug.LogError(typeof(List<Map.Point>) + " received is null");
            }
        }

        public void ZoomIn()
        {
            rect.localScale *= 2;
            RefreshPoints(2);
        }

        public void ZoomOut()
        {
            if (rect.localScale != Vector3.one)
            {
                rect.localScale /= 2;
                RefreshPoints(.5f);
            }
        }

        public void RefreshPoints(float zoomDifference)
        {
            if (points != null)
            {
                foreach (TimelinePoint point in points.Values)
                {
                    float newPosition = point.transform.localPosition.x * zoomDifference;
                    point.SetPoint(newPosition);
                }
            }
        }
    }
}