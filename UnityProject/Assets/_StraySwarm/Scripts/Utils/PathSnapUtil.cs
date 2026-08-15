using UnityEngine;
using UnityEngine.Tilemaps;

namespace StraySwarm.Utils
{
    /// <summary>
    /// Master utility for computing the exact visual center of any Tilemap path tile,
    /// dynamically adapting to any Tilemap tileAnchor (0 vs 0.5) and cell layout.
    /// </summary>
    public static class PathSnapUtil
    {
        public static Vector3 GetTileVisualCenter(Vector3 worldPos, Component context = null)
        {
            Tilemap tilemap = null;
            if (context != null)
            {
                tilemap = context.GetComponentInParent<Tilemap>();
            }
            if (tilemap == null)
            {
                tilemap = Object.FindAnyObjectByType<Tilemap>();
            }

            if (tilemap != null)
            {
                Vector3 anchor = tilemap.tileAnchor;
                Vector3 testPos = worldPos + new Vector3(0.5f, 0.5f, 0f) - anchor;
                Vector3Int cell = tilemap.WorldToCell(testPos);
                Vector3 cellOrigin = tilemap.CellToWorld(cell);
                return new Vector3(cellOrigin.x + anchor.x, cellOrigin.y + anchor.y, worldPos.z);
            }

            Grid grid = Object.FindAnyObjectByType<Grid>();
            if (grid != null)
            {
                Vector3Int cell = grid.WorldToCell(worldPos);
                Vector3 c = grid.CellToWorld(cell);
                return new Vector3(c.x, c.y, worldPos.z);
            }

            return new Vector3(Mathf.Round(worldPos.x), Mathf.Round(worldPos.y), worldPos.z);
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
