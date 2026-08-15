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
    /// Master tool to standardize level prefabs, anchors, and playlists WITHOUT modifying user-painted tilemaps.
    /// Menu: Stray Swarm -> 🔄 Sync & Standardize Level Settings (Level 1 & 2)
    /// </summary>
    public static class ResetAndRebuildGridsEditor
    {
        [MenuItem("Stray Swarm/🔄 Sync & Standardize Level Settings (Level 1 & 2)", false, 20)]
        public static void RebuildAllGrids()
        {
            string prefabDir = "Assets/_StraySwarm/Prefabs/Levels";
            string levelDataDir = "Assets/_StraySwarm/Data/Levels";
            string world01Dir = "Assets/_StraySwarm/Data/Levels/World_01_Desert";

            if (!Directory.Exists(prefabDir)) Directory.CreateDirectory(prefabDir);
            if (!Directory.Exists(levelDataDir)) Directory.CreateDirectory(levelDataDir);
            if (!Directory.Exists(world01Dir)) Directory.CreateDirectory(world01Dir);

            // 0. Ensure PlayerCat prefab exists
            CreatePlayerPrefab.GeneratePlayerPrefab();

            // 1. Process Level_01_Map.prefab non-destructively (never overwrite painted tiles)
            string prefab01Path = $"{prefabDir}/Level_01_Map.prefab";
            GameObject level1Prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefab01Path);
            if (level1Prefab != null)
            {
                GameObject l1Instance = (GameObject)PrefabUtility.InstantiatePrefab(level1Prefab);
                var tm = l1Instance.GetComponentInChildren<Tilemap>(true);
                if (tm != null) tm.tileAnchor = new Vector3(0.5f, 0.5f, 0f);

                if (l1Instance.GetComponentInChildren<PlayerSpawnPoint>() == null)
                {
                    GameObject psp = new GameObject("PlayerSpawnPoint");
                    psp.transform.SetParent(l1Instance.transform, false);
                    psp.transform.localPosition = new Vector3(-3.5f, 3.5f, 0f);
                    psp.AddComponent<PlayerSpawnPoint>();
                }

                if (l1Instance.GetComponentInChildren<RescueStation>() == null)
                {
                    GameObject stationPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_StraySwarm/Prefabs/Environment/RescueStation.prefab");
                    if (stationPrefab != null)
                    {
                        GameObject stGo = (GameObject)PrefabUtility.InstantiatePrefab(stationPrefab, l1Instance.transform);
                        stGo.transform.localPosition = new Vector3(2.5f, 3.5f, 0f);
                    }
                }

                PrefabUtility.SaveAsPrefabAsset(l1Instance, prefab01Path);
                Object.DestroyImmediate(l1Instance);
            }

            // 2. Process Level_02_Map.prefab non-destructively
            string prefab02Path = $"{prefabDir}/Level_02_Map.prefab";
            GameObject level2Prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefab02Path);
            if (level2Prefab != null)
            {
                GameObject l2Instance = (GameObject)PrefabUtility.InstantiatePrefab(level2Prefab);
                var tm = l2Instance.GetComponentInChildren<Tilemap>(true);
                if (tm != null) tm.tileAnchor = new Vector3(0.5f, 0.5f, 0f);

                if (l2Instance.GetComponentInChildren<PlayerSpawnPoint>() == null)
                {
                    GameObject psp02 = new GameObject("PlayerSpawnPoint");
                    psp02.transform.SetParent(l2Instance.transform, false);
                    psp02.transform.localPosition = new Vector3(-3.5f, 4.5f, 0f);
                    psp02.AddComponent<PlayerSpawnPoint>();
                }

                PrefabUtility.SaveAsPrefabAsset(l2Instance, prefab02Path);
                Object.DestroyImmediate(l2Instance);
            }

            // 3. Link LevelData in World 1
            string data01WorldPath = $"{world01Dir}/Level_01.asset";
            LevelData data01 = AssetDatabase.LoadAssetAtPath<LevelData>(data01WorldPath);
            if (data01 != null && level1Prefab != null)
            {
                data01.MapPrefab = level1Prefab;
                EditorUtility.SetDirty(data01);
            }

            string data02WorldPath = $"{world01Dir}/Level_02.asset";
            LevelData data02 = AssetDatabase.LoadAssetAtPath<LevelData>(data02WorldPath);
            if (data02 != null && level2Prefab != null)
            {
                data02.MapPrefab = level2Prefab;
                EditorUtility.SetDirty(data02);
            }

            // 4. Auto-wire scene references
            AutoWireSceneReferences.AutoWireAll();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            Debug.Log("🎉 [ResetAndRebuildGridsEditor] Synced settings without modifying user-painted tilemaps!");
            EditorUtility.DisplayDialog("Stray Swarm", "Successfully synced all level settings, playlists, and prefabs!\n\nYour painted tilemaps remain 100% intact and untouched.", "Awesome!");
        }
    }
}
#endif
