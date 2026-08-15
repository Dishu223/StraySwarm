using UnityEngine;
using UnityEngine.Tilemaps;

namespace StraySwarm.Utils
{
    /// <summary>
    /// Ensures GameObjects (Player, Obstacles, Stations, SpawnPoints) stay snapped
    /// to the exact center of Tilemap path cells in Edit Mode.
    /// </summary>
    [ExecuteAlways]
    public class GridSnap : MonoBehaviour
    {
        public bool SnapOnUpdate = true;

        private void Update()
        {
            if (!Application.isPlaying && SnapOnUpdate && transform.hasChanged)
            {
                SnapToPathCenter();
                transform.hasChanged = false;
            }
        }

        [ContextMenu("Snap to Path Center")]
        public void SnapToPathCenter()
        {
            Tilemap tilemap = GetComponentInParent<Tilemap>() ?? Object.FindAnyObjectByType<Tilemap>();
            if (tilemap != null)
            {
                Vector3Int cell = tilemap.WorldToCell(transform.position);
                Vector3 target = tilemap.GetCellCenterWorld(cell);
                if ((transform.position - target).sqrMagnitude > 0.0001f)
                {
                    transform.position = new Vector3(target.x, target.y, transform.position.z);
                }
            }
            else
            {
                Grid grid = Object.FindAnyObjectByType<Grid>();
                if (grid != null)
                {
                    Vector3Int cell = grid.WorldToCell(transform.position);
                    Vector3 target = grid.GetCellCenterWorld(cell);
                    if ((transform.position - target).sqrMagnitude > 0.0001f)
                    {
                        transform.position = new Vector3(target.x, target.y, transform.position.z);
                    }
                }
                else
                {
                    float cx = Mathf.Round(transform.position.x);
                    float cy = Mathf.Round(transform.position.y);
                    transform.position = new Vector3(cx, cy, transform.position.z);
                }
            }
        }
    }
}
