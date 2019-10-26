using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Map", menuName ="Map", order =0)]
public class Map : ScriptableObject
{
    [SerializeField] List<Point> points = null;

    public List<Point> Points { get => points; }

    [System.Serializable]
    public class Point
    {
        public float time = 0f;
        public int tileIndex = 0;
    }
}
