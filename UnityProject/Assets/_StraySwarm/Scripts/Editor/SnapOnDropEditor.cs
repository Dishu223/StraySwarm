#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using StraySwarm.Gameplay;
using StraySwarm.Utils;

namespace StraySwarm.Editor
{
    /// <summary>
    /// Snaps level objects (SpawnPoints, Walls, Arrows, Stations) to the exact visual center
    /// of the nearest path tile the moment the mouse is released after dragging.
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

                    bool shouldSnap = go.GetComponent<AnimalSpawnPoint>() != null ||
                                      go.GetComponent<NumberedWall>() != null ||
                                      go.GetComponent<OneWayArrow>() != null ||
                                      go.GetComponent<RescueStation>() != null ||
                                      go.GetComponent<GridSnap>() != null;

                    if (shouldSnap)
                    {
                        Undo.RecordObject(go.transform, "Snap to Path Tile Center");
                        PathSnapUtil.SnapTransform(go.transform, go.transform);
                    }
                }
            }
        }
    }
}
#endif
