#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEditor;
using StraySwarm.Gameplay;

namespace StraySwarm.Editor
{
    /// <summary>
    /// Editor utility to generate the AnimalSpawnPoint marker prefab for creators to drop onto the grid.
    /// Menu: Stray Swarm -> 📍 Create Animal Spawn Point Prefab
    /// </summary>
    public static class CreateSpawnPointPrefab
    {
        [MenuItem("Stray Swarm/📍 Create Animal Spawn Point Prefab")]
        public static void GenerateSpawnPointPrefab()
        {
            string folderPath = "Assets/_StraySwarm/Prefabs/Environment";
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            string prefabPath = $"{folderPath}/AnimalSpawnPoint.prefab";

            GameObject go = new GameObject("AnimalSpawnPoint");
            go.AddComponent<AnimalSpawnPoint>();

            PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
            Object.DestroyImmediate(go);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"📍 [CreateSpawnPointPrefab] Successfully created {prefabPath}!");
        }
    }
}
#endif
