using PadTap.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
                    Debug.LogError(typeof(List<Map.Point>) + " in " + map.mapName + " map is null");
                }
                if (map.song == null)
                {
                    Debug.LogError(typeof(AudioClip) + " in " + map.mapName + " map is null");
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
                if(points.ContainsKey(point) && points[point] == null)
                {
                    points.Remove(point);
                }
                if (!points.ContainsKey(point))
                {
                    TimelinePoint timelinePoint = Instantiate(pointPrefab, transform);
                    float positionOnTimeline = timePercentage * rect.rect.width;
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
                        TimelinePoint pointToDelete = points[point];
                        points.Remove(point);
                        pointToDelete.DeletePoint();
                    }
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
                Debug.LogError(typeof(List<Map.Point>) + " received is null");
            }
        }

        public void UpdateTime(float time, Map map)
        {
            float timelineWidth = rect.rect.width * rect.localScale.x;
            float timePercentage = time / map.song.length;
            float newPosition = -timePercentage * timelineWidth;
            transform.localPosition = new Vector3(newPosition, 0, 0);
        }

        public void ZoomIn()
        {
            rect.localScale = new Vector3(rect.localScale.x * 2, 1, 1);
        }

        public void ZoomOut()
        {
            if (rect.localScale != Vector3.one)
            {
                rect.localScale = new Vector3(rect.localScale.x / 2, 1, 1);
            }
        }

        public void CreateAudioWaveform(AudioClip song)
        {
            StartCoroutine(CreateAudioWaveformc(song));
        }

        private IEnumerator CreateAudioWaveformc(AudioClip song)
        {
            if (song != null)
            {
                int samples = song.samples;
                float[] data = new float[samples];
                song.GetData(data, 0);
                float sum = 0;
                List<Vector3> dataa = new List<Vector3>();
                for (int i = 0; i < samples; i++)
                {
                    sum += Mathf.Abs(data[i]);
                    if (i % 480 == 0)
                    {
                        dataa.Add(new Vector3(i / 48000, sum / 480 * 100, 0));
                        sum = 0;
                        yield return null;
                    }
                }
            }
            else
            {
                Debug.LogError(typeof(AudioClip) + " received is null");
            }
        }
    }
}