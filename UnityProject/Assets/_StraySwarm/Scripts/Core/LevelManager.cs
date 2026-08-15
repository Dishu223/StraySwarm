using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using StraySwarm.Data;

namespace StraySwarm.Core
{
    /// <summary>
    /// Manages the full world playlists, level navigation, and seamless scene transitions.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [Header("Worlds Configuration")]
        [SerializeField] private List<WorldData> _worlds = new List<WorldData>();

        [Header("Flat Playlist (Fallback / Auto-Populated)")]
        [SerializeField] private List<LevelData> _levelPlaylist = new List<LevelData>();

        [Header("Player Configuration")]
        [SerializeField] private GameObject _playerPrefab;

        public int CurrentWorldIndex { get; private set; } = 0;
        public int CurrentLevelIndex { get; private set; } = 0;
        public List<WorldData> Worlds => _worlds;

        private GameObject _spawnedMapInstance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                RebuildFlatPlaylist();
                SpawnActiveLevelMap();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _spawnedMapInstance = null;
            RebuildFlatPlaylist();
            SpawnActiveLevelMap();
        }

        public void SpawnActiveLevelMap()
        {
            LevelData data = GetCurrentLevelData();
            if (data == null || data.MapPrefab == null) return;

            // 1. Clean up old spawned map or loose prototype scene grid
            if (_spawnedMapInstance != null)
            {
                DestroyImmediate(_spawnedMapInstance);
            }

            // Remove any prototype root objects if present in scene
            var oldRoots = new string[] { "Grid", "AnimalSpawnPoints", "Stations", "Obstacles", "RescueStation", "NumberedWall_3", "OneWayArrow_Down", "Level_01_Map", "Level_02_Map" };
            foreach (var rName in oldRoots)
            {
                GameObject obj = GameObject.Find(rName);
                if (obj != null && obj.transform.parent == null)
                {
                    DestroyImmediate(obj);
                }
            }

            // 2. Instantiate new level map prefab
            _spawnedMapInstance = Instantiate(data.MapPrefab);
            _spawnedMapInstance.name = data.MapPrefab.name;

            // 3. Rebuild Grid Graph from newly spawned map
            if (GridManager.Instance != null)
            {
                GridManager.Instance.RebuildGrid();
            }

            // 4. Position or Spawn Player at PlayerSpawnPoint
            Gameplay.PlayerSpawnPoint psp = _spawnedMapInstance.GetComponentInChildren<Gameplay.PlayerSpawnPoint>();
            Vector3 startPos = psp != null ? psp.transform.position : new Vector3(0.5f, 0.5f, 0f);

            PlayerController player = FindAnyObjectByType<PlayerController>();
            if (player == null)
            {
                if (_playerPrefab == null)
                {
#if UNITY_EDITOR
                    _playerPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_StraySwarm/Prefabs/Characters/PlayerCat.prefab");
#endif
                }

                if (_playerPrefab != null)
                {
                    GameObject playerObj = Instantiate(_playerPrefab, startPos, Quaternion.identity);
                    playerObj.name = "Player";
                    player = playerObj.GetComponent<PlayerController>();
                }
            }
            else
            {
                player.transform.position = startPos;
                var sr = player.GetComponent<SpriteRenderer>();
                if (sr != null) sr.sortingOrder = 10;
            }

            if (player != null)
            {
                player.SnapToClosestNode();
            }

            // 5. Initialize Wave Spawner
            if (Gameplay.WaveSpawner.Instance != null)
            {
                Gameplay.WaveSpawner.Instance.InitializeWaveSystem();
            }
        }

        public void RebuildFlatPlaylist()
        {
            _levelPlaylist.Clear();
            if (_worlds != null && _worlds.Count > 0)
            {
                foreach (var world in _worlds)
                {
                    if (world != null && world.Levels != null)
                    {
                        _levelPlaylist.AddRange(world.Levels);
                    }
                }
            }

#if UNITY_EDITOR
            if (_levelPlaylist.Count == 0)
            {
                string[] guids = UnityEditor.AssetDatabase.FindAssets("t:LevelData", new[] { "Assets/_StraySwarm/Data/Levels" });
                List<LevelData> found = new List<LevelData>();
                foreach (string guid in guids)
                {
                    string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                    LevelData ld = UnityEditor.AssetDatabase.LoadAssetAtPath<LevelData>(path);
                    if (ld != null && !found.Contains(ld)) found.Add(ld);
                }
                found.Sort((a, b) => a.LevelID.CompareTo(b.LevelID));
                _levelPlaylist.AddRange(found);
            }
#endif
        }

        public LevelData GetCurrentLevelData()
        {
            if (_levelPlaylist.Count == 0) RebuildFlatPlaylist();
            if (_levelPlaylist.Count == 0) return null;
            return _levelPlaylist[Mathf.Clamp(CurrentLevelIndex, 0, _levelPlaylist.Count - 1)];
        }

        public void SelectLevel(int flatIndex)
        {
            CurrentLevelIndex = Mathf.Clamp(flatIndex, 0, Mathf.Max(0, _levelPlaylist.Count - 1));
        }

        public void SelectWorldAndLevel(int worldIndex, int levelIndexInWorld)
        {
            CurrentWorldIndex = worldIndex;
            int flatIndex = 0;

            for (int w = 0; w < worldIndex && w < _worlds.Count; w++)
            {
                if (_worlds[w] != null) flatIndex += _worlds[w].Levels.Count;
            }
            flatIndex += levelIndexInWorld;
            SelectLevel(flatIndex);
        }

        public void LoadNextLevel()
        {
            CurrentLevelIndex++;
            if (CurrentLevelIndex >= _levelPlaylist.Count)
            {
                CurrentLevelIndex = 0; // Or return to Main Menu / Victory
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void RestartCurrentLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

#if UNITY_EDITOR
        [ContextMenu("🕹️ Play Level 1")]
        public void EditorSelectLevel1()
        {
            SelectLevel(0);
            Debug.Log($"🎮 [LevelManager] Selected Level 1 ({GetCurrentLevelData()?.LevelName})");
        }

        [ContextMenu("🕹️ Play Level 2")]
        public void EditorSelectLevel2()
        {
            SelectLevel(1);
            Debug.Log($"🎮 [LevelManager] Selected Level 2 ({GetCurrentLevelData()?.LevelName})");
        }
#endif
    }
}
