using System;
using System.Collections.Generic;
using UnityEngine;

namespace PadTap
{
    [CreateAssetMenu(fileName = "Map", menuName = "Map", order = 0)]
    public class Map : ScriptableObject
    {
        public int tilesRows = 4;
        public int tilesColumns = 4;
        [Range(0, 1)] public float threshold = .8f;
        [Range(0, 5)] public float indicatorLifespan = 2;
        public List<Point> points = null;
        public AudioClip song = null;
        public string songName = "";
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
        }
    }
}