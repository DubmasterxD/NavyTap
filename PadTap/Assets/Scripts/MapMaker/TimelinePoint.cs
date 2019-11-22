using UnityEngine;

namespace PadTap.MapMaker
{
    public class TimelinePoint : MonoBehaviour
    {
        [SerializeField] RectTransform rect = null;

        public void SetPoint(float positionOnTimeline)
        {
            rect.localPosition = new Vector3(positionOnTimeline, 0);
        }
    }
}