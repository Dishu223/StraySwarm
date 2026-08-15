using UnityEngine;

namespace StraySwarm.Utils
{
    /// <summary>
    /// Master utility for computing the exact visual center of any Tilemap path tile.
    /// All levels strictly use standard Tile Anchor (0.5, 0.5) so (Floor(x) + 0.5, Floor(y) + 0.5)
    /// is 100% unified and rock-solid across all scenes, prefabs, and stages.
    /// </summary>
    public static class PathSnapUtil
    {
        public static Vector3 GetTileVisualCenter(Vector3 worldPos, Component context = null)
        {
            float cx = Mathf.Floor(worldPos.x) + 0.5f;
            float cy = Mathf.Floor(worldPos.y) + 0.5f;
            return new Vector3(cx, cy, worldPos.z);
        }

        public static void SnapTransform(Transform t, Component context = null)
        {
            if (t == null) return;
            Vector3 target = GetTileVisualCenter(t.position, context ?? t);
            if ((t.position - target).sqrMagnitude > 0.0001f)
            {
                t.position = target;
            }
        }
    }
}
