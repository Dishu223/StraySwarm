using UnityEngine;

namespace StraySwarm.Utils
{
    /// <summary>
    /// Ensures GameObjects (Player, Obstacles, Stations, SpawnPoints) stay snapped
    /// to the exact center of Tilemap path cells in Edit Mode.
    /// </summary>
    public class GridSnap : MonoBehaviour
    {
        [ContextMenu("Snap to Path Center")]
        public void SnapToPathCenter()
        {
            PathSnapUtil.SnapTransform(transform, this);
        }
    }
}
