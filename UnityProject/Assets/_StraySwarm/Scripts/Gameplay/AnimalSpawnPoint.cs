using UnityEngine;

namespace StraySwarm.Gameplay
{
    /// <summary>
    /// Placed on tiles where animals can spawn.
    /// Auto-snaps to tile center in Edit Mode and renders a helpful gizmo in Scene View.
    /// </summary>
    [ExecuteInEditMode]
    public class AnimalSpawnPoint : MonoBehaviour
    {
        public bool IsOccupied { get; set; } = false;
        public FollowerBehavior CurrentAnimal { get; set; } = null;

        private void Update()
        {
            // Auto-snap to exact integer tile center in Editor mode
            if (!Application.isPlaying)
            {
                SnapToGridCenter();
            }
        }

        public void SnapToGridCenter()
        {
            Vector3 pos = transform.position;
            float snappedX = Mathf.Round(pos.x);
            float snappedY = Mathf.Round(pos.y);
            if (!Mathf.Approximately(pos.x, snappedX) || !Mathf.Approximately(pos.y, snappedY))
            {
                transform.position = new Vector3(snappedX, snappedY, 0f);
            }
        }

        private void OnDrawGizmos()
        {
            // Draw a cute teal spawn marker in Scene View
            Gizmos.color = IsOccupied ? new Color(1f, 0.4f, 0.4f, 0.6f) : new Color(0.2f, 0.8f, 0.8f, 0.6f);
            Gizmos.DrawWireSphere(transform.position, 0.35f);
            Gizmos.color = new Color(0.2f, 0.8f, 0.8f, 0.25f);
            Gizmos.DrawSphere(transform.position, 0.25f);
        }
    }
}
