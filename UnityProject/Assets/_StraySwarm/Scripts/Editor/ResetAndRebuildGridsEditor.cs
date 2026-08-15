#if UNITY_EDITOR
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEditor.SceneManagement;
using StraySwarm.Data;
using StraySwarm.Gameplay;
using StraySwarm.Utils;

namespace StraySwarm.Editor
{
    /// <summary>
    /// Master tool to reset, standardize, and wire all level grids, prefabs, and playlists.
    /// Menu: Stray Swarm -> 🔄 Reset & Rebuild Standardized Grids (Level 1 & 2)
    /// </summary>
    public static class ResetAndRebuildGridsEditor
    {
        [MenuItem("Stray Swarm/🔄 Reset & Rebuild Standardized Grids (Level 1 & 2)", false, 20)]
        public static void RebuildAllGrids()
        {
            string prefabDir = "Assets/_StraySwarm/Prefabs/Levels";
            string levelDataDir = "Assets/_StraySwarm/Data/Levels";
            string world01Dir = "Assets/_StraySwarm/Data/Levels/World_01_Desert";

            if (!Directory.Exists(prefabDir)) Directory.CreateDirectory(prefabDir);
            if (!Directory.Exists(levelDataDir)) Directory.CreateDirectory(levelDataDir);
            if (!Directory.Exists(world01Dir)) Directory.CreateDirectory(world01Dir);

            // Load white tile asset
            TileBase whiteTile = AssetDatabase.LoadAssetAtPath<TileBase>("Assets/_StraySwarm/Art/Square.asset");
            if (whiteTile == null)
            {
                string[] guids = AssetDatabase.FindAssets("t:TileBase", new[] { "Assets/_StraySwarm" });
                if (guids.Length > 0)
                {
                    whiteTile = AssetDatabase.LoadAssetAtPath<TileBase>(AssetDatabase.GUIDToAssetPath(guids[0]));
                }
            }

            GameObject stationPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_StraySwarm/Prefabs/Environment/RescueStation.prefab");

            // ==========================================
            // 1. BUILD LEVEL 01 MAP PREFAB FROM SCRATCH
            // ==========================================
            GameObject map01Root = new GameObject("Level_01_Map");
            GameObject grid01 = new GameObject("Grid");
            grid01.transform.SetParent(map01Root.transform, false);
            Grid g01 = grid01.AddComponent<Grid>();
            g01.cellSize = new Vector3(1f, 1f, 0f);

            GameObject tm01 = new GameObject("FloorTilemap");
            tm01.transform.SetParent(grid01.transform, false);
            Tilemap tilemap01 = tm01.AddComponent<Tilemap>();
            tilemap01.tileAnchor = new Vector3(0.5f, 0.5f, 0f);
            TilemapRenderer tmr01 = tm01.AddComponent<TilemapRenderer>();
            tmr01.sortingOrder = 0;

            // Paint Level 1 layout (2-room figure 8 loop)
            List<Vector3Int> level1Cells = new List<Vector3Int>();

            // Top bar (y = 3, x = -4 to 3)
            for (int x = -4; x <= 3; x++) level1Cells.Add(new Vector3Int(x, 3, 0));
            // Middle bar (y = -2, x = -4 to 3)
            for (int x = -4; x <= 3; x++) level1Cells.Add(new Vector3Int(x, -2, 0));
            // Bottom bar (y = -7, x = -4 to 3)
            for (int x = -4; x <= 3; x++) level1Cells.Add(new Vector3Int(x, -7, 0));
            // Left column (x = -4, y = -7 to 3)
            for (int y = -7; y <= 3; y++) level1Cells.Add(new Vector3Int(-4, y, 0));
            // Right column (x = 3, y = -7 to 3)
            for (int y = -7; y <= 3; y++) level1Cells.Add(new Vector3Int(3, y, 0));

            foreach (var cell in level1Cells)
            {
                if (whiteTile != null) tilemap01.SetTile(cell, whiteTile);
            }

            // Player Spawn Point
            GameObject psp01 = new GameObject("PlayerSpawnPoint");
            psp01.transform.SetParent(map01Root.transform, false);
            psp01.transform.localPosition = new Vector3(-3.5f, 3.5f, 0f);
            psp01.AddComponent<PlayerSpawnPoint>();

            // Spawn Points container
            GameObject spContainer01 = new GameObject("AnimalSpawnPoints");
            spContainer01.transform.SetParent(map01Root.transform, false);

            Vector3Int[] spawnCoords01 = new Vector3Int[]
            {
                new Vector3Int(-4, 0, 0),
                new Vector3Int(-4, 1, 0),
                new Vector3Int(-4, -4, 0),
                new Vector3Int(-4, -5, 0),
                new Vector3Int(3, 0, 0),
                new Vector3Int(3, 1, 0),
                new Vector3Int(3, -4, 0),
                new Vector3Int(3, -5, 0),
                new Vector3Int(-1, -2, 0),
                new Vector3Int(0, -2, 0)
            };

            int idx = 0;
            foreach (var sc in spawnCoords01)
            {
                Vector3 centerPos = new Vector3(sc.x + 0.5f, sc.y + 0.5f, 0f);
                GameObject spGo = new GameObject($"AnimalSpawnPoint ({idx})");
                spGo.transform.SetParent(spContainer01.transform, false);
                spGo.transform.localPosition = centerPos;
                spGo.AddComponent<AnimalSpawnPoint>();
                idx++;
            }

            // Stations container
            GameObject stations01 = new GameObject("Stations");
            stations01.transform.SetParent(map01Root.transform, false);
            if (stationPrefab != null)
            {
                GameObject stGo = (GameObject)PrefabUtility.InstantiatePrefab(stationPrefab, stations01.transform);
                stGo.transform.localPosition = new Vector3(2.5f, 3.5f, 0f);
            }

            // Obstacles container
            GameObject obstacles01 = new GameObject("Obstacles");
            obstacles01.transform.SetParent(map01Root.transform, false);

            // Save Level 1 Prefab
            string prefab01Path = $"{prefabDir}/Level_01_Map.prefab";
            GameObject level1Prefab = PrefabUtility.SaveAsPrefabAsset(map01Root, prefab01Path);
            Object.DestroyImmediate(map01Root);

            // Update Level_01.asset in World_01_Desert
            string data01WorldPath = $"{world01Dir}/Level_01.asset";
            LevelData data01 = AssetDatabase.LoadAssetAtPath<LevelData>(data01WorldPath);
            if (data01 != null)
            {
                data01.MapPrefab = level1Prefab;
                data01.TotalAnimalsToRescue = 10;
                data01.MaxConcurrentOnMap = 4;
                data01.TimeLimit = 60f;
                EditorUtility.SetDirty(data01);
            }

            // Update Level_02.asset in World_01_Desert
            string prefab02Path = $"{prefabDir}/Level_02_Map.prefab";
            GameObject level2Prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefab02Path);
            string data02WorldPath = $"{world01Dir}/Level_02.asset";
            LevelData data02 = AssetDatabase.LoadAssetAtPath<LevelData>(data02WorldPath);
            if (data02 != null && level2Prefab != null)
            {
                data02.MapPrefab = level2Prefab;
                data02.TotalAnimalsToRescue = 12;
                data02.MaxConcurrentOnMap = 5;
                data02.TimeLimit = 60f;
                EditorUtility.SetDirty(data02);
            }

