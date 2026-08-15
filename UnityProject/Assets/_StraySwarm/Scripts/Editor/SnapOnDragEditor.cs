#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using StraySwarm.Gameplay;
using StraySwarm.Utils;

namespace StraySwarm.Editor
{
    /// <summary>
    /// Snaps level objects in real-time as they are being dragged in Scene View.
    /// Hooks into SceneView GUI event loop so all gizmos, sprites, and colliders move together in sync.
    /// </summary>
    [InitializeOnLoad]
    public static class SnapOnDragEditor
    {
        static SnapOnDragEditor()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            if (Application.isPlaying) return;

            Event e = Event.current;
            if (e == null) return;

            if (Selection.transforms == null || Selection.transforms.Length == 0) return;

            foreach (Transform t in Selection.transforms)
            {
                if (t == null) continue;

                bool isLevelObj = t.GetComponent<AnimalSpawnPoint>() != null ||
                                  t.GetComponent<NumberedWall>() != null ||
                                  t.GetComponent<OneWayArrow>() != null ||
                                  t.GetComponent<RescueStation>() != null ||
                                  t.GetComponent<GridSnap>() != null;

                if (isLevelObj && t.hasChanged)
                {
                    Vector3 currentPos = t.position;
                    Vector3 snapped = PathSnapUtil.GetTileVisualCenter(currentPos, t);
                    if ((currentPos - snapped).sqrMagnitude > 0.0001f)
                    {
                        Undo.RecordObject(t, "Drag Snap to Tile Center");
                        t.position = snapped;
                    }
                    t.hasChanged = false;
                }
            }
        }
    }
}
#endif
