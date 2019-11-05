using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MapMakerWindow : EditorWindow
{
    AudioClip song;
    float currentTime = 0;
    AudioSource audioSource;
    string songName = "";
    string copyright = "";
    PadTap.Map map;
    int tilesRows = 4;
    int tilesColumns = 4;
    [Range(0, 1)] float threshold = .8f;
    [Range(0, 5)] float indicatorLifespan = 2;
    List<Point> points = new List<Point>();
    static Vector2 windowSize = new Vector2(400, 600);
    float deltaTime = 1;
    bool canCreate = false;

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
        window.minSize = windowSize;
        window.maxSize = windowSize;
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
            AudioClip previousSong = song;
            song = (AudioClip)EditorGUILayout.ObjectField("Song", song, typeof(AudioClip), false);
            if (previousSong != song)
            {
                canCreate = false;
                copyright = "";
                tilesRows = 4;
                tilesColumns = 4;
                audioSource.time = 0;
                currentTime = 0;
                songName = "";
                deltaTime = 1;
                threshold = .8f;
                indicatorLifespan = 2;
                points = new List<Point>();
            }
            if (song != null)
            {
                if (!canCreate)
                {
                    copyright = EditorGUILayout.TextArea(copyright);
                    if (copyright=="")
                    {
                        EditorGUILayout.HelpBox("Valid copyright informations are necessary!", MessageType.Warning);
                    }
                    else if (GUILayout.Button("Begin Creating"))
                    {
                        canCreate = true;
                    }
                }
                else
                {
                    songName = EditorGUILayout.TextField("Song Name", songName);
                    threshold = Mathf.Clamp01(EditorGUILayout.FloatField("Threshold", threshold));
                    indicatorLifespan = Mathf.Clamp(EditorGUILayout.FloatField("Indicator Lifespan", indicatorLifespan), 0, 10);
                    tilesRows = Mathf.Clamp(EditorGUILayout.IntField("Rows", tilesRows), 1, 6);
                    tilesColumns = Mathf.Clamp(EditorGUILayout.IntField("Columns", tilesColumns), 1, 6);
                    if(currentTime > song.length)
                    {
                        currentTime = Mathf.Floor((currentTime-0.01f) * 100) / 100;
                    }
                    currentTime = Mathf.Round(currentTime * 100) / 100;
                    currentTime = EditorGUILayout.Slider(currentTime, 0, song.length);
                    //EditorGUI.ProgressBar(new Rect(7, 36, 432, 18), currentTime / song.length, "asa");
                    if (GUILayout.Button("Begin Creating"))
                    {
                        points = new List<Point>();
                    }
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Back"))
                    {
                        currentTime -= deltaTime;
                        if (currentTime < 0)
                        {
                            audioSource.time = 0;
                        }
                        else
                        {
                            audioSource.time = currentTime;
                        }
                    }
                    if (GUILayout.Button("Front"))
                    {
                        currentTime += deltaTime;
                        Debug.Log(currentTime);
                        if (currentTime > song.length)
                        {
                            audioSource.time = song.length - 0.001f;
                        }
                        else
                        {
                            audioSource.time = currentTime;
                        }
                    }
                    deltaTime = EditorGUILayout.FloatField("Delta Time", deltaTime);
                    deltaTime = Mathf.Round(deltaTime * 100) / 100;
                    EditorGUILayout.EndHorizontal();
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
                        GUIStyle center = skin.GetStyle("Center");
                        GUIStyle tileButton = skin.button;
                        center.padding.left = (int)((windowSize.x - tilesColumns * (tileButton.fixedWidth + tileButton.margin.left)) / 2);
                        EditorGUILayout.BeginHorizontal(center);
                        for (int j = 0; j < tilesColumns; j++)
                        {
                            if (GUILayout.Button((i * tilesColumns + j).ToString(), skin.button))
                            {
                                AddPoint(i * tilesColumns + j);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.BeginHorizontal();
                    if (songName == "")
                    {
                        EditorGUILayout.HelpBox("Song Name required!", MessageType.Warning);
                    }
                    else if (GUILayout.Button("Save"))
                    {
                        SaveMap();
                    }
                    if (GUILayout.Button("Load"))
                    {
                        LoadMap();
                    }
                    EditorGUILayout.EndHorizontal();
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
        map.threshold = threshold;
        map.song = song;
        map.tilesColumns = tilesColumns;
        map.tilesRows = tilesRows;
        map.indicatorLifespan = indicatorLifespan;
        map.copyright = copyright;
        map.songName = songName;
        foreach(Point point in points)
        {
            map.points.Add(new PadTap.Map.Point(point.time, point.tileIndex));
        }
        string path = "Assets/Maps/" + songName + ".asset";
        AssetDatabase.CreateAsset(map, path);
    }

    void LoadMap()
    {
        //TODO Load Map
    }
}
