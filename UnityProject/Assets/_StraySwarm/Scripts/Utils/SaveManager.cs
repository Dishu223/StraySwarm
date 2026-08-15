using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace StraySwarm.Utils
{
    [Serializable]
    public class SaveData
    {
        public int SaveVersion = 1;
        public int UnlockedLevel = 1;
        public int TotalCoins = 0;
        public string SelectedSkinId = "DefaultTabby";
        public List<int> LevelStars = new List<int>(); // Stars earned per level index (0-indexed)

        // Settings
        public float SFXVolume = 1f;
        public float MusicVolume = 0.7f;
        public bool HapticsEnabled = true;
        public bool ColorblindMode = false;
    }

    /// <summary>
    /// Handles persistent JSON saving and loading to Application.persistentDataPath.
    /// Persists unlocked levels, stars, coins, skins, and audio settings across game restarts.
    /// </summary>
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        public SaveData Data { get; private set; } = new SaveData();

        private string SaveFilePath => Path.Combine(Application.persistentDataPath, "strayswarm_save.json");

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadGame();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SaveGame()
        {
            try
            {
                string json = JsonUtility.ToJson(Data, true);
                File.WriteAllText(SaveFilePath, json);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveManager] Failed to save game: {ex.Message}");
            }
        }

        public void LoadGame()
        {
            try
            {
                if (File.Exists(SaveFilePath))
                {
                    string json = File.ReadAllText(SaveFilePath);
                    Data = JsonUtility.FromJson<SaveData>(json);
                    Debug.Log($"[SaveManager] Loaded save data. Unlocked Level: {Data.UnlockedLevel}, Total Stars: {GetTotalStars()}, Coins: {Data.TotalCoins}");
                }
                else
                {
                    Data = new SaveData();
                    SaveGame(); // Create fresh save file
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveManager] Failed to load save, creating fresh: {ex.Message}");
                Data = new SaveData();
            }
        }

        public int GetCoins()
        {
            return Data != null ? Data.TotalCoins : 0;
        }

        public void AddCoins(int amount)
        {
            if (Data != null)
            {
                Data.TotalCoins += amount;
                SaveGame();
            }
        }

        public void SaveLevelStars(int levelIndex, int stars)
        {
            // Expand list if needed
            while (Data.LevelStars.Count <= levelIndex)
            {
                Data.LevelStars.Add(0);
            }

            if (stars > Data.LevelStars[levelIndex])
            {
                Data.LevelStars[levelIndex] = stars;
            }

            // Unlock next level if completed
            if (levelIndex + 1 >= Data.UnlockedLevel)
            {
                Data.UnlockedLevel = levelIndex + 2;
            }

            SaveGame();
        }

        public int GetStarsForLevel(int levelIndex)
        {
            if (levelIndex >= 0 && levelIndex < Data.LevelStars.Count)
            {
                return Data.LevelStars[levelIndex];
            }
            return 0;
        }

        /// <summary>
        /// Calculates the total cumulative stars earned across all levels (used for world unlock gates).
        /// </summary>
        public int GetTotalStars()
        {
            int total = 0;
            if (Data.LevelStars != null)
            {
                for (int i = 0; i < Data.LevelStars.Count; i++)
                {
                    total += Data.LevelStars[i];
                }
            }
            return total;
        }

        public bool IsWorldUnlocked(int starsRequired)
        {
            return GetTotalStars() >= starsRequired;
        }

        public void AddCoins(int amount)
        {
            Data.TotalCoins += amount;
            SaveGame();
        }

        public void SaveSettings(float sfx, float music, bool haptics, bool colorblind)
        {
            Data.SFXVolume = sfx;
            Data.MusicVolume = music;
            Data.HapticsEnabled = haptics;
            Data.ColorblindMode = colorblind;
            SaveGame();
        }
    }
}
