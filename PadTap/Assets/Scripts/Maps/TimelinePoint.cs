using UnityEngine;

namespace PadTap.Maps
{
    public class TimelinePoint : MonoBehaviour
    {
        RectTransform rect = null;

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
        }

        public void SetPoint(float positionOnTimeline)
        {
            rect.localPosition = new Vector3(positionOnTimeline, 0);
        }
    }
}