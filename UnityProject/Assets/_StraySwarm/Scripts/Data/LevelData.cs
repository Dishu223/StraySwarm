using System;
using System.Collections.Generic;
using UnityEngine;

namespace StraySwarm.Data
{
    [Serializable]
    public class AnimalSpawnEntry
    {
        public AnimalType Type;
        public Vector2Int GridPosition;
    }

    [Serializable]
    public class StationSpawnEntry
    {
        public AnimalType TargetType;
        public Vector2Int GridPosition;
        public int Capacity = 3;
    }

    /// <summary>
    /// ScriptableObject defining the complete, handcrafted layout, objectives, and thresholds of a single level.
    /// </summary>
    [CreateAssetMenu(fileName = "LevelData_01", menuName = "Stray Swarm/Level Data")]
    public class LevelData : ScriptableObject
    {
        [Header("Level Identity")]
        public int LevelID = 1;
        public string LevelName = "Level 1";
        public WorldTheme World = WorldTheme.Desert;

        [Header("Timing & Star Thresholds")]
        [Tooltip("Total time in seconds to beat the level.")]
        public float TimeLimit = 60f;

        [Tooltip("Percentage of time remaining for 3 Stars (e.g. 0.6 = 60% of TimeLimit left).")]
        [Range(0.1f, 0.9f)] public float ThreeStarPercentage = 0.6f;

        [Tooltip("Percentage of time remaining for 2 Stars (e.g. 0.3 = 30% of TimeLimit left).")]
        [Range(0.05f, 0.5f)] public float TwoStarPercentage = 0.3f;

        [Header("Rewards")]
        public int CoinReward = 50;

        [Header("Animal Spawns (Fixed Placements)")]
        public List<AnimalSpawnEntry> AnimalSpawns = new List<AnimalSpawnEntry>();

        [Header("Stations (Fixed Placements)")]
        public List<StationSpawnEntry> Stations = new List<StationSpawnEntry>();

        [Header("Legacy Van Support")]
        public List<string> VanSequence = new List<string>();

        // Legacy compatibility properties
        public float ThreeStarTimeRemaining => TimeLimit * ThreeStarPercentage;
        public float TwoStarTimeRemaining => TimeLimit * TwoStarPercentage;
    }
}
