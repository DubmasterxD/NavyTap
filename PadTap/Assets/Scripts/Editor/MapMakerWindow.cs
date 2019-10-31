using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MapMakerWindow : EditorWindow
{
    AudioClip song;
    float currentTime = 0;
    AudioSource audioSource;
    string songName = "";
    PadTap.Map map;
    int tilesRows = 4;
    int tilesColumns = 4;
    [Range(0, 1)] float threshold = .8f;
    [Range(0, 5)] float indicatorLifespan = 2;
    List<Point> points = new List<Point>();

    [System.Serializable]
    public class Point
    {
        public float time = 0f;
        public int tileIndex = 0;

        public Point(float time, int tileIndex)
        {
            this.time = time;
            this.tileIndex = tileIndex;
        }
    }

    [MenuItem("Window/Map Maker")]
    static void OpenWindow()
    {
        MapMakerWindow window = GetWindow<MapMakerWindow>("Map Maker");
        //window.minSize = new Vector2(500, 500);
        window.Show();
    }

    private void Update()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            Repaint();
        }
    }

    private void OnGUI()
    {
        if (audioSource == null)
        {
            audioSource = FindObjectOfType<AudioSource>();
        }
        else
        {
            if (audioSource.isPlaying)
            {
                currentTime = audioSource.time;
            }
            songName = EditorGUILayout.TextField("Song Name", songName);
            song = (AudioClip)EditorGUILayout.ObjectField("Song", song, typeof(AudioClip), false);
            if (song != null)
            {
                currentTime = Mathf.Round(currentTime * 100) / 100;
                currentTime = EditorGUILayout.Slider(currentTime, 0, song.length);
                if(GUILayout.Button("Begin Creating"))
                {
                    points = new List<Point>();
                }
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Play"))
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.clip = song;
                        audioSource.time = currentTime;
                        audioSource.Play();
                    }
                }
                if (GUILayout.Button("Stop"))
                {
                    currentTime = audioSource.time;
                    audioSource.Stop();
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("0"))
                {
                    AddPoint(0);
                }
                if (GUILayout.Button("1"))
                {
                    AddPoint(1);
                }
                if (GUILayout.Button("2"))
                {
                    AddPoint(2);
                }
                if (GUILayout.Button("3"))
                {
                    AddPoint(3);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("4"))
                {
                    AddPoint(4);
                }
                if (GUILayout.Button("5"))
                {
                    AddPoint(5);
                }
                if (GUILayout.Button("6"))
                {
                    AddPoint(4);
                }
                if (GUILayout.Button("7"))
                {
                    AddPoint(5);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("8"))
                {
                    AddPoint(4);
                }
                if (GUILayout.Button("9"))
                {
                    AddPoint(5);
                }
                if (GUILayout.Button("10"))
                {
                    AddPoint(4);
                }
                if (GUILayout.Button("11"))
                {
                    AddPoint(5);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("12"))
                {
                    AddPoint(4);
                }
                if (GUILayout.Button("13"))
                {
                    AddPoint(5);
                }
                if (GUILayout.Button("14"))
                {
                    AddPoint(4);
                }
                if (GUILayout.Button("15"))
                {
                    AddPoint(5);
                }
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button("Save"))
                {
                    SaveMap();
                }
            }
        }
    }

    void AddPoint(int tileIndex)
    {
        Point newPoint = new Point(currentTime, tileIndex);
        points.Add(newPoint);
    }

    void SaveMap()
    {
        map = CreateInstance<PadTap.Map>();
        map.points = new List<PadTap.Map.Point>();
        foreach(Point point in points)
        {
            map.points.Add(new PadTap.Map.Point(point.time, point.tileIndex));
        }
        string path = "Assets/Maps/" + songName + ".asset";
        AssetDatabase.CreateAsset(map, path);
    }
}
