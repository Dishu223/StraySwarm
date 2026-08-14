using UnityEngine;

namespace StraySwarm.Utils
{
    /// <summary>
    /// Ensures GameObjects (Player, Obstacles, Stations, SpawnPoints) stay snapped
    /// to the exact center of grid tiles (integer coordinates) in Edit Mode.
    /// </summary>
    [ExecuteInEditMode]
    public class GridSnap : MonoBehaviour
    {
        public bool SnapOnUpdate = true;

        private void Update()
        {
            if (!Application.isPlaying && SnapOnUpdate)
            {
                Snap();
            }
        }

        public void Snap()
        {
            Vector3 pos = transform.position;
            float snappedX = Mathf.Round(pos.x);
            float snappedY = Mathf.Round(pos.y);
            if (!Mathf.Approximately(pos.x, snappedX) || !Mathf.Approximately(pos.y, snappedY))
            {
                transform.position = new Vector3(snappedX, snappedY, pos.z);
            }
        }
    }
}
