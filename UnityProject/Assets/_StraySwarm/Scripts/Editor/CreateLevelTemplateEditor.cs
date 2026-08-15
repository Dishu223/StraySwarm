#if UNITY_EDITOR
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEditor.SceneManagement;
using StraySwarm.Data;
using StraySwarm.Core;

namespace StraySwarm.Editor
{
    /// <summary>
    /// 1-Click Level Creator Window for Stray Swarm.
    /// Automates the creation of LevelData ScriptableObjects, Handcrafted Map Prefabs, and playlist registration.
    /// Menu: Stray Swarm -> 🗺️ Create New Handcrafted Level
    /// </summary>
    public class CreateLevelTemplateEditor : EditorWindow
    {
        private int _levelID = 2;
        private string _levelName = "Level 2";
        private WorldTheme _worldTheme = WorldTheme.Desert;
        private int _totalAnimals = 15;
        private int _maxConcurrent = 5;
        private float _timeLimit = 60f;
        private int _coinReward = 50;

        private bool _allowPuppy = true;
        private bool _allowKitten = true;
        private bool _allowFrog = false;
        private bool _allowMouse = false;
        private bool _allowPigeon = false;
        private bool _allowBunny = false;

        [MenuItem("Stray Swarm/🗺️ Create New Handcrafted Level", false, 10)]
        public static void ShowWindow()
        {
            var win = GetWindow<CreateLevelTemplateEditor>("Create New Level");
            win.minSize = new Vector2(380, 520);
            win.maxSize = new Vector2(400, 550);
            win.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(10);
            GUILayout.Label("🗺️ Stray Swarm: Handcrafted Level Creator", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("This tool automatically generates a LevelData asset, a Handcrafted Map Prefab, and links them into the Level playlist with 1 click!", MessageType.Info);
            EditorGUILayout.Space(10);

            _levelID = EditorGUILayout.IntField("Level ID", _levelID);
            _levelName = EditorGUILayout.TextField("Level Name", _levelName);
            _worldTheme = (WorldTheme)EditorGUILayout.EnumPopup("World Theme", _worldTheme);

            EditorGUILayout.Space(10);
            GUILayout.Label("⚙️ Objectives & Timing", EditorStyles.boldLabel);
            _totalAnimals = EditorGUILayout.IntSlider("Total Animals Quota", _totalAnimals, 3, 50);
            _maxConcurrent = EditorGUILayout.IntSlider("Max Concurrent On Map", _maxConcurrent, 2, 10);
            _timeLimit = EditorGUILayout.Slider("Time Limit (Seconds)", _timeLimit, 20f, 180f);
            _coinReward = EditorGUILayout.IntField("Coin Reward", _coinReward);

            EditorGUILayout.Space(10);
            GUILayout.Label("🐾 Allowed Animal Species", EditorStyles.boldLabel);
            _allowPuppy = EditorGUILayout.Toggle("🐶 Puppy", _allowPuppy);
            _allowKitten = EditorGUILayout.Toggle("🐱 Kitten", _allowKitten);
            _allowFrog = EditorGUILayout.Toggle("🐸 Frog", _allowFrog);
            _allowMouse = EditorGUILayout.Toggle("🐭 Mouse", _allowMouse);
            _allowPigeon = EditorGUILayout.Toggle("🐦 Pigeon", _allowPigeon);
            _allowBunny = EditorGUILayout.Toggle("🐰 Bunny", _allowBunny);

            EditorGUILayout.Space(20);
            GUI.backgroundColor = new Color(0.3f, 0.8f, 0.3f);
            if (GUILayout.Button($"✨ CREATE LEVEL {_levelID} NOW", GUILayout.Height(42)))
            {
                CreateLevel();
            }
            GUI.backgroundColor = Color.white;
        }

        private void CreateLevel()
        {
            string levelDataDir = "Assets/_StraySwarm/Data/Levels";
            string prefabDir = "Assets/_StraySwarm/Prefabs/Levels";

            if (!Directory.Exists(levelDataDir)) Directory.CreateDirectory(levelDataDir);
            if (!Directory.Exists(prefabDir)) Directory.CreateDirectory(prefabDir);

            string formattedID = _levelID < 10 ? $"0{_levelID}" : $"{_levelID}";
            string prefabPath = $"{prefabDir}/Level_{formattedID}_Map.prefab";
            string dataPath = $"{levelDataDir}/Level_{formattedID}.asset";

            // 1. Create Map Prefab Structure
            GameObject mapRoot = new GameObject($"Level_{formattedID}_Map");

            // Grid & Tilemap
            GameObject gridObj = new GameObject("Grid");
            gridObj.transform.SetParent(mapRoot.transform);
            Grid grid = gridObj.AddComponent<Grid>();
            grid.cellSize = new Vector3(1f, 1f, 0f);

            GameObject tilemapObj = new GameObject("FloorTilemap");
            tilemapObj.transform.SetParent(gridObj.transform);
            Tilemap tilemap = tilemapObj.AddComponent<Tilemap>();
            tilemap.tileAnchor = new Vector3(0.5f, 0.5f, 0f); // 0.5, 0.5 anchor aligns sprites with grid cells
            TilemapRenderer tmr = tilemapObj.AddComponent<TilemapRenderer>();
            tmr.sortingOrder = 0;

            // Spawn Points container
            GameObject spawnPointsContainer = new GameObject("AnimalSpawnPoints");
            spawnPointsContainer.transform.SetParent(mapRoot.transform);

            // Stations container
            GameObject stationsContainer = new GameObject("Stations");
            stationsContainer.transform.SetParent(mapRoot.transform);

            // Player Spawn Point
            GameObject psp = new GameObject("PlayerSpawnPoint");
            psp.transform.SetParent(mapRoot.transform, false);
            psp.transform.localPosition = new Vector3(0.5f, 0.5f, 0f);
            psp.AddComponent<Gameplay.PlayerSpawnPoint>();

            // Obstacles container
            GameObject obstaclesContainer = new GameObject("Obstacles");
            obstaclesContainer.transform.SetParent(mapRoot.transform);

            // Save Prefab
            GameObject mapPrefab = PrefabUtility.SaveAsPrefabAsset(mapRoot, prefabPath);
            DestroyImmediate(mapRoot);

            // 2. Create LevelData ScriptableObject
            LevelData data = CreateInstance<LevelData>();
            data.LevelID = _levelID;
            data.LevelName = _levelName;
            data.World = _worldTheme;
            data.TotalAnimalsToRescue = _totalAnimals;
            data.MaxConcurrentOnMap = _maxConcurrent;
            data.TimeLimit = _timeLimit;
            data.CoinReward = _coinReward;
            data.MapPrefab = mapPrefab;

            List<AnimalType> allowed = new List<AnimalType>();
            if (_allowPuppy) allowed.Add(AnimalType.Puppy);
            if (_allowKitten) allowed.Add(AnimalType.Kitten);
            if (_allowFrog) allowed.Add(AnimalType.Frog);
            if (_allowMouse) allowed.Add(AnimalType.Mouse);
            if (_allowPigeon) allowed.Add(AnimalType.Pigeon);
            if (_allowBunny) allowed.Add(AnimalType.Bunny);

            if (allowed.Count == 0) allowed.Add(AnimalType.Puppy);
            data.AllowedAnimalTypes = allowed;

            AssetDatabase.CreateAsset(data, dataPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"🎉 [CreateLevelTemplate] Created Level {_levelID}: {dataPath} & {prefabPath}!");

            // Open the new Map Prefab in Prefab Mode for immediate editing!
            AssetDatabase.OpenAsset(mapPrefab);

            EditorUtility.DisplayDialog("Level Created Successfully!", 
                $"Level {_levelID} ({_levelName}) has been created and opened in Prefab Mode!\n\n" +
                $"What to do next:\n" +
                $"1. Select 'FloorTilemap' and paint your maze with the Tile Palette.\n" +
                $"2. Drag 'AnimalSpawnPoint' prefabs onto your path.\n" +
                $"3. Drag a 'RescueStation' prefab onto your drop-off spot.\n" +
                $"4. Click the '<' arrow at top left of Scene view to save and return!", "Let's Go!");
        }
    }
}
#endif
