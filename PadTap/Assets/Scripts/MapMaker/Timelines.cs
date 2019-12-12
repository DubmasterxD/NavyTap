using PadTap.Core;
using UnityEngine;
using UnityEngine.UI;

namespace PadTap.MapMaker
{
    public class Timelines : MonoBehaviour
    {
        [SerializeField] Timeline timeline = null;
        [SerializeField] Timeline zoomableTimeline = null;
        [SerializeField] Text currentTime = null;
        [SerializeField] Transform pointer = null;

        public void UpdatePoints(Map map)
        {
            if (timeline != null)
            {
                timeline.UpdatePoints(map);
            }
            else
            {
                Logger.NotAssigned(typeof(Timeline), GetType(), name);
            }
            if (zoomableTimeline != null)
            {
                zoomableTimeline.UpdatePoints(map);
            }
            else
            {
                Logger.NotAssigned(typeof(Timeline), GetType(), name);
            }
        }

        public void UpdateTime(float time, Map map)
        {
            if (currentTime != null)
            {
                string cTime = string.Format("{0:00}:{1:00.####}", Mathf.Floor(time / 60), (time - 60 * Mathf.Floor(time / 60)));
                currentTime.text = cTime;
            }
            else
            {
                Logger.NotAssigned(typeof(Text), GetType(), name);
            }
            if (pointer != null)
            {
                pointer.localPosition = new Vector3(10 * time / map.song.length, -0.5f, 0);
            }
            else
            {
                Logger.NotAssigned(typeof(Transform), GetType(), name);
            }
            if (zoomableTimeline != null)
            {
                zoomableTimeline.UpdateTime(time, map);
            }
            else
            {
                Logger.NotAssigned(typeof(Timeline), GetType(), name);
            }
        }

        public void ChangeSong(AudioClip song)
        {
            if (timeline != null)
            {
                timeline.CreateAudioWaveform(song);
            }
            else
            {
                Logger.NotAssigned(typeof(Timeline), GetType(), name);
            }
            if (zoomableTimeline != null)
            {
                zoomableTimeline.CreateAudioWaveform(song);
            }
            else
            {
                Logger.NotAssigned(typeof(Timeline), GetType(), name);
            }
        }

        public void ZoomIn()
        {
            if (zoomableTimeline != null)
            {
                zoomableTimeline.ZoomIn();
            }
            else
            {
                Logger.NotAssigned(typeof(Timeline), GetType(), name);
            }
        }

        public void ZoomOut()
        {
            if (zoomableTimeline != null)
            {
                zoomableTimeline.ZoomOut();
            }
            else
            {
                Logger.NotAssigned(typeof(Timeline), GetType(), name);
            }
        }

        public void VerticalZoomIn()
        {
            if (zoomableTimeline != null)
            {
                zoomableTimeline.VerticalZoomIn();
            }
            else
            {
                Logger.NotAssigned(typeof(Timeline), GetType(), name);
            }
        }

        public void VerticalZoomOut()
        {
            if (zoomableTimeline != null)
            {
                zoomableTimeline.VerticalZoomOut();
            }
            else
            {
                Logger.NotAssigned(typeof(Timeline), GetType(), name);
            }
        }

        public void MoveTimelineUp()
        {
            if (zoomableTimeline != null)
            {
                zoomableTimeline.MoveUp();
            }
            else
            {
                Logger.NotAssigned(typeof(Timeline), GetType(), name);
            }
        }

        public void MoveTimeLineDown()
        {
            if (zoomableTimeline != null)
            {
                zoomableTimeline.MoveDown();
            }
            else
            {
                Logger.NotAssigned(typeof(Timeline), GetType(), name);
            }
        }

        public void ResetTimelineZoom()
        {
            if (zoomableTimeline != null)
            {
                zoomableTimeline.ResetZoom();
            }
            else
            {
                Logger.NotAssigned(typeof(Timeline), GetType(), name);
            }
        }
    }
}