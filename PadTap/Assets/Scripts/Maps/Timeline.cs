using System.Collections.Generic;
using UnityEngine;

namespace PadTap.Maps
{
    public class Timeline : MonoBehaviour
    {
        [SerializeField] TimelinePoint pointPrefab = null;

        List<TimelinePoint> points = null;
        RectTransform rect = null;

        private void Awake()
        {
            rect = transform.parent.GetComponent<RectTransform>();
        }

        public void MakeNewList()
        {
            foreach (TimelinePoint point in points)
            {
                Destroy(point);
            }
            points = new List<TimelinePoint>();
        }

        public void AddPoint(float timePercentage)
        {
            TimelinePoint point = Instantiate(pointPrefab, transform);
            float timelineWidth = rect.rect.width * rect.localScale.x;
            float positionOnTimeline = timePercentage * timelineWidth;
            point.SetPoint(positionOnTimeline);
        }
    }
}