using System;
using System.Collections.Generic;
using UnityEngine;

namespace NavyTap.Core
{
    [CreateAssetMenu(fileName = "Map", menuName = "Map", order = 0)]
    public class Map : ScriptableObject
    {
        public int tilesRows = 4;
        public int tilesColumns = 4;
        public float threshold = 0.5f;
        public float indicatorLifespan = 1;
        public List<Point> points = null;
        public AudioClip song = null;
        public string mapName = "";
        public string copyright = "";

        [Serializable]
        public class Point : IComparable
        {
            public float time = 0f;
            public int tileIndex = 0;

            public Point(float time, int tileIndex)
            {
                this.time = time;
                this.tileIndex = tileIndex;
            }

            public int CompareTo(object obj)
            {
                if (obj != null)
                {
                    Point point = (Point)obj;
                    if (point.time > time)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }

        public float GetPerfectScore()
        {
            return (1 + threshold) / 2;
        }

        public float GetPerfectScoreAcceptableDifference()
        {
            return (1 - threshold) / 6;
        }

        public void ResetMap()
        {
            tilesRows = 4;
            tilesColumns = 4;
            threshold = 0.5f;
            indicatorLifespan = 1;
            points = new List<Point>();
            mapName = "";
            copyright = "";
        }

        public int GetPointsCount()
        {
            if (points != null)
            {
                return points.Count;
            }
            else
            {
                Debug.LogError(Logger.NotAssigned(typeof(List<Point>), GetType(), name));
                return 0;
            }
        }
    }
}