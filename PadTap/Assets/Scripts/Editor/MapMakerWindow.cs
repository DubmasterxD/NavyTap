using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MapMakerWindow : EditorWindow
{
    PadTap.Map map = null;
    PadTap.Map.Point currentPoint = null;
    AudioSource audioSource = null;
    float deltaTime = 1;
    bool canCreate = false;
    static Vector2 windowSize = new Vector2(400, 600);

    GUISkin skin = null;
    string skinsPath = "";
    string skinName = "Test";

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
        if (skin == null)
        {
            skin = Resources.Load<GUISkin>(skinsPath + skinName);
            if (skin == null)
            {
                Debug.LogError("Invalid skin path or name!");
                skin = CreateInstance<GUISkin>();
            }
        }
        if (map == null)
        {
            CreateNewMap();
        }
        if (audioSource == null)
        {
            audioSource = FindObjectOfType<AudioSource>();
        }
    }

    private void OnGUI()
    {
        if (audioSource != null)
        {
            AudioClip previousSong = map.song;
            map.song = (AudioClip)EditorGUILayout.ObjectField("Song", map.song, typeof(AudioClip), false);
            if (previousSong != map.song)
            {
                canCreate = false;
                map.copyright = "";
                map.tilesRows = 4;
                map.tilesColumns = 4;
                audioSource.time = 0;
                map.songName = "";
                deltaTime = 1;
                map.threshold = .8f;
                map.indicatorLifespan = 2;
                map.points = new List<PadTap.Map.Point>();
            }
            if (map.song != null)
            {
                if (!canCreate)
                {
                    EditorGUILayout.LabelField("Copyright:");
                    map.copyright = EditorGUILayout.TextArea(map.copyright);
                    if (map.copyright == "")
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
                    map.songName = EditorGUILayout.TextField("Song Name", map.songName);
                    map.threshold = EditorGUILayout.Slider("Threshold", map.threshold, 0, 1);
                    map.indicatorLifespan = EditorGUILayout.Slider("Indicator Lifespan", map.indicatorLifespan, 0, 10);
                    map.tilesRows = EditorGUILayout.IntSlider("Rows", map.tilesRows, 1, 6);
                    map.tilesColumns = EditorGUILayout.IntSlider("Columns", map.tilesColumns, 1, 6);
                    EditorGUILayout.BeginHorizontal();
                    if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                        audioSource.Pause();
                    }
                    if (GUILayout.Button("Play"))
                    {
                        if (!audioSource.isPlaying)
                        {
                    audioSource.clip = map.song;
                            audioSource.Play();
                        }
                    }
                    if (GUILayout.Button("Stop"))
                    {
                        audioSource.Pause();
                    }
                    EditorGUILayout.EndHorizontal();
                    audioSource.time = EditorGUILayout.Slider(audioSource.time, 0, map.song.length);
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Back"))
                    {
                        audioSource.time -= deltaTime;
                        if (audioSource.time < 0)
                        {
                            audioSource.time = 0;
                        }
                    }
                    if (GUILayout.Button("Front"))
                    {
                        audioSource.time += deltaTime;
                        if (audioSource.time > map.song.length)
                        {
                            audioSource.time = map.song.length;
                        }
                    }
                    deltaTime = EditorGUILayout.FloatField("Delta Time", deltaTime);
                    deltaTime = Mathf.Round(deltaTime * 100) / 100;
                    EditorGUILayout.EndHorizontal();
                    Rect a = EditorGUILayout.BeginHorizontal();
                    if (map.points.Count > 0)
                    {
                        currentPoint = map.points[0];
                        foreach (PadTap.Map.Point point in map.points)
                        {
                            if (point.time <= audioSource.time)
                            {
                                currentPoint = point;
                            }
                        }
                    }
                    string chosen = "-";
                    if (map.points.Count != 0)
                    {
                        chosen = currentPoint.time.ToString();
                    }
                    if (EditorGUILayout.DropdownButton(new GUIContent(chosen), FocusType.Keyboard))
                    {
                        PointChoicePopup menu = new PointChoicePopup();
                        menu.Initialization(this, map.points, currentPoint);
                        PopupWindow.Show(a, menu);
                    }
                    if (GUILayout.Button("Previous Point"))
                    {
                        if (audioSource.time > currentPoint.time)
                        {
                            ChangeCurrentPoint(map.points.IndexOf(currentPoint));
                        }
                        else
                        {
                            ChangeCurrentPoint(map.points.IndexOf(currentPoint) - 1);
                        }
                    }
                    if (GUILayout.Button("Next Point"))
                    {
                        ChangeCurrentPoint(map.points.IndexOf(currentPoint) + 1);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Delete Point"))
                    {
                        int previousIndex = map.points.IndexOf(currentPoint) - 1;
                        if (previousIndex < 0)
                        {
                            previousIndex = 0;
                        }
                        map.points.Remove(currentPoint);
                        ChangeCurrentPoint(previousIndex);
                    }
                    if (GUILayout.Button("Clear Points"))
                    {
                        map.points = new List<PadTap.Map.Point>();
                        ChangeCurrentPoint(0);
                    }
                    EditorGUILayout.EndHorizontal();
                    for (int i = 0; i < map.tilesRows; i++)
                    {
                        GUIStyle center = skin.GetStyle("Center");
                        GUIStyle tileButton = skin.button;
                        center.padding.left = (int)((windowSize.x - map.tilesColumns * (tileButton.fixedWidth + tileButton.margin.left)) / 2);
                        EditorGUILayout.BeginHorizontal(center);
                        for (int j = 0; j < map.tilesColumns; j++)
                        {
                            if (GUILayout.Button((i * map.tilesColumns + j).ToString(), skin.button))
                            {
                                AddPoint(i * map.tilesColumns + j);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.BeginHorizontal();
                    if (map.songName == "")
                    {
                        EditorGUILayout.HelpBox("Song Name required!", MessageType.Warning);
                    }
                    else if (GUILayout.Button("Save"))
                    {
                        SaveMap();
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            if (GUILayout.Button("Load Selected Map"))
            {
                LoadMap();
            }
        }
    }

    void CreateNewMap()
    {
        map = CreateInstance<PadTap.Map>();
    }

    void AddPoint(int tileIndex)
    {
        if (map.points == null)
        {
            map.points = new List<PadTap.Map.Point>();
        }
        foreach (PadTap.Map.Point point in map.points)
        {
            if (point.time == audioSource.time)
            {
                return;
            }
        }
        PadTap.Map.Point newPoint = new PadTap.Map.Point(audioSource.time, tileIndex);
        map.points.Add(newPoint);
        map.points.Sort();
    }

    void ChangeCurrentPoint(int indexInList)
    {
        if (map.points.Count != 0)
        {
            indexInList = Mathf.Clamp(indexInList, 0, map.points.Count - 1);
            currentPoint = map.points[indexInList];
            ChangeCurrentTime(currentPoint.time);
        }
    }

    void ChangeCurrentTime(float time)
    {
        time = Mathf.Clamp(time, 0, map.song.length);
        audioSource.time = time;
    }

    void SaveMap()
    {
        if (!string.IsNullOrEmpty(map.songName))
        {
            string path = "Assets/Maps/" + map.songName + ".asset";
            AssetDatabase.CreateAsset(map, path);
            CreateMapCopy();
        }
    }

    void CreateMapCopy()
    {
        if (map != null)
        {
            PadTap.Map newInstance = CreateInstance<PadTap.Map>();
            newInstance.copyright = map.copyright;
            newInstance.indicatorLifespan = map.indicatorLifespan;
            newInstance.song = map.song;
            newInstance.songName = map.songName;
            newInstance.threshold = map.threshold;
            newInstance.tilesColumns = map.tilesColumns;
            newInstance.tilesRows = map.tilesRows;
            newInstance.points = new List<PadTap.Map.Point>();
            foreach (PadTap.Map.Point point in map.points)
            {
                newInstance.points.Add(new PadTap.Map.Point(point.time, point.tileIndex));
            }
            map = newInstance;
        }
        else
        {
            CreateNewMap();
        }
    }

    void LoadMap()
    {
        if (Selection.assetGUIDs.Length == 1)
        {
            string mapGUID = Selection.assetGUIDs[0];
            map = AssetDatabase.LoadAssetAtPath<PadTap.Map>(AssetDatabase.GUIDToAssetPath(mapGUID));
            if (map == null)
            {
                CreateNewMap();
            }
            else
            {
                canCreate = true;
                CreateMapCopy();
            }
        }
    }

    public class PointChoicePopup : PopupWindowContent
    {
        MapMakerWindow parent;

        List<PadTap.Map.Point> points;
        PadTap.Map.Point currentPoint;

        Vector2 scrollPosition = Vector2.zero;

        public override void OnGUI(Rect rect)
        {
            editorWindow.minSize = new Vector2(150, 0);
            editorWindow.maxSize = new Vector2(150, 300);
            DrawDropdown();
        }

        private void DrawDropdown()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(150));
            for (int i = 0; i < points.Count; i++)
            {
                if (EditorGUILayout.ToggleLeft(points[i].time.ToString(), points[i].time == currentPoint.time, GUILayout.Width(100)))
                {
                    SelectPoint(i);
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private void SelectPoint(int index)
        {
            if (points[index].time != currentPoint.time)
            {
                parent.ChangeCurrentPoint(index);
                editorWindow.Close();
            }
        }

        public void Initialization(MapMakerWindow newParent, List<PadTap.Map.Point> newPoints, PadTap.Map.Point newCurrentPoint)
        {
            parent = newParent;
            points = newPoints;
            currentPoint = newCurrentPoint;
        }
    }
}