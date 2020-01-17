using NavyTap.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NavyTap.MapMaker
{
    public class Timelines : MonoBehaviour
    {
        [SerializeField] Timeline timeline = null;
        [SerializeField] Timeline zoomableTimeline = null;
        [SerializeField] Text currentTime = null;
        [SerializeField] Transform pointer = null;

        public void UpdatePoints(Map map, List<Map.Point> points)
        {
            if (timeline != null)
            {
                timeline.UpdatePoints(map, points);
            }
            else
            {
                Debug.LogError(Logger.NotAssigned(typeof(Timeline), GetType(), name));
            }
            if (zoomableTimeline != null)
            {
                zoomableTimeline.UpdatePoints(map, points);
            }
            else
            {
                Debug.LogError(Logger.NotAssigned(typeof(Timeline), GetType(), name));
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
                Debug.LogError(Logger.NotAssigned(typeof(Text), GetType(), name));
            }
            if (pointer != null)
            {
                pointer.localPosition = new Vector3(10 * time / map.song.length, -0.5f, 0);
            }
            else
            {
                Debug.LogError(Logger.NotAssigned(typeof(Transform), GetType(), name));
            }
            if (zoomableTimeline != null)
            {
                zoomableTimeline.UpdateTime(time, map);
            }
            else
            {
                Debug.LogError(Logger.NotAssigned(typeof(Timeline), GetType(), name));
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
                Debug.LogError(Logger.NotAssigned(typeof(Timeline), GetType(), name));
            }
            if (zoomableTimeline != null)
            {
                zoomableTimeline.CreateAudioWaveform(song);
            }
            else
            {
                Debug.LogError(Logger.NotAssigned(typeof(Timeline), GetType(), name));
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
                Debug.LogError(Logger.NotAssigned(typeof(Timeline), GetType(), name));
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
                Debug.LogError(Logger.NotAssigned(typeof(Timeline), GetType(), name));
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
                Debug.LogError(Logger.NotAssigned(typeof(Timeline), GetType(), name));
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
                Debug.LogError(Logger.NotAssigned(typeof(Timeline), GetType(), name));
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
                Debug.LogError(Logger.NotAssigned(typeof(Timeline), GetType(), name));
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
                Debug.LogError(Logger.NotAssigned(typeof(Timeline), GetType(), name));
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
                Debug.LogError(Logger.NotAssigned(typeof(Timeline), GetType(), name));
            }
        }

        public void ShowSelection(float selectionStartTime, float selectionEndTime, float songLength)
        {
            if (timeline != null)
            {
                timeline.ShowSelection(selectionStartTime, selectionEndTime, songLength);
            }
            else
            {
                Debug.LogError(Logger.NotAssigned(typeof(Timelines), GetType(), name));
            }
            if (zoomableTimeline != null)
            {
                zoomableTimeline.ShowSelection(selectionStartTime, selectionEndTime, songLength);
            }
            else
            {
                Debug.LogError(Logger.NotAssigned(typeof(Timelines), GetType(), name));
            }
        }
    }
}