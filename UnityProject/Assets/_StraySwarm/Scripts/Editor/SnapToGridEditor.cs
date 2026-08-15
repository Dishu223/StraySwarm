#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEditor.SceneManagement;
using StraySwarm.Gameplay;

namespace StraySwarm.Editor
{
    /// <summary>
    /// Editor utility to snap all GameObjects in the scene to exact Tilemap path cell centers.
    /// Menu: Stray Swarm -> 🧲 Snap All Scene Objects to Path Center
    /// </summary>
    public static class SnapToGridEditor
    {
        [MenuItem("Stray Swarm/🧲 Snap All Scene Objects to Path Center")]
        public static void SnapAllObjects()
        {
            Tilemap tilemap = Object.FindAnyObjectByType<Tilemap>();
            Grid grid = Object.FindAnyObjectByType<Grid>();

            if (tilemap == null && grid == null)
            {
                Debug.LogWarning("⚠️ [SnapToGridEditor] No Tilemap or Grid found in the scene.");
                return;
            }

            int snappedCount = 0;

            // Helper lambda to get path center
            Vector3 GetPathCenter(Vector3 worldPos, Component context)
            {
                return StraySwarm.Utils.PathSnapUtil.GetTileVisualCenter(worldPos, context);
            }

            // 1. Snap SpawnPoints
            var spawnPoints = Object.FindObjectsByType<AnimalSpawnPoint>(FindObjectsInactive.Include);
            foreach (var sp in spawnPoints)
            {
                Undo.RecordObject(sp.transform, "Snap to Path Center");
                sp.transform.position = GetPathCenter(sp.transform.position, sp);
                snappedCount++;
            }

            // 2. Snap Obstacles
            var walls = Object.FindObjectsByType<NumberedWall>(FindObjectsInactive.Include);
            foreach (var wall in walls)
            {
                Undo.RecordObject(wall.transform, "Snap to Path Center");
                wall.transform.position = GetPathCenter(wall.transform.position, wall);
                snappedCount++;
            }

            var arrows = Object.FindObjectsByType<OneWayArrow>(FindObjectsInactive.Include);
            foreach (var arrow in arrows)
            {
                Undo.RecordObject(arrow.transform, "Snap to Path Center");
                arrow.transform.position = GetPathCenter(arrow.transform.position, arrow);
                snappedCount++;
            }

            // 3. Snap Stations
            var stations = Object.FindObjectsByType<RescueStation>(FindObjectsInactive.Include);
            foreach (var station in stations)
            {
                Undo.RecordObject(station.transform, "Snap to Path Center");
                station.transform.position = GetPathCenter(station.transform.position, station);
                snappedCount++;
            }

            // 4. Snap Player
            var player = GameObject.Find("Player");
            if (player != null)
            {
                Undo.RecordObject(player.transform, "Snap to Path Center");
                player.transform.position = GetPathCenter(player.transform.position, player.transform);
                snappedCount++;
            }

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            Debug.Log($"🧲 [SnapToGridEditor] Snapped {snappedCount} objects to exact Tilemap path centers!");
            EditorUtility.DisplayDialog("Stray Swarm", $"Successfully snapped {snappedCount} objects to the exact center of the Tilemap path!", "Great!");
        }
    }
}
#endif
