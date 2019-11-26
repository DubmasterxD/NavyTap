using System.Collections.Generic;
using UnityEngine;

namespace PadTap.MapMaker
{
    public class Timeline : MonoBehaviour
    {
        [SerializeField] TimelinePoint pointPrefab = null;
        [SerializeField] RectTransform rect = null;

        float zoom = 1;
        List<TimelinePoint> points = null;

        public void AddPoint(float timePercentage)
        {
            if (pointPrefab != null && rect != null)
            {
                if (points == null)
                {
                    points = new List<TimelinePoint>();
                }
                TimelinePoint point = Instantiate(pointPrefab, transform);
                float timelineWidth = rect.rect.width * rect.localScale.x;
                float positionOnTimeline = timePercentage * timelineWidth;
                point.SetPoint(positionOnTimeline);
                points.Add(point);
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

        public void ClearList()
        {
            if (points != null)
            {
                foreach (TimelinePoint point in points)
                {
                    Destroy(point);
                }
            }
            points = new List<TimelinePoint>();
        }

        public void ZoomIn()
        {
            zoom *= 2;
            RefreshPoints(2);
        }

        public void ZoomOut()
        {
            if (zoom != 1)
            {
                zoom /= 2;
                RefreshPoints(.5f);
            }
        }

        public void RefreshPoints(float zoomDifference)
        {
            if (points != null)
            {
                foreach (TimelinePoint point in points)
                {
                    float newPosition = point.transform.localPosition.x * zoomDifference;
                    point.SetPoint(newPosition);
                }
            }
        }
    }
}