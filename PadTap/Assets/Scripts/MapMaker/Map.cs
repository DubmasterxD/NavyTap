using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    List<Point> points = new List<Point>();

    [System.Serializable]
    public class Point
    {
        public float time = 0f;
        public int tileIndex = 0;
    }

    public void AddPoint(int tileIndex)
    {
        Point newPoint = new Point();
        newPoint.tileIndex = tileIndex;
        newPoint.time = FindObjectOfType<Audio>().GetTime();
        points.Add(newPoint);
    }
}
