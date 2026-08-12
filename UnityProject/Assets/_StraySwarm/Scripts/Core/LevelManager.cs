using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using StraySwarm.Data;

namespace StraySwarm.Core
{
    /// <summary>
    /// Manages loading levels, restarting, and progressing through the game playlist.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [Header("Level Playlist")]
        [Tooltip("The list of all levels in your game!")]
        [SerializeField] private List<LevelData> _levelPlaylist = new List<LevelData>();

        private int _currentLevelIndex = 0;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public LevelData GetCurrentLevelData()
        {
            if (_levelPlaylist.Count == 0) return null;
            return _levelPlaylist[Mathf.Clamp(_currentLevelIndex, 0, _levelPlaylist.Count - 1)];
        }

        public void LoadNextLevel()
        {
            _currentLevelIndex++;
            if (_currentLevelIndex >= _levelPlaylist.Count)
            {
                _currentLevelIndex = 0; // Loop back to level 1 (or show Game Completed screen!)
            }

            // Reload the main scene to start the next level fresh!
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void RestartCurrentLevel()
        {
            // Reload current level
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
