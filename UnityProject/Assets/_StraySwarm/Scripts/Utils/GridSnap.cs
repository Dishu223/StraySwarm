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
            Tilemap tilemap = Object.FindAnyObjectByType<Tilemap>();
            if (tilemap != null)
            {
                Vector3Int cell = tilemap.WorldToCell(transform.position);
                Vector3 center = tilemap.GetCellCenterWorld(cell);
                transform.position = new Vector3(center.x, center.y, transform.position.z);
            }
            else
            {
                Grid grid = Object.FindAnyObjectByType<Grid>();
                if (grid != null)
                {
                    Vector3Int cell = grid.WorldToCell(transform.position);
                    Vector3 center = grid.GetCellCenterWorld(cell);
                    transform.position = new Vector3(center.x, center.y, transform.position.z);
                }
            }
        }
    }
}
