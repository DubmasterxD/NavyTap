using UnityEngine;

namespace NavyTap.MapMaker
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
                Debug.LogError(Logger.NotAssigned(typeof(RectTransform), GetType(), name));
            }
        }

        public void DeletePoint()
        {
            DestroyImmediate(gameObject);
        }
    }
}