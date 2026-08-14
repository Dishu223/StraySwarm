using System;
using System.Collections.Generic;
using UnityEngine;
using StraySwarm.Gameplay;

namespace StraySwarm.Data
{
    [Serializable]
    public class ScheduledWaveSpawn
    {
        public int SpawnPointIndex;
        public AnimalType Type;
    }

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

    [Serializable]
    public class OneWayArrowSpawnEntry
    {
        public ArrowDirection Direction;
        public Vector2Int GridPosition;
    }

    [Serializable]
    public class NumberedWallSpawnEntry
    {
        public int HitPoints = 3;
        public Vector2Int GridPosition;
    }

    /// <summary>
    /// ScriptableObject defining the complete, handcrafted layout, objectives, and thresholds of a single level.
    /// Supports deterministic wave schedules, fixed entity placements, and dynamic progression.
    /// </summary>
    [CreateAssetMenu(fileName = "LevelData_01", menuName = "Stray Swarm/Level Data")]
    public class LevelData : ScriptableObject
    {
        [Header("Level Identity")]
        public int LevelID = 1;
        public string LevelName = "Level 1";
        public WorldTheme World = WorldTheme.Desert;

        [Header("Handcrafted Level Map Prefab")]
        [Tooltip("The prefab containing the Tilemap, Spawn Points, Stations, and Obstacles for this level (instantiated dynamically).")]
        public GameObject MapPrefab;

        [Header("Wave Spawning Settings")]
        [Tooltip("Total number of animals required to rescue and deliver to finish the level.")]
        public int TotalAnimalsToRescue = 12;

        [Tooltip("Maximum animals allowed on the map at the same time.")]
        public int MaxConcurrentOnMap = 5;

        [Tooltip("Animal species allowed to appear in this level.")]
        public List<AnimalType> AllowedAnimalTypes = new List<AnimalType> { AnimalType.Puppy, AnimalType.Kitten };

        [Tooltip("Seed offset for deterministic reproduction (defaults to LevelID * 1000).")]
        public int SeedOffset = 0;

        [Header("Fixed Wave Schedule (Optional Handcrafted Sequence)")]
        [Tooltip("If populated, the level will follow this exact pre-determined sequence of (SpawnPointIndex, AnimalType) on every playthrough.")]
        public List<ScheduledWaveSpawn> FixedWaveSchedule = new List<ScheduledWaveSpawn>();

        [Header("Player Start Position")]
        public Vector2Int PlayerStartCell = new Vector2Int(0, 0);

        [Header("Timing & Star Thresholds")]
        [Tooltip("Total time in seconds to beat the level.")]
        public float TimeLimit = 60f;

        [Tooltip("Percentage of time remaining for 3 Stars (e.g. 0.6 = 60% of TimeLimit left).")]
        [Range(0.1f, 0.9f)] public float ThreeStarPercentage = 0.6f;

        [Tooltip("Percentage of time remaining for 2 Stars (e.g. 0.3 = 30% of TimeLimit left).")]
        [Range(0.05f, 0.5f)] public float TwoStarPercentage = 0.3f;

        [Header("Rewards")]
        public int CoinReward = 50;

        [Header("Fixed Placements (Optional Overrides)")]
        public List<AnimalSpawnEntry> AnimalSpawns = new List<AnimalSpawnEntry>();
        public List<StationSpawnEntry> Stations = new List<StationSpawnEntry>();
        public List<OneWayArrowSpawnEntry> OneWayArrows = new List<OneWayArrowSpawnEntry>();
        public List<NumberedWallSpawnEntry> NumberedWalls = new List<NumberedWallSpawnEntry>();

        [Header("Legacy Van Support")]
        public List<string> VanSequence = new List<string>();

        // Helpers
        public float ThreeStarTimeRemaining => TimeLimit * ThreeStarPercentage;
        public float TwoStarTimeRemaining => TimeLimit * TwoStarPercentage;
        public int GetDeterministicSeed() => (LevelID * 1000) + SeedOffset;
    }
}
