#if UNITY_EDITOR
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using StraySwarm.Data;
using StraySwarm.Gameplay;

namespace StraySwarm.Editor
{
    /// <summary>
    /// Custom Unity Editor tool to generate 40 complete, balanced, handcrafted level ScriptableObjects across 2 worlds!
    /// Menu: Stray Swarm -> 🌟 Generate 40 Predefined Handcrafted Levels (2 Worlds)
    /// </summary>
    public static class CreatePredefinedLevels
    {
        [MenuItem("Stray Swarm/🌟 Generate 40 Predefined Handcrafted Levels (2 Worlds)")]
        public static void GenerateAllLevels()
        {
            string baseDataPath = "Assets/_StraySwarm/Data";
            string worldsPath = Path.Combine(baseDataPath, "Worlds");
            string levelsPath = Path.Combine(baseDataPath, "Levels");
            string world1Path = Path.Combine(levelsPath, "World_01_Desert");
            string world2Path = Path.Combine(levelsPath, "World_02_Forest");

            EnsureDirectory(worldsPath);
            EnsureDirectory(world1Path);
            EnsureDirectory(world2Path);

            // 1. Generate World 1: Desert Oasis (Levels 1–20)
            WorldData world1 = AssetDatabase.LoadAssetAtPath<WorldData>($"{worldsPath}/World_01_Desert.asset");
            if (world1 == null)
            {
                world1 = ScriptableObject.CreateInstance<WorldData>();
                world1.WorldName = "Desert Oasis";
                world1.Theme = WorldTheme.Desert;
                world1.WorldColor = new Color(0.96f, 0.73f, 0.42f);
                world1.StarsRequiredToUnlock = 0; // Unlocked from start!
                AssetDatabase.CreateAsset(world1, $"{worldsPath}/World_01_Desert.asset");
            }
            world1.Levels.Clear();

            for (int i = 1; i <= 20; i++)
            {
                string assetPath = $"{world1Path}/Level_{i:D2}.asset";
                LevelData level = AssetDatabase.LoadAssetAtPath<LevelData>(assetPath);
                if (level == null)
                {
                    level = ScriptableObject.CreateInstance<LevelData>();
                    AssetDatabase.CreateAsset(level, assetPath);
                }

                level.LevelID = i;
                level.LevelName = $"Desert - Stage {i}";
                level.World = WorldTheme.Desert;
                level.ThreeStarPercentage = 0.6f;
                level.TwoStarPercentage = 0.3f;
                level.CoinReward = 30 + (i * 2);

                // Clear lists
                level.AnimalSpawns.Clear();
                level.Stations.Clear();
                level.OneWayArrows.Clear();
                level.NumberedWalls.Clear();

                // Handcrafted placements
                if (i <= 5)
                {
                    level.TimeLimit = 45f;
                    level.Stations.Add(new StationSpawnEntry { TargetType = AnimalType.Puppy, GridPosition = new Vector2Int(3, 4), Capacity = 3 });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Puppy, GridPosition = new Vector2Int(-1, 0) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Puppy, GridPosition = new Vector2Int(0, 0) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Puppy, GridPosition = new Vector2Int(1, 0) });
                    if (i >= 3)
                    {
                        level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Kitten, GridPosition = new Vector2Int(0, -1) });
                    }
                }
                else if (i <= 10)
                {
                    level.TimeLimit = 60f;
                    level.Stations.Add(new StationSpawnEntry { TargetType = AnimalType.Puppy, GridPosition = new Vector2Int(3, 4), Capacity = 3 });
                    level.Stations.Add(new StationSpawnEntry { TargetType = AnimalType.Kitten, GridPosition = new Vector2Int(-3, 4), Capacity = 3 });
                    
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Puppy, GridPosition = new Vector2Int(-2, 0) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Puppy, GridPosition = new Vector2Int(2, 0) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Kitten, GridPosition = new Vector2Int(-1, -1) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Kitten, GridPosition = new Vector2Int(1, -1) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Pigeon, GridPosition = new Vector2Int(0, 1) });
                    
                    if (i >= 8)
                    {
                        level.OneWayArrows.Add(new OneWayArrowSpawnEntry { Direction = ArrowDirection.Right, GridPosition = new Vector2Int(-1, 2) });
                    }
                }
                else if (i <= 15)
                {
                    level.TimeLimit = 75f;
                    level.Stations.Add(new StationSpawnEntry { TargetType = AnimalType.Puppy, GridPosition = new Vector2Int(3, 4), Capacity = 3 });
                    level.Stations.Add(new StationSpawnEntry { TargetType = AnimalType.Pigeon, GridPosition = new Vector2Int(-3, 4), Capacity = 3 });

                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Puppy, GridPosition = new Vector2Int(-2, -1) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Puppy, GridPosition = new Vector2Int(2, -1) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Kitten, GridPosition = new Vector2Int(-1, 0) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Pigeon, GridPosition = new Vector2Int(1, 0) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Pigeon, GridPosition = new Vector2Int(0, 2) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Frog, GridPosition = new Vector2Int(0, -2) });

                    level.OneWayArrows.Add(new OneWayArrowSpawnEntry { Direction = ArrowDirection.Up, GridPosition = new Vector2Int(0, -1) });
                    level.NumberedWalls.Add(new NumberedWallSpawnEntry { HitPoints = 2, GridPosition = new Vector2Int(1, 1) });
                }
                else
                {
                    level.TimeLimit = 90f;
                    level.Stations.Add(new StationSpawnEntry { TargetType = AnimalType.Puppy, GridPosition = new Vector2Int(3, 4), Capacity = 4 });
                    level.Stations.Add(new StationSpawnEntry { TargetType = AnimalType.Frog, GridPosition = new Vector2Int(-3, 4), Capacity = 3 });

                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Puppy, GridPosition = new Vector2Int(-2, 0) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Puppy, GridPosition = new Vector2Int(2, 0) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Kitten, GridPosition = new Vector2Int(-1, 1) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Kitten, GridPosition = new Vector2Int(1, 1) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Pigeon, GridPosition = new Vector2Int(-1, -2) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Frog, GridPosition = new Vector2Int(1, -2) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Frog, GridPosition = new Vector2Int(0, 3) });

                    level.OneWayArrows.Add(new OneWayArrowSpawnEntry { Direction = ArrowDirection.Right, GridPosition = new Vector2Int(-1, 2) });
                    level.OneWayArrows.Add(new OneWayArrowSpawnEntry { Direction = ArrowDirection.Left, GridPosition = new Vector2Int(1, -1) });
                    level.NumberedWalls.Add(new NumberedWallSpawnEntry { HitPoints = 3, GridPosition = new Vector2Int(0, 0) });
                }

                EditorUtility.SetDirty(level);
                world1.Levels.Add(level);
            }
            EditorUtility.SetDirty(world1);

            // 2. Generate World 2: Whispering Forest (Levels 21–40)
            WorldData world2 = AssetDatabase.LoadAssetAtPath<WorldData>($"{worldsPath}/World_02_Forest.asset");
            if (world2 == null)
            {
                world2 = ScriptableObject.CreateInstance<WorldData>();
                world2.WorldName = "Whispering Forest";
                world2.Theme = WorldTheme.Forest;
                world2.WorldColor = new Color(0.36f, 0.72f, 0.36f);
                world2.StarsRequiredToUnlock = 30; // Requires 30 stars from World 1!
                AssetDatabase.CreateAsset(world2, $"{worldsPath}/World_02_Forest.asset");
            }
            world2.Levels.Clear();

            for (int i = 21; i <= 40; i++)
            {
                string assetPath = $"{world2Path}/Level_{i:D2}.asset";
                LevelData level = AssetDatabase.LoadAssetAtPath<LevelData>(assetPath);
                if (level == null)
                {
                    level = ScriptableObject.CreateInstance<LevelData>();
                    AssetDatabase.CreateAsset(level, assetPath);
                }

                int stageInWorld = i - 20;
                level.LevelID = i;
                level.LevelName = $"Forest - Stage {stageInWorld}";
                level.World = WorldTheme.Forest;
                level.ThreeStarPercentage = 0.6f;
                level.TwoStarPercentage = 0.3f;
                level.CoinReward = 50 + (stageInWorld * 3);

                level.AnimalSpawns.Clear();
                level.Stations.Clear();
                level.OneWayArrows.Clear();
                level.NumberedWalls.Clear();

                if (stageInWorld <= 5)
                {
                    level.TimeLimit = 60f;
                    level.Stations.Add(new StationSpawnEntry { TargetType = AnimalType.Frog, GridPosition = new Vector2Int(3, 4), Capacity = 3 });
                    level.Stations.Add(new StationSpawnEntry { TargetType = AnimalType.Mouse, GridPosition = new Vector2Int(-3, 4), Capacity = 3 });

                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Frog, GridPosition = new Vector2Int(-1, 0) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Frog, GridPosition = new Vector2Int(1, 0) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Mouse, GridPosition = new Vector2Int(0, -1) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Mouse, GridPosition = new Vector2Int(0, 1) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Puppy, GridPosition = new Vector2Int(-2, 2) });

                    level.NumberedWalls.Add(new NumberedWallSpawnEntry { HitPoints = 2, GridPosition = new Vector2Int(0, 0) });
                }
                else if (stageInWorld <= 10)
                {
                    level.TimeLimit = 75f;
                    level.Stations.Add(new StationSpawnEntry { TargetType = AnimalType.Frog, GridPosition = new Vector2Int(3, 4), Capacity = 3 });
                    level.Stations.Add(new StationSpawnEntry { TargetType = AnimalType.Bunny, GridPosition = new Vector2Int(-3, 4), Capacity = 3 });

                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Frog, GridPosition = new Vector2Int(-2, -1) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Mouse, GridPosition = new Vector2Int(2, -1) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Bunny, GridPosition = new Vector2Int(0, 2) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Bunny, GridPosition = new Vector2Int(-1, 0) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Pigeon, GridPosition = new Vector2Int(1, 0) });

                    level.OneWayArrows.Add(new OneWayArrowSpawnEntry { Direction = ArrowDirection.Up, GridPosition = new Vector2Int(0, 1) });
                    level.NumberedWalls.Add(new NumberedWallSpawnEntry { HitPoints = 3, GridPosition = new Vector2Int(-1, -1) });
                }
                else
                {
                    level.TimeLimit = 95f;
                    level.Stations.Add(new StationSpawnEntry { TargetType = AnimalType.Bunny, GridPosition = new Vector2Int(3, 4), Capacity = 4 });
                    level.Stations.Add(new StationSpawnEntry { TargetType = AnimalType.Mouse, GridPosition = new Vector2Int(-3, 4), Capacity = 3 });

                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Puppy, GridPosition = new Vector2Int(-2, 0) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Kitten, GridPosition = new Vector2Int(2, 0) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Frog, GridPosition = new Vector2Int(-1, 1) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Mouse, GridPosition = new Vector2Int(1, 1) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Pigeon, GridPosition = new Vector2Int(-1, -2) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Bunny, GridPosition = new Vector2Int(1, -2) });
                    level.AnimalSpawns.Add(new AnimalSpawnEntry { Type = AnimalType.Bunny, GridPosition = new Vector2Int(0, 3) });

                    level.OneWayArrows.Add(new OneWayArrowSpawnEntry { Direction = ArrowDirection.Right, GridPosition = new Vector2Int(-1, 2) });
                    level.OneWayArrows.Add(new OneWayArrowSpawnEntry { Direction = ArrowDirection.Down, GridPosition = new Vector2Int(1, 2) });
                    level.NumberedWalls.Add(new NumberedWallSpawnEntry { HitPoints = 3, GridPosition = new Vector2Int(0, 0) });
                    level.NumberedWalls.Add(new NumberedWallSpawnEntry { HitPoints = 2, GridPosition = new Vector2Int(0, -1) });
                }

                EditorUtility.SetDirty(level);
                world2.Levels.Add(level);
            }
            EditorUtility.SetDirty(world2);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"🎉 [CreatePredefinedLevels] Successfully generated 40 Handcrafted Levels across 2 Worlds with complete animal, station, and obstacle placements!");
            EditorUtility.DisplayDialog("Stray Swarm", "Successfully generated 40 Predefined Handcrafted Level assets across 2 Worlds!\n\nAll animals, delivery stations, one-way arrows, and breakable walls are configured!", "Awesome!");
        }

        private static void EnsureDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
#endif
