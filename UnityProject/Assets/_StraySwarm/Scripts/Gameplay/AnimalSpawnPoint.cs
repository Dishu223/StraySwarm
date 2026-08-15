using UnityEngine;
using UnityEngine.Tilemaps;

namespace StraySwarm.Gameplay
{
    /// <summary>
    /// Placed on tiles where animals can spawn.
    /// Snaps directly to the visual center of the Tilemap path in Edit Mode.
    /// </summary>
    public class AnimalSpawnPoint : MonoBehaviour
    {
        public bool IsOccupied { get; set; } = false;
        public FollowerBehavior CurrentAnimal { get; set; } = null;

        [ContextMenu("Snap to Tile Center")]
        public void SnapToTileCenter()
        {
            float cx = Mathf.Floor(transform.position.x) + 0.5f;
            float cy = Mathf.Floor(transform.position.y) + 0.5f;
            transform.position = new Vector3(cx, cy, 0f);
        }

        private void OnDrawGizmos()
        {
            Color baseCol = IsOccupied ? new Color(1f, 0.4f, 0.4f, 0.85f) : new Color(0.1f, 0.85f, 0.95f, 0.85f);
            
            // 1. Semi-transparent filled center
            Gizmos.color = new Color(baseCol.r, baseCol.g, baseCol.b, 0.35f);
            Gizmos.DrawCube(transform.position, new Vector3(0.75f, 0.75f, 0.05f));

            // 2. High-contrast wire outline
            Gizmos.color = baseCol;
            Gizmos.DrawWireCube(transform.position, new Vector3(0.8f, 0.8f, 0.05f));

            // 3. Central crosshair target
            Gizmos.DrawLine(transform.position + Vector3.left * 0.2f, transform.position + Vector3.right * 0.2f);
            Gizmos.DrawLine(transform.position + Vector3.up * 0.2f, transform.position + Vector3.down * 0.2f);

#if UNITY_EDITOR
            // 4. Cute paw icon / label
            UnityEditor.Handles.color = baseCol;
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.fontSize = 14;
            labelStyle.normal.textColor = Color.white;
            labelStyle.alignment = TextAnchor.MiddleCenter;
            UnityEditor.Handles.Label(transform.position, "🐾", labelStyle);
#endif
        }
    }
}
