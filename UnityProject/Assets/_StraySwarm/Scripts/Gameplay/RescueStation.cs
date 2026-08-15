using UnityEngine;

namespace StraySwarm.Gameplay
{
    /// <summary>
    /// The delivery zone where the player drops off collected animals.
    /// Supports both stationary DeliveryCrates (multi-station) and legacy VanQueue.
    /// </summary>
    public class RescueStation : MonoBehaviour
    {
        [Header("Van Parking Spot")]
        [Tooltip("Where should the vans park?")]
        public Transform VanParkingSpot;

        [Header("Delivery Crate (Multi-Station)")]
        [SerializeField] private DeliveryCrate _attachedCrate;

        [Header("Legacy Driving Van Queue")]
        [SerializeField] private VanQueue _vanQueue;

        [Header("Dependencies")]
        [SerializeField] private TailManager _tailManager;

        [ContextMenu("Snap to Tile Center")]
        public void SnapToTileCenter()
        {
            StraySwarm.Utils.PathSnapUtil.SnapTransform(transform, this);
        }

        private void Start()
        {
            if (!Application.isPlaying) return;
            if (_tailManager == null) _tailManager = FindAnyObjectByType<TailManager>();
            if (_attachedCrate == null) _attachedCrate = GetComponent<DeliveryCrate>();
            if (_vanQueue == null) _vanQueue = FindAnyObjectByType<VanQueue>();
        }

        private bool _isPlayerInside = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _isPlayerInside = true;
                AttemptDelivery();
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _isPlayerInside = true;
                AttemptDelivery();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _isPlayerInside = false;
            }
        }

        public void AttemptDelivery()
        {
            if (!_isPlayerInside || _tailManager == null) return;

            // 1. Try attached Delivery Crate (Multi-Station System)
            if (_attachedCrate != null && !_attachedCrate.IsFull)
            {
                _tailManager.DeliverToCrate(_attachedCrate);
                return;
            }

            // 2. Fallback to Van Queue (Single Driving Van)
            if (_vanQueue != null)
            {
                VanController currentVan = _vanQueue.GetCurrentVan();
                if (currentVan != null && !currentVan.IsFull && !currentVan.IsDrivingAway && currentVan.IsParked)
                {
                    _tailManager.DeliverToVan(currentVan);
                }
            }
        }
    }
}
