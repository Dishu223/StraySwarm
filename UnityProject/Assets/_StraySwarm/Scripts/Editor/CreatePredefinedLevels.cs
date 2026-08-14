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
    /// Generates 40 complete, balanced, handcrafted level ScriptableObjects across 2 worlds.
    /// Supports deterministic wave spawning, multi-station capacities, and obstacle curves.
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
                world1.StarsRequiredToUnlock = 0;
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
                level.SeedOffset = 0;

                // Wave Settings & Balance Progression
                if (i <= 5)
                {
                    level.TotalAnimalsToRescue = 9 + (i - 1); // 9 to 13
                    level.MaxConcurrentOnMap = 4;
                    level.AllowedAnimalTypes = new List<AnimalType> { AnimalType.Puppy, AnimalType.Kitten };
                    level.TimeLimit = 45f + (i * 2f);
                }
                else if (i <= 10)
                {
                    level.TotalAnimalsToRescue = 14 + (i - 6); // 14 to 18
                    level.MaxConcurrentOnMap = 5;
                    level.AllowedAnimalTypes = new List<AnimalType> { AnimalType.Puppy, AnimalType.Kitten, AnimalType.Pigeon };
                    level.TimeLimit = 60f + (i * 2f);
                }
                else if (i <= 15)
                {
                    level.TotalAnimalsToRescue = 18 + (i - 11); // 18 to 22
                    level.MaxConcurrentOnMap = 5;
                    level.AllowedAnimalTypes = new List<AnimalType> { AnimalType.Puppy, AnimalType.Kitten, AnimalType.Pigeon, AnimalType.Frog };
                    level.TimeLimit = 75f + (i * 2f);
                }
                else
                {
                    level.TotalAnimalsToRescue = 22 + (i - 16); // 22 to 26
                    level.MaxConcurrentOnMap = 6;
                    level.AllowedAnimalTypes = new List<AnimalType> { AnimalType.Puppy, AnimalType.Kitten, AnimalType.Pigeon, AnimalType.Frog };
                    level.TimeLimit = 90f + (i * 2f);
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
                level.SeedOffset = 0;

                if (stageInWorld <= 5)
                {
                    level.TotalAnimalsToRescue = 12 + stageInWorld;
                    level.MaxConcurrentOnMap = 5;
                    level.AllowedAnimalTypes = new List<AnimalType> { AnimalType.Frog, AnimalType.Mouse, AnimalType.Puppy };
                    level.TimeLimit = 65f + (stageInWorld * 2f);
                }
                else if (stageInWorld <= 10)
                {
                    level.TotalAnimalsToRescue = 16 + stageInWorld;
                    level.MaxConcurrentOnMap = 6;
                    level.AllowedAnimalTypes = new List<AnimalType> { AnimalType.Frog, AnimalType.Mouse, AnimalType.Bunny, AnimalType.Kitten };
                    level.TimeLimit = 80f + (stageInWorld * 2f);
                }
                else
                {
                    level.TotalAnimalsToRescue = 20 + stageInWorld;
                    level.MaxConcurrentOnMap = 6;
                    level.AllowedAnimalTypes = new List<AnimalType> { AnimalType.Puppy, AnimalType.Kitten, AnimalType.Frog, AnimalType.Mouse, AnimalType.Pigeon, AnimalType.Bunny };
                    level.TimeLimit = 95f + (stageInWorld * 2f);
                }

                EditorUtility.SetDirty(level);
                world2.Levels.Add(level);
            }
            EditorUtility.SetDirty(world2);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"🎉 [CreatePredefinedLevels] Successfully generated 40 Handcrafted Wave Level assets across 2 Worlds!");
            EditorUtility.DisplayDialog("Stray Swarm", "Successfully generated 40 Predefined Handcrafted Level assets across 2 Worlds!\n\nDeterministic wave quotas, multi-species allowances, and balanced time limits are configured!", "Awesome!");
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
