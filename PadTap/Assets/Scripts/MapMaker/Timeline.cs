using PadTap.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PadTap.MapMaker
{
    public class Timeline : MonoBehaviour
    {
        [SerializeField] TimelinePoint pointPrefab = null;
        [SerializeField] TimelinePoint copiedPrefab = null;
        [SerializeField] Transform space = null;
        [SerializeField] Transform selection = null;
        [SerializeField] LineRenderer lineRenderer = null;

        float startingWaveVerticalScale = 1;
        int zoom = 1;
        Coroutine loadingMap;

        Dictionary<Map.Point, TimelinePoint> points = null;
        Dictionary<Map.Point, TimelinePoint> copied = null;

        public void UpdatePoints(Map map, List<Map.Point> newPoints)
        {
            if (newPoints != map.points)
            {
                ClearCopied();
            }
            if (map != null && newPoints != null && map.song != null)
            {
                foreach (Map.Point point in newPoints)
                {
                    AddPoint(point, map.song.length, newPoints != map.points);
                }
                if (newPoints == map.points)
                {
                    DeleteUnnecesaryPoints(map.points);
                }
                RefreshPointsScale(zoom);
            }
            else
            {
                if (map == null)
                {
                    Logger.ReceivedNull(typeof(Map));
                }
                if (map.points == null)
                {
                    Logger.ReceivedNull(typeof(List<Map.Point>));
                }
                if (map.song == null)
                {
                    Debug.LogError(typeof(AudioClip) + " in " + map.mapName + " map is null");
                }
            }
        }

        private void ClearCopied()
        {
            if (copied != null)
            {
                List<Map.Point> pointsToRemove = new List<Map.Point>();
                foreach (Map.Point point in copied.Keys)
                {
                    TimelinePoint pointToDelete = copied[point];
                    pointsToRemove.Add(point);
                    if (pointToDelete != null)
                    {
                        pointToDelete.DeletePoint();
                    }
                }
                foreach (Map.Point point in pointsToRemove)
                {
                    copied.Remove(point);
                }
            }
        }

        private void AddPoint(Map.Point point, float songLength, bool isCopied)
        {
            float timePercentage = point.time / songLength;
            float positionOnTimeline = timePercentage * space.localScale.x * 10 / zoom;
            if (pointPrefab != null && space != null && copiedPrefab!=null)
            {
                if (!isCopied)
                {
                    if (points == null)
                    {
                        points = new Dictionary<Map.Point, TimelinePoint>();
                    }
                    if (points.ContainsKey(point) && points[point] == null)
                    {
                        points.Remove(point);
                    }
                    if (!points.ContainsKey(point))
                    {
                        TimelinePoint timelinePoint = Instantiate(pointPrefab, transform);
                        timelinePoint.SetPoint(positionOnTimeline);
                        points.Add(point, timelinePoint);
                    }
                }
                else
                {
                    if (copied == null)
                    {
                        copied = new Dictionary<Map.Point, TimelinePoint>();
                    }
                    if (!copied.ContainsKey(point))
                    {
                        TimelinePoint timelinePoint = Instantiate(copiedPrefab, transform);
                        timelinePoint.SetPoint(positionOnTimeline);
                        copied.Add(point, timelinePoint);
                    }
                }
            }
            else
            {
                if (pointPrefab == null)
                {
                    Logger.NotAssigned(typeof(TimelinePoint), GetType(), name);
                }
                if (copiedPrefab == null)
                {
                    Logger.NotAssigned(typeof(TimelinePoint), GetType(), name);
                }
                if (space == null)
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
            if (points != null)
            {
                foreach (TimelinePoint point in points.Values)
                {
                    point.transform.localScale = new Vector3(1f / zoom, 1, 1);
                }
            }
            if (copied != null)
            {
                foreach (TimelinePoint point in copied.Values)
                {
                    if (point != null)
                    {
                        point.transform.localScale = new Vector3(1f / zoom, 1, 1);
                    }
                }
            }
        }

        public void VerticalZoomIn()
        {
            if (lineRenderer.transform.localScale.y / startingWaveVerticalScale < Mathf.Pow(2, 3))
            {
                lineRenderer.transform.localScale = new Vector3(1, lineRenderer.transform.localScale.y * 2, 1);
                float tmp = (lineRenderer.transform.localPosition.y + 0.5f) / -0.5f;
                for (int i = 0; i < tmp; i++)
                {
                    MoveUp();
                }
            }
        }

        public void VerticalZoomOut()
        {
            if (lineRenderer.transform.localScale.y != startingWaveVerticalScale)
            {
                lineRenderer.transform.localScale = new Vector3(1, lineRenderer.transform.localScale.y / 2, 1);
                float tmp = (lineRenderer.transform.localPosition.y + 0.5f) / -0.5f / 2;
                for (int i = 0; i < tmp; i++)
                {
                    MoveDown();
                }
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
                int samples = (int)(song.frequency * song.length) * 2;
                float[] data = new float[samples];
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
            if (lineRenderer.transform.localPosition.y > -0.5f - 0.5f * lineRenderer.transform.localScale.y / startingWaveVerticalScale * 2 + 0.5f)
            {
                lineRenderer.transform.localPosition = new Vector3(lineRenderer.transform.localPosition.x, lineRenderer.transform.localPosition.y - 1 / 2f, lineRenderer.transform.localPosition.z);
            }
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

        public void ShowSelection(float selectionStartTime, float selectionEndTime, float songLength)
        {
            if (selection != null)
            {
                if (selectionEndTime > selectionStartTime)
                {
                    selection.gameObject.SetActive(true);
                    float timePercentage = selectionStartTime / songLength;
                    float positionOnTimeline = timePercentage * 10;
                    selection.localPosition = new Vector3(positionOnTimeline, 0, 0);
                    float scale = (selectionEndTime - selectionStartTime) / songLength * 1000;
                    selection.localScale = new Vector3(scale, 1, 1);
                }
                else
                {
                    selection.gameObject.SetActive(false);
                }
            }
            else
            {
                Logger.NotAssigned(typeof(Transform), GetType(), name);
            }
        }
    }
}