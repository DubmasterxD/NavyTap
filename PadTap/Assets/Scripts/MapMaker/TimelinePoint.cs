using UnityEngine;

namespace PadTap.MapMaker
{
    public class TimelinePoint : MonoBehaviour
    {
        public void SetPoint(float positionOnTimeline)
        {
            if (transform != null)
            {
                transform.localPosition = new Vector3(positionOnTimeline, 0);
            }
            else
            {
                Logger.NotAssigned(typeof(RectTransform), GetType(), name);
            }
        }

        public void DeletePoint()
        {
            DestroyImmediate(gameObject);
        }
    }
}