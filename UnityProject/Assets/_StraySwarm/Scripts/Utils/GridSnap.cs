using UnityEngine;
using UnityEngine.Tilemaps;

namespace StraySwarm.Utils
{
    /// <summary>
    /// Ensures GameObjects (Player, Obstacles, Stations, SpawnPoints) stay snapped
    /// to the exact center of Tilemap path cells in Edit Mode.
    /// </summary>
    [ExecuteInEditMode]
    public class GridSnap : MonoBehaviour
    {
        public bool SnapOnUpdate = true;

        private void Update()
        {
            if (!Application.isPlaying && SnapOnUpdate)
            {
                SnapToPathCenter();
            }
        }

        public void SnapToPathCenter()
        {
            Tilemap tilemap = GetComponentInParent<Tilemap>() ?? Object.FindAnyObjectByType<Tilemap>();
            if (tilemap != null && tilemap.tileAnchor.x > 0.1f)
            {
                Vector3Int cell = tilemap.WorldToCell(transform.position);
                Vector3 center = tilemap.GetCellCenterWorld(cell);
                transform.position = new Vector3(center.x, center.y, transform.position.z);
            }
            else
            {
                // Standard cell center: (Floor(x) + 0.5, Floor(y) + 0.5)
                float cx = Mathf.Floor(transform.position.x) + 0.5f;
                float cy = Mathf.Floor(transform.position.y) + 0.5f;
                transform.position = new Vector3(cx, cy, transform.position.z);
            }
        }
    }
}
