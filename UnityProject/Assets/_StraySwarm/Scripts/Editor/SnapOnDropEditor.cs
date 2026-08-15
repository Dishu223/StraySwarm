#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using StraySwarm.Gameplay;
using StraySwarm.Utils;

namespace StraySwarm.Editor
{
    /// <summary>
    /// Smoothly snaps level objects (SpawnPoints, Walls, Arrows, Stations) to exact path tile centers
    /// the moment the mouse is released after dragging, eliminating any collider ghosting or gizmo fighting.
    /// </summary>
    [InitializeOnLoad]
    public static class SnapOnDropEditor
    {
        static SnapOnDropEditor()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            if (Application.isPlaying) return;

            Event e = Event.current;
            if (e != null && (e.type == EventType.MouseUp || e.rawType == EventType.MouseUp) && e.button == 0)
            {
                if (Selection.gameObjects == null || Selection.gameObjects.Length == 0) return;

                foreach (GameObject go in Selection.gameObjects)
                {
                    if (go == null) continue;

                    var sp = go.GetComponent<AnimalSpawnPoint>();
                    if (sp != null)
                    {
                        Undo.RecordObject(go.transform, "Snap SpawnPoint to Tile Center");
                        sp.SnapToTileCenter();
                        continue;
                    }

                    var wall = go.GetComponent<NumberedWall>();
                    if (wall != null)
                    {
                        Undo.RecordObject(go.transform, "Snap NumberedWall to Tile Center");
                        wall.SnapToTileCenter();
                        continue;
                    }

                    var arrow = go.GetComponent<OneWayArrow>();
                    if (arrow != null)
                    {
                        Undo.RecordObject(go.transform, "Snap OneWayArrow to Tile Center");
                        arrow.SnapToTileCenter();
                        continue;
                    }

                    var station = go.GetComponent<RescueStation>();
                    if (station != null)
                    {
                        Undo.RecordObject(go.transform, "Snap RescueStation to Tile Center");
                        station.SnapToTileCenter();
                        continue;
                    }

                    var gridSnap = go.GetComponent<GridSnap>();
                    if (gridSnap != null)
                    {
                        Undo.RecordObject(go.transform, "Snap to Path Center");
                        gridSnap.SnapToPathCenter();
                        continue;
                    }
                }
            }
        }
    }
}
#endif
