#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using StraySwarm.Gameplay;
using StraySwarm.Core;

namespace StraySwarm.Editor
{
    /// <summary>
    /// Editor utility to snap all GameObjects in the scene to exact grid tile centers.
    /// Menu: Stray Swarm -> 🧲 Snap All Scene Objects to Grid Center
    /// </summary>
    public static class SnapToGridEditor
    {
        [MenuItem("Stray Swarm/🧲 Snap All Scene Objects to Grid Center")]
        public static void SnapAllObjects()
        {
            int snappedCount = 0;

            // 1. Snap SpawnPoints
            var spawnPoints = Object.FindObjectsByType<AnimalSpawnPoint>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var sp in spawnPoints)
            {
                Undo.RecordObject(sp.transform, "Snap to Grid");
                sp.SnapToGridCenter();
                snappedCount++;
            }

            // 2. Snap Obstacles (NumberedWalls, OneWayArrows)
            var walls = Object.FindObjectsByType<NumberedWall>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var wall in walls)
            {
                Undo.RecordObject(wall.transform, "Snap to Grid");
                Vector3 p = wall.transform.position;
                wall.transform.position = new Vector3(Mathf.Round(p.x), Mathf.Round(p.y), p.z);
                snappedCount++;
            }

            var arrows = Object.FindObjectsByType<OneWayArrow>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var arrow in arrows)
            {
                Undo.RecordObject(arrow.transform, "Snap to Grid");
                Vector3 p = arrow.transform.position;
                arrow.transform.position = new Vector3(Mathf.Round(p.x), Mathf.Round(p.y), p.z);
                snappedCount++;
            }

            // 3. Snap Stations
            var stations = Object.FindObjectsByType<RescueStation>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var station in stations)
            {
                Undo.RecordObject(station.transform, "Snap to Grid");
                Vector3 p = station.transform.position;
                station.transform.position = new Vector3(Mathf.Round(p.x), Mathf.Round(p.y), p.z);
                snappedCount++;
            }

            // 4. Snap Player
            var player = GameObject.Find("Player");
            if (player != null)
            {
                Undo.RecordObject(player.transform, "Snap to Grid");
                Vector3 p = player.transform.position;
                player.transform.position = new Vector3(Mathf.Round(p.x), Mathf.Round(p.y), p.z);
                snappedCount++;
            }

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            Debug.Log($"🧲 [SnapToGridEditor] Snapped {snappedCount} objects to exact tile centers!");
            EditorUtility.DisplayDialog("Stray Swarm", $"Successfully snapped {snappedCount} objects to the exact integer center of the path tiles!", "Great!");
        }
    }
}
#endif
