using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using PadTap.Core;

public class MapMakerWindow : EditorWindow
{
    private static Vector2 windowSize = new Vector2(400, 600);

    [MenuItem("Window/Map Maker")]
    private static void OpenWindow()
    {
        MapMakerWindow window = GetWindow<MapMakerWindow>("Map Maker");
        window.minSize = windowSize;
        window.maxSize = windowSize;
        window.Show();
    }

    private MapMakerManager manager = null;
    private Map map = null;
    private Map.Point currentPoint = null;
    private AudioSource audioSource = null;
    private float deltaTime = 1;
    private bool givenCopyright = false;
    private int minRows = 1;
    private int minColumns = 1;

    private GUISkin skin = null;
    private string skinsPath = "";
    private string skinName = "Test";

    private void Update()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            Repaint();
        }
        manager.SetVisibleTiles(map.tilesRows, map.tilesColumns);
        manager.Animate(Time.deltaTime);
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
    }

    private void OnGUI()
    {
        CheckForManager();
        CheckForAudioSource();
        if (audioSource != null && manager!=null)
        {
            DrawSongSelection();
            if (map.song != null)
            {
                if (!givenCopyright)
                {
                    DrawCopyright();
                }
                else
                {
                    DrawMapSettings();
                    DrawMusicPlayer();
                    DrawPointsManager();
                    DrawTiles();
                    DrawMapSave();
                }
            }
            DrawMapLoad();
        }
    }

    private void CheckForManager()
    {
        if (manager == null)
        {
            manager = FindObjectOfType<MapMakerManager>();
            if (manager == null)
            {
                Debug.LogError("No MapMakerManager found!");
            }
        }
    }

    private void CheckForAudioSource()
    {
        if (audioSource == null)
        {
            audioSource = FindObjectOfType<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("No AudioSource found!");
            }
        }
    }

    private void DrawSongSelection()
    {
        AudioClip previousSong = map.song;
        map.song = (AudioClip)EditorGUILayout.ObjectField("Song", map.song, typeof(AudioClip), false);
        if (previousSong != map.song)
        {
            ResetMap();
        }
    }

    private void DrawCopyright()
    {
        EditorGUILayout.LabelField("Copyright:");
        map.copyright = EditorGUILayout.TextArea(map.copyright);
        if (map.copyright == "")
        {
            EditorGUILayout.HelpBox("Valid copyright informations are necessary!", MessageType.Warning);
        }
        else if (GUILayout.Button("Begin Creating"))
        {
            givenCopyright = true;
        }
    }

    private void DrawMapSettings()
    {
        map.mapName = EditorGUILayout.TextField("Song Name", map.mapName);
        map.threshold = EditorGUILayout.Slider("Threshold", map.threshold, 0, 1);
        manager.ChageThreshold(map.threshold);
        map.indicatorLifespan = EditorGUILayout.Slider("Indicator Lifespan", map.indicatorLifespan, 0.1f, 10);
        manager.ChangeSpeedFromFilespan(map.indicatorLifespan);
        map.tilesRows = EditorGUILayout.IntSlider("Rows", map.tilesRows, minRows, 6);
        ChangeMinRows(map.tilesRows);
        int previousColumns = map.tilesColumns;
        map.tilesColumns = EditorGUILayout.IntSlider("Columns", map.tilesColumns, minColumns, 6);
        ChangeMinColumns(map.tilesColumns);
        if (previousColumns != map.tilesColumns)
        {
            ChangeIndexes(previousColumns);
        }
    }

    private void ChangeMinRows(int rows)
    {
        int highestIndex = 0;
        foreach(Map.Point point in map.points)
        {
            if (point.tileIndex > highestIndex)
            {
                highestIndex = point.tileIndex;
            }
        }
        minRows = Mathf.FloorToInt(highestIndex / map.tilesColumns) + 1;
    }

    private void ChangeMinColumns(int columns)
    {
        int highestColumnUsed = 0;
        foreach(Map.Point point in map.points)
        {
            if (point.tileIndex % map.tilesColumns > highestColumnUsed)
            {
                highestColumnUsed = point.tileIndex % map.tilesColumns;
            }
        }
        minColumns = highestColumnUsed + 1;
    }

    private void ChangeIndexes(int previousColumns)
    {
        int deltaColumn = map.tilesColumns - previousColumns;
        foreach(Map.Point point in map.points)
        {
            point.tileIndex += Mathf.FloorToInt(point.tileIndex / previousColumns) * deltaColumn;
        }
    }

    private void DrawMusicPlayer()
    {
        EditorGUILayout.BeginHorizontal();
        MakeSureAudioSourceDoesntReset();
        if (GUILayout.Button("Play"))
        {
            Play();
        }
        if (GUILayout.Button("Pause"))
        {
            Pause();
        }
        EditorGUILayout.EndHorizontal();
        audioSource.time = EditorGUILayout.Slider(audioSource.time, 0, map.song.length);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Back"))
        {
            MoveTime(-deltaTime);
        }
        if (GUILayout.Button("Front"))
        {
            MoveTime(deltaTime);
        }
        deltaTime = EditorGUILayout.FloatField("Delta Time", deltaTime);
        EditorGUILayout.EndHorizontal();
    }

    private void DrawPointsManager()
    {
        Rect pointsManager = EditorGUILayout.BeginHorizontal();
        MakePointsDropdown(pointsManager);
        if (GUILayout.Button("Previous Point"))
        {
            PreviousPoint();
        }
        if (GUILayout.Button("Next Point"))
        {
            NextPoint();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Delete Point"))
        {
            DeletePoint(currentPoint);
        }
        if (GUILayout.Button("Clear Points"))
        {
            ClearPoints();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawTiles()
    {
        for (int i = 0; i < map.tilesRows; i++)
        {
            EditorGUILayout.BeginHorizontal(TilesHorizontalCenter());
            for (int j = 0; j < map.tilesColumns; j++)
            {
                if (GUILayout.Button(GetTileIndex(i, j).ToString(), skin.button))
                {
                    AddPoint(GetTileIndex(i, j));
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    private void DrawMapSave()
    {
        EditorGUILayout.BeginHorizontal();
        if (map.mapName == "")
        {
            EditorGUILayout.HelpBox("Song Name required!", MessageType.Warning);
        }
        else if (GUILayout.Button("Save"))
        {
            SaveMap();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawMapLoad()
    {
        if (GUILayout.Button("Load Selected Map"))
        {
            LoadMap();
        }
    }

    private void CreateNewMap()
    {
        map = CreateInstance<Map>();
    }

    private void ResetMap()
    {
        givenCopyright = false;
        map.copyright = "";
        map.tilesRows = 4;
        map.tilesColumns = 4;
        audioSource.time = 0;
        map.mapName = "";
        deltaTime = 1;
        map.threshold = .8f;
        map.indicatorLifespan = 2;
        map.points = new List<Map.Point>();
    }

    private void MakeSureAudioSourceDoesntReset()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            audioSource.Pause();
        }
    }

    private void Play()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = map.song;
            audioSource.Play();
        }
    }

    private void Pause()
    {
        audioSource.Pause();
    }

    private void MoveTime(float deltaTime)
    {
        audioSource.time = Mathf.Clamp(audioSource.time + deltaTime, 0, map.song.length - deltaTime);
    }

    private void MakePointsDropdown(Rect pointsManager)
    {
        SetCurrentPointFromCurrentTime();
        string chosen = GetCurrentPointsTime();
        if (EditorGUILayout.DropdownButton(new GUIContent(chosen), FocusType.Keyboard))
        {
            ShowDropdown(pointsManager, map.points, currentPoint);
        }
    }

    private void SetCurrentPointFromCurrentTime()
    {
        if (map.points.Count > 0)
        {
            currentPoint = map.points[0];
            foreach (Map.Point point in map.points)
            {
                if (point.time <= audioSource.time)
                {
                    currentPoint = point;
                }
            }
        }
    }

    private string GetCurrentPointsTime()
    {
        string chosen = "-";
        if (map.points.Count != 0)
        {
            chosen = currentPoint.time.ToString();
        }
        return chosen;
    }

    private void ShowDropdown(Rect pointsManager, List<Map.Point> points, Map.Point point)
    {
        PointChoicePopup menu = new PointChoicePopup();
        menu.Initialization(this, points, point);
        PopupWindow.Show(pointsManager, menu);
    }

    private void PreviousPoint()
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

    private void NextPoint()
    {
        ChangeCurrentPoint(map.points.IndexOf(currentPoint) + 1);
    }

    private void DeletePoint(Map.Point point)
    {
        int previousIndex = map.points.IndexOf(point) - 1;
        if (previousIndex < 0)
        {
            previousIndex = 0;
        }
        map.points.Remove(point);
        ChangeCurrentPoint(previousIndex);
    }

    private void ClearPoints()
    {
        map.points = new List<Map.Point>();
        ChangeCurrentPoint(0);
    }

    private void ChangeCurrentPoint(int indexInList)
    {
        if (map.points.Count != 0)
        {
            indexInList = Mathf.Clamp(indexInList, 0, map.points.Count - 1);
            currentPoint = map.points[indexInList];
            ChangeCurrentTime(currentPoint.time);
        }
    }

    private void ChangeCurrentTime(float time)
    {
        time = Mathf.Clamp(time, 0, map.song.length);
        audioSource.time = time;
    }
    
    private GUIStyle TilesHorizontalCenter()
    {
        GUIStyle center = skin.GetStyle("Center");
        GUIStyle tileButton = skin.button;
        center.padding.left = (int)((windowSize.x - map.tilesColumns * (tileButton.fixedWidth + tileButton.margin.left)) / 2);
        return center;
    }

    private int GetTileIndex(int i, int j)
    {
        return i * map.tilesColumns + j;
    }

    private void AddPoint(int tileIndex)
    {
        if (map.points == null)
        {
            map.points = new List<Map.Point>();
        }
        foreach (Map.Point point in map.points)
        {
            if (point.time == audioSource.time)
            {
                return;
            }
        }
        Map.Point newPoint = new Map.Point(audioSource.time, tileIndex);
        map.points.Add(newPoint);
        map.points.Sort();
    }

    private void SaveMap()
    {
        if (!string.IsNullOrEmpty(map.mapName))
        {
            string path = "Assets/Maps/" + map.mapName + ".asset";
            AssetDatabase.CreateAsset(map, path);
            CreateMapCopy();
        }
    }

    private void CreateMapCopy()
    {
        if (map != null)
        {
            Map newInstance = CreateInstance<Map>();
            newInstance.copyright = map.copyright;
            newInstance.indicatorLifespan = map.indicatorLifespan;
            newInstance.song = map.song;
            newInstance.mapName = map.mapName;
            newInstance.threshold = map.threshold;
            newInstance.tilesColumns = map.tilesColumns;
            newInstance.tilesRows = map.tilesRows;
            newInstance.points = new List<Map.Point>();
            foreach (Map.Point point in map.points)
            {
                newInstance.points.Add(new Map.Point(point.time, point.tileIndex));
            }
            map = newInstance;
        }
        else
        {
            CreateNewMap();
        }
    }

    private void LoadMap()
    {
        if (Selection.assetGUIDs.Length == 1)
        {
            string mapGUID = Selection.assetGUIDs[0];
            map = AssetDatabase.LoadAssetAtPath<Map>(AssetDatabase.GUIDToAssetPath(mapGUID));
            if (map == null)
            {
                CreateNewMap();
            }
            else
            {
                givenCopyright = true;
                CreateMapCopy();
            }
        }
    }

    public class PointChoicePopup : PopupWindowContent
    {
        private MapMakerWindow parent;

        private List<Map.Point> points;
        private Map.Point currentPoint;

        private Vector2 scrollPosition = Vector2.zero;

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

        public void Initialization(MapMakerWindow newParent, List<Map.Point> newPoints, Map.Point newCurrentPoint)
        {
            parent = newParent;
            points = newPoints;
            currentPoint = newCurrentPoint;
        }
    }
}