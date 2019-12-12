using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using PadTap.Core;

namespace PadTap.MapMaker
{
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

        private MapMakerVisualizerManager visualizationManager = null;
        private AudioSource audioSource = null;
        private Map map = null;
        private Map.Point currentPoint = null;
        private float currentTime = 0;
        private float playbackSpeed = 1;
        private float deltaTime = 1;
        private bool givenCopyright = false;

        private int minRows = 1;
        private int maxRows = 6;
        private int minColumns = 1;
        private int maxColumns = 6;
        private float minIndicatorLifespan = 0.1f;
        private float maxIndicatorLifespan = 10;

        private GUISkin skin = null;
        private string skinsPath = "";
        private string defaultSkinName = "Test";

        private void Update()
        {
            CheckForMap();
            CheckForAudioSource();
            CheckForSkin(skinsPath + defaultSkinName);
            CheckForVisualizationManager();
            if (audioSource != null && audioSource.isPlaying)
            {
                Repaint();
            }
            if (visualizationManager != null && map != null && map.song != null)
            {
                visualizationManager.ManualUpdate(map, currentTime, Time.deltaTime);
            }
        }

        private void OnGUI()
        {
            DrawMapLoad();
            if (map != null)
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
                        DrawVisualizerOptions();
                    }
                }
            }
        }

        private void CheckForMap()
        {
            if (map == null)
            {
                CreateNewMap();
            }
        }

        private void CheckForVisualizationManager()
        {
            if (visualizationManager == null)
            {
                visualizationManager = FindObjectOfType<MapMakerVisualizerManager>();
                if (visualizationManager == null)
                {
                    Logger.NoComponentFound(typeof(MapMakerVisualizerManager));
                }
            }
        }

        private void CheckForSkin(string path)
        {
            skin = Resources.Load<GUISkin>(path);
            if (skin == null)
            {
                Debug.LogError("Invalid skin path or name!");
                skin = CreateInstance<GUISkin>();
            }
        }

        private void CheckForAudioSource()
        {
            if (audioSource == null)
            {
                audioSource = FindObjectOfType<AudioSource>();
                if (audioSource == null)
                {
                    Logger.NoComponentFound(typeof(AudioSource));
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
                visualizationManager.ChangeSong(map.song);
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
            map.indicatorLifespan = EditorGUILayout.Slider("Indicator Lifespan", map.indicatorLifespan, minIndicatorLifespan, maxIndicatorLifespan);
            map.tilesRows = EditorGUILayout.IntSlider("Rows", map.tilesRows, minRows, maxRows);
            ChangeMinRows(map.tilesRows);
            int previousColumns = map.tilesColumns;
            map.tilesColumns = EditorGUILayout.IntSlider("Columns", map.tilesColumns, minColumns, maxColumns);
            ChangeMinColumns(map.tilesColumns);
            if (previousColumns != map.tilesColumns)
            {
                ChangeIndexes(previousColumns);
            }
        }

        private void ChangeMinRows(int rows)
        {
            int highestIndex = 0;
            if (map.points != null)
            {
                foreach (Map.Point point in map.points)
                {
                    if (point.tileIndex > highestIndex)
                    {
                        highestIndex = point.tileIndex;
                    }
                }
            }
            minRows = Mathf.FloorToInt(highestIndex / map.tilesColumns) + 1;
        }

        private void ChangeMinColumns(int columns)
        {
            int highestColumnUsed = 0;
            if (map.points != null)
            {
                foreach (Map.Point point in map.points)
                {
                    if (point.tileIndex % map.tilesColumns > highestColumnUsed)
                    {
                        highestColumnUsed = point.tileIndex % map.tilesColumns;
                    }
                }
            }
            minColumns = highestColumnUsed + 1;
        }

        private void ChangeIndexes(int previousColumns)
        {
            if (map.points != null)
            {
                int deltaColumn = map.tilesColumns - previousColumns;
                foreach (Map.Point point in map.points)
                {
                    point.tileIndex += Mathf.FloorToInt(point.tileIndex / previousColumns) * deltaColumn;
                }
            }
        }

        private void DrawMusicPlayer()
        {
            EditorGUILayout.BeginHorizontal();
            MakeSureAudioSourceDoesntReset();
            if (GUILayout.Button("Play"))
            {
                PlayMusic();
            }
            if (GUILayout.Button("Pause"))
            {
                PauseMusic();
            }
            EditorGUILayout.EndHorizontal();
            UpdateTime(currentTime);
            SetPlaybackSpeed();
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
                    if (skin != null && GUILayout.Button(GetTileIndex(i, j).ToString(), skin.button))
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

        private void DrawVisualizerOptions()
        {
            EditorGUILayout.LabelField("Visualizer Options");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Zoom In"))
            {
                visualizationManager.ZoomInTimeline();
            }
            if (GUILayout.Button("Zoom Out"))
            {
                visualizationManager.ZoomOutTimeline();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Vertical Zoom In"))
            {
                visualizationManager.VerticalZoomInTimeline();
            }
            if (GUILayout.Button("Vertical Zoom Out"))
            {
                visualizationManager.VerticalZoomOutTimeline();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("U"))
            {
                visualizationManager.MoveUpTimeline();
            }
            if (GUILayout.Button("D"))
            {
                visualizationManager.MoveDownTimeline();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            if(GUILayout.Button("Reset Zoom"))
            {
                visualizationManager.ResetTimelineZoom();
            }
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
            map.ResetMap();
            visualizationManager.ResetMap();
            givenCopyright = false;
            UpdateTime(0);
            PauseMusic();
            deltaTime = 1;
        }

        private void MakeSureAudioSourceDoesntReset()
        {
            if (audioSource!=null && !audioSource.isPlaying)
            {
                audioSource.Play();
                audioSource.Pause();
            }
        }

        private void PlayMusic()
        {
            if (audioSource!=null && !audioSource.isPlaying)
            {
                audioSource.clip = map.song;
                audioSource.Play();
            }
        }

        private void PauseMusic()
        {
            if (audioSource != null)
            {
                audioSource.Pause();
            }
        }

        private void UpdateTime(float newTime)
        {
            float previousTime = currentTime;
            if (map != null && map.song != null)
            {
                currentTime = EditorGUILayout.Slider(newTime, 0, map.song.length);
            }
            if (audioSource != null)
            {
                if (previousTime != currentTime)
                {
                    audioSource.time = currentTime;
                }
                if (audioSource.isPlaying)
                {
                    currentTime = audioSource.time;
                }
            }
            currentTime = Mathf.Round(currentTime * 10000) / 10000;
        }

        private void SetPlaybackSpeed()
        {
            playbackSpeed = EditorGUILayout.Slider("Playback Speed",playbackSpeed, -1, 1);
            if (audioSource != null)
            {
                audioSource.pitch = playbackSpeed;
            }
        }

        private void MoveTime(float deltaTime)
        {
            UpdateTime(Mathf.Clamp(currentTime + deltaTime, 0, map.song.length - deltaTime));
        }

        private void MakePointsDropdown(Rect pointsManager)
        {
            SetCurrentPointFromCurrentTime();
            string chosen = GetCurrentPointsTime();
            if (EditorGUILayout.DropdownButton(new GUIContent(chosen), FocusType.Keyboard))
            {
                if (map.points != null && pointsManager != null && currentPoint != null)
                {
                    ShowDropdown(pointsManager, map.points, currentPoint);
                }
            }
        }

        private void SetCurrentPointFromCurrentTime()
        {
            if (map.points != null && map.points.Count > 0)
            {
                currentPoint = map.points[0];
                foreach (Map.Point point in map.points)
                {
                    if (point.time <= currentTime)
                    {
                        currentPoint = point;
                    }
                }
            }
            else
            {
                currentPoint = null;
            }
        }

        private string GetCurrentPointsTime()
        {
            string chosen = "-";
            if (currentPoint!=null)
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
            if (currentPoint != null && map.points!=null)
            {
                if (currentTime > currentPoint.time)
                {
                    ChangeCurrentPoint(map.points.IndexOf(currentPoint));
                }
                else
                {
                    ChangeCurrentPoint(map.points.IndexOf(currentPoint) - 1);
                }
            }
        }

        private void NextPoint()
        {
            if (currentPoint != null && map.points !=null)
            {
                ChangeCurrentPoint(map.points.IndexOf(currentPoint) + 1);
            }
        }

        private void DeletePoint(Map.Point point)
        {
            if (map.points != null)
            {
                int previousIndex = map.points.IndexOf(point) - 1;
                if (previousIndex < 0)
                {
                    previousIndex = 0;
                }
                map.points.Remove(point);
                ChangeCurrentPoint(previousIndex);
            }
        }

        private void ClearPoints()
        {
            map.points = new List<Map.Point>();
            ChangeCurrentPoint(0);
        }

        private void ChangeCurrentPoint(int indexInList)
        {
            if (map.points != null && map.points.Count != 0)
            {
                indexInList = Mathf.Clamp(indexInList, 0, map.points.Count - 1);
                currentPoint = map.points[indexInList];
            }
            if (currentPoint != null)
            {
                UpdateTime(currentPoint.time);
            }
        }

        private GUIStyle TilesHorizontalCenter()
        {
            GUIStyle center = new GUIStyle();
            if (skin != null)
            {
                center = skin.GetStyle("Center");
                if (center == null)
                {
                    center = skin.button;
                }
                GUIStyle tileButton = skin.button;
                center.padding.left = (int)((windowSize.x - map.tilesColumns * (tileButton.fixedWidth + tileButton.margin.left)) / 2);
            }
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
                if (point.time == currentTime)
                {
                    point.tileIndex = tileIndex;
                    return;
                }
            }
            Map.Point newPoint = new Map.Point(currentTime, tileIndex);
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
                if (map.points != null)
                {
                    foreach (Map.Point point in map.points)
                    {
                        newInstance.points.Add(new Map.Point(point.time, point.tileIndex));
                    }
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
                ResetMap();
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
                    visualizationManager.ChangeSong(map.song);
                }
            }
        }

        public class PointChoicePopup : PopupWindowContent
        {
            private MapMakerWindow parent;

            private List<Map.Point> points;
            private Map.Point currentPoint;

            private Vector2 scrollPosition = Vector2.zero;
            private float scrollWidth = 25;
            private Vector2 size = new Vector2(150, 300);

            public override void OnGUI(Rect rect)
            {
                editorWindow.minSize = size;
                editorWindow.maxSize = size;
                DrawDropdown();
            }

            private void DrawDropdown()
            {
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(size.x));
                for (int i = 0; i < points.Count; i++)
                {
                    if (EditorGUILayout.ToggleLeft(points[i].time.ToString(), points[i].time == currentPoint.time, GUILayout.Width(size.x - scrollWidth)))
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
}