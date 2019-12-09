using PadTap.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PadTap.MapMaker
{
    public class Timeline : MonoBehaviour
    {
        [SerializeField] TimelinePoint pointPrefab = null;
        [SerializeField] Transform space = null;
        [SerializeField] LineRenderer lineRenderer = null;

        float startingWaveVerticalScale = 1;
        int zoom = 1;
        Coroutine loadingMap;

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
                    Logger.ReceivedNull(typeof(Map));
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
            if (pointPrefab != null && space != null)
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
                    float positionOnTimeline = timePercentage * space.localScale.x * 10 / zoom;
                    timelinePoint.SetPoint(positionOnTimeline);
                    points.Add(point, timelinePoint);
                    RefreshPointsScale(zoom);
                }
            }
            else
            {
                if(pointPrefab == null)
                {
                    Logger.NotAssigned(typeof(TimelinePoint), GetType(), name);
                }
                if(space == null)
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
                List<Map.Point> pointsToRemove = new List<Map.Point>();
                foreach (Map.Point point in points.Keys)
                {
                    if (!mapPoints.Contains(point))
                    {
                        TimelinePoint pointToDelete = points[point];
                        pointsToRemove.Add(point);
                        pointToDelete.DeletePoint();
                    }
                }
                foreach (Map.Point point in pointsToRemove)
                {
                    points.Remove(point);
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
                Logger.ReceivedNull(typeof(List<Map.Point>));
            }
        }

        public void UpdateTime(float time, Map map)
        {
            float timelineWidth = space.localScale.x * 10;
            float timePercentage = time / map.song.length;
            float newPosition = -timePercentage * timelineWidth;
            transform.localPosition = new Vector3(newPosition, 0, 0);
        }

        public void ZoomIn()
        {
            RefreshPointsScale(zoom * 2);
        }

        public void ZoomOut()
        {
            if (space.localScale != Vector3.one)
            {
                RefreshPointsScale(zoom / 2);
            }
        }

        private void RefreshPointsScale(int newScale)
        {
            zoom = newScale;
            space.localScale = new Vector3(zoom, 1, 1);
            foreach (TimelinePoint point in points.Values)
            {
                point.transform.localScale = new Vector3(1f / zoom, 1, 1);
            }
        }

        public void VerticalZoomIn()
        {
            lineRenderer.transform.localScale = new Vector3(1, lineRenderer.transform.localScale.y * 2, 1);
        }

        public void VerticalZoomOut()
        {
            if (lineRenderer.transform.localScale.y != startingWaveVerticalScale)
            {
                lineRenderer.transform.localScale = new Vector3(1, lineRenderer.transform.localScale.y / 2, 1);
            }
        }

        public void CreateAudioWaveform(AudioClip song)
        {
            if (loadingMap != null)
            {
                StopCoroutine(loadingMap);
            }
            loadingMap = StartCoroutine(CreateAudioWaveformc(song));
        }

        private IEnumerator CreateAudioWaveformc(AudioClip song)
        {
            if (song != null)
            {
                int samples = song.samples;
                float[] data = new float[samples * 2];
                song.GetData(data, 0);
                float sum = 0;
                lineRenderer.positionCount = 0;
                List<Vector3> dataa = new List<Vector3>();
                float max = 0;
                for (int i = 0; i < data.Length; i++)
                {
                    sum += Mathf.Abs(data[i]);
                    if (i % 200 == 0)
                    {
                        dataa.Add(new Vector3(i / (float)song.frequency * 5 / song.length, sum / 200, 0));
                        sum = 0;
                        lineRenderer.positionCount += 1;
                        lineRenderer.SetPosition(lineRenderer.positionCount - 1, dataa[lineRenderer.positionCount - 1]);
                        if (dataa[lineRenderer.positionCount - 1].y > max)
                        {
                            max = dataa[lineRenderer.positionCount - 1].y;
                            lineRenderer.transform.localScale = new Vector3(1, 1 / max, 1);
                        }
                        if (i % 100000 == 0)
                        {
                            yield return null;
                        }
                    }
                }
                lineRenderer.transform.localScale = new Vector3(1, 1 / max, 1);
                startingWaveVerticalScale = 1 / max;
            }
            else
            {
                Logger.ReceivedNull(typeof(AudioClip));
            }
        }

        public void MoveUp()
        {
            lineRenderer.transform.localPosition = new Vector3(lineRenderer.transform.localPosition.x, lineRenderer.transform.localPosition.y - 1 / 2f, lineRenderer.transform.localPosition.z);
        }

        public void MoveDown()
        {
            if (lineRenderer.transform.localPosition.y != -0.5)
            {
                lineRenderer.transform.localPosition = new Vector3(lineRenderer.transform.localPosition.x, lineRenderer.transform.localPosition.y + 1 / 2f, lineRenderer.transform.localPosition.z);
            }
        }

        public void ResetZoom()
        {
            lineRenderer.transform.localPosition = new Vector3(lineRenderer.transform.localPosition.x, -0.5f, 0);
            lineRenderer.transform.localScale = new Vector3(1, startingWaveVerticalScale, 1);
            RefreshPointsScale(1);
        }
    }
}