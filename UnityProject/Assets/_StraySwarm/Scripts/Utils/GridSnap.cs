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
            float cx = Mathf.Floor(transform.position.x) + 0.5f;
            float cy = Mathf.Floor(transform.position.y) + 0.5f;
            Vector3 target = new Vector3(cx, cy, transform.position.z);
            if ((transform.position - target).sqrMagnitude > 0.0001f)
            {
                transform.position = target;
            }
        }
    }
}
