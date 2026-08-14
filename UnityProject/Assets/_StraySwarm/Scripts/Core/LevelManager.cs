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

        public int CurrentWorldIndex { get; private set; } = 0;
        public int CurrentLevelIndex { get; private set; } = 0;
        public List<WorldData> Worlds => _worlds;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                RebuildFlatPlaylist();
            }
            else
            {
                Destroy(gameObject);
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
    }
}