            // Ensure PlayerSpawnPoint exists in Level_02_Map.prefab
            if (level2Prefab != null)
            {
                GameObject l2Instance = (GameObject)PrefabUtility.InstantiatePrefab(level2Prefab);
                if (l2Instance.GetComponentInChildren<PlayerSpawnPoint>() == null)
                {
                    GameObject psp02 = new GameObject("PlayerSpawnPoint");
                    psp02.transform.SetParent(l2Instance.transform, false);
                    psp02.transform.localPosition = new Vector3(-3.5f, 4.5f, 0f);
                    psp02.AddComponent<PlayerSpawnPoint>();
                    PrefabUtility.SaveAsPrefabAsset(l2Instance, prefab02Path);
                }
                Object.DestroyImmediate(l2Instance);
            }

            // ==========================================
            // 2. CLEAN STATIC PROTOTYPES FROM SAMPLE SCENE
            // ==========================================
            var oldRoots = new string[] { "Grid", "AnimalSpawnPoints", "Stations", "Obstacles", "RescueStation", "NumberedWall_3", "OneWayArrow_Down" };
            foreach (var rName in oldRoots)
            {
                GameObject obj = GameObject.Find(rName);
                if (obj != null && obj.transform.parent == null)
                {
                    Undo.DestroyObjectImmediate(obj);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            Debug.Log("🎉 [ResetAndRebuildGridsEditor] Level 1 & Level 2 prefabs and World 1 data fully standardized with PlayerSpawnPoints!");
            EditorUtility.DisplayDialog("Stray Swarm", "Successfully standardized Level 1 & 2 Prefabs, linked MapPrefabs in World 1, added PlayerSpawnPoints, and cleaned the Scene shell!\n\nPress Play to test Level 1 or Level 2 seamlessly!", "Awesome!");
        }
    }
}
#endif
