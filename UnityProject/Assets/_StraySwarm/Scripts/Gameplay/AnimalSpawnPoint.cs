using UnityEngine;
using UnityEngine.Tilemaps;

namespace StraySwarm.Gameplay
{
    /// <summary>
    /// Placed on tiles where animals can spawn.
    /// Snaps directly to the visual center of the Tilemap path in Edit Mode.
    /// </summary>
    [ExecuteInEditMode]
    public class AnimalSpawnPoint : MonoBehaviour
    {
        public bool IsOccupied { get; set; } = false;
        public FollowerBehavior CurrentAnimal { get; set; } = null;

        private void Update()
        {
            if (!Application.isPlaying)
            {
                SnapToTileCenter();
            }
        }

        public void SnapToTileCenter()
        {
            Tilemap tilemap = GetComponentInParent<Tilemap>() ?? Object.FindAnyObjectByType<Tilemap>();
            if (tilemap != null && tilemap.tileAnchor.x > 0.1f)
            {
                Vector3Int cell = tilemap.WorldToCell(transform.position);
                Vector3 center = tilemap.GetCellCenterWorld(cell);
                transform.position = new Vector3(center.x, center.y, 0f);
            }
            else
            {
                // Standard cell center: (Floor(x) + 0.5, Floor(y) + 0.5)
                float cx = Mathf.Floor(transform.position.x) + 0.5f;
                float cy = Mathf.Floor(transform.position.y) + 0.5f;
                transform.position = new Vector3(cx, cy, 0f);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = IsOccupied ? new Color(1f, 0.4f, 0.4f, 0.8f) : new Color(0.2f, 0.9f, 0.8f, 0.8f);
            Gizmos.DrawWireSphere(transform.position, 0.4f);
            Gizmos.color = new Color(0.2f, 0.9f, 0.8f, 0.35f);
            Gizmos.DrawSphere(transform.position, 0.3f);
        }
    }
}
