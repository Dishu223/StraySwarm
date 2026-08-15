using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using StraySwarm.Utils;

namespace StraySwarm.Gameplay
{
    /// <summary>
    /// Designates the starting tile for the Player Cat on handcrafted levels.
    /// Snaps cleanly to the center of Tilemap paths.
    /// </summary>
    [SelectionBase]
    public class PlayerSpawnPoint : MonoBehaviour
    {
        [ContextMenu("Snap to Tile Center")]
        public void SnapToTileCenter()
        {
            PathSnapUtil.SnapTransform(transform, this);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Color gold = new Color(1f, 0.85f, 0.2f, 0.9f);
            
            // 1. Semi-transparent filled square
            Gizmos.color = new Color(1f, 0.85f, 0.2f, 0.35f);
            Gizmos.DrawCube(transform.position, new Vector3(0.85f, 0.85f, 0.05f));

            // 2. High-contrast wire outline
            Gizmos.color = gold;
            Gizmos.DrawWireCube(transform.position, new Vector3(0.85f, 0.85f, 0.05f));

            // 3. Inner crosshairs
            Gizmos.DrawLine(transform.position - new Vector3(0.2f, 0f, 0f), transform.position + new Vector3(0.2f, 0f, 0f));
            Gizmos.DrawLine(transform.position - new Vector3(0f, 0.2f, 0f), transform.position + new Vector3(0f, 0.2f, 0f));

            // 4. Cat Icon Label
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.fontSize = 13;
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.normal.textColor = Color.yellow;
            Handles.Label(transform.position + new Vector3(0f, 0.45f, 0f), "🐱 START", labelStyle);
        }
#endif
    }
}
