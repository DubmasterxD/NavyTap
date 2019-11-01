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

    GUISkin skin;

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
        window.minSize = new Vector2(500, 500);
        window.maxSize = new Vector2(500, 500);
        window.Show();
    }

    private void Update()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            Repaint();
        }
    }

    private void OnEnable()
    {
        skin = Resources.Load<GUISkin>("Test");
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
                tilesRows = EditorGUILayout.IntField("Rows", tilesRows);
                if (tilesRows > 6)
                {
                    tilesRows = 6;
                }
                if (tilesRows < 1)
                {
                    tilesRows = 1;
                }
                tilesColumns = EditorGUILayout.IntField("Columns", tilesColumns);
                if (tilesColumns > 6)
                {
                    tilesColumns = 6;
                }
                if (tilesColumns < 1)
                {
                    tilesColumns = 1;
                }
                currentTime = Mathf.Round(currentTime * 100) / 100;
                currentTime = EditorGUILayout.Slider(currentTime, 0, song.length);
                //EditorGUI.ProgressBar(new Rect(7, 36, 432, 18), currentTime / song.length, "asa");
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
                for (int i = 0; i < tilesRows; i++)
                {
                    EditorGUILayout.BeginHorizontal(skin.GetStyle("Center"));
                    for (int j = 0; j < tilesColumns; j++)
                    {
                        if (GUILayout.Button((i * tilesColumns + j).ToString(), skin.button))
                        {
                            AddPoint(i*tilesColumns + j);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
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
