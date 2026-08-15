using UnityEngine;
using UnityEngine.Tilemaps;

namespace StraySwarm.Utils
{
    /// <summary>
    /// Master utility for computing the exact visual center of any Tilemap path tile,
    /// dynamically adapting to any Tilemap tileAnchor (0 vs 0.5) in Scene and Prefab stages.
    /// </summary>
    public static class PathSnapUtil
    {
        public static Tilemap FindRelevantTilemap(Component context)
        {
            if (context == null) return Object.FindAnyObjectByType<Tilemap>();

            // 1. Direct parent/ancestor
            Tilemap tilemap = context.GetComponentInParent<Tilemap>();
            if (tilemap != null) return tilemap;

            // 2. Sibling under common root (e.g. Level_XX_Map root containing Grid and AnimalSpawnPoints)
            if (context.transform.root != null)
            {
                tilemap = context.transform.root.GetComponentInChildren<Tilemap>(true);
                if (tilemap != null) return tilemap;
            }

            // 3. Within same GameObject scene (works for PrefabStage preview scene vs Main Scene)
            if (context.gameObject.scene.IsValid())
            {
                var roots = context.gameObject.scene.GetRootGameObjects();
                foreach (var root in roots)
                {
                    tilemap = root.GetComponentInChildren<Tilemap>(true);
                    if (tilemap != null) return tilemap;
                }
            }

            // 4. Fallback global search
            return Object.FindAnyObjectByType<Tilemap>();
        }

        public static Vector3 GetTileVisualCenter(Vector3 worldPos, Component context = null)
        {
            Tilemap tilemap = FindRelevantTilemap(context);

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
