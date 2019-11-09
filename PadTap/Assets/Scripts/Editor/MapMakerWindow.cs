using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

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

    private PadTap.Map map = null;
    private PadTap.Map.Point currentPoint = null;
    private AudioSource audioSource = null;
    private float deltaTime = 1;
    private bool givenCopyright = false;

    private GUISkin skin = null;
    private string skinsPath = "";
    private string skinName = "Test";

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
    }

    private void OnGUI()
    {
        CheckForAudioSource();
        if (audioSource != null)
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
            Reset();
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
        map.songName = EditorGUILayout.TextField("Song Name", map.songName);
        map.threshold = EditorGUILayout.Slider("Threshold", map.threshold, 0, 1);
        map.indicatorLifespan = EditorGUILayout.Slider("Indicator Lifespan", map.indicatorLifespan, 0, 10);
        map.tilesRows = EditorGUILayout.IntSlider("Rows", map.tilesRows, 1, 6);
        map.tilesColumns = EditorGUILayout.IntSlider("Columns", map.tilesColumns, 1, 6);
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

    private void DrawMapLoad()
    {
        if (GUILayout.Button("Load Selected Map"))
        {
            LoadMap();
        }
    }

    private void CreateNewMap()
    {
        map = CreateInstance<PadTap.Map>();
    }

    private void Reset()
    {
        givenCopyright = false;
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

    private void SaveMap()
    {
        if (!string.IsNullOrEmpty(map.songName))
        {
            string path = "Assets/Maps/" + map.songName + ".asset";
            AssetDatabase.CreateAsset(map, path);
            CreateMapCopy();
        }
    }

    private void CreateMapCopy()
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

    private void LoadMap()
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
                givenCopyright = true;
                CreateMapCopy();
            }
        }
    }

    public class PointChoicePopup : PopupWindowContent
    {
        private MapMakerWindow parent;

        private List<PadTap.Map.Point> points;
        private PadTap.Map.Point currentPoint;

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

        public void Initialization(MapMakerWindow newParent, List<PadTap.Map.Point> newPoints, PadTap.Map.Point newCurrentPoint)
        {
            parent = newParent;
            points = newPoints;
            currentPoint = newCurrentPoint;
        }
    }
}