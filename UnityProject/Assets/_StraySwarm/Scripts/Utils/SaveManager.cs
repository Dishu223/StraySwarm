using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace StraySwarm.Utils
{
    [Serializable]
    public class SaveData
    {
        public int UnlockedLevel = 1;
        public int TotalCoins = 0;
        public string SelectedSkinId = "DefaultTabby";
        public List<int> LevelStars = new List<int>(); // Stars earned per level index
    }

    /// <summary>
    /// Handles JSON saving and loading to Application.persistentDataPath.
    /// Persists unlocked levels, stars, coins, and selected skins across game restarts.
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
                Debug.Log($"[SaveManager] Saved game data to {SaveFilePath}");
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
                    Debug.Log($"[SaveManager] Loaded save data. Unlocked Level: {Data.UnlockedLevel}, Coins: {Data.TotalCoins}");
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
            if (levelIndex + 1 > Data.UnlockedLevel)
            {
                Data.UnlockedLevel = levelIndex + 1;
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

        public void AddCoins(int amount)
        {
            Data.TotalCoins += amount;
            SaveGame();
        }
    }
}
