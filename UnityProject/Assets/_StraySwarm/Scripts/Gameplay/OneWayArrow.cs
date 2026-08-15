using UnityEngine;
using StraySwarm.Core;

namespace StraySwarm.Gameplay
{
    public enum ArrowDirection
    {
        Up,
        Right,
        Down,
        Left
    }

    /// <summary>
    /// One-Way Arrow tile on the path.
    /// Forces the player in a specific direction and prevents reversing against the arrow!
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Collider2D))]
    public class OneWayArrow : MonoBehaviour
    {
        [Header("Direction Configuration")]
        [SerializeField] private ArrowDirection _direction = ArrowDirection.Right;
        [SerializeField] private SpriteRenderer _arrowRenderer;

        public Vector2Int AllowedDirectionVector
        {
            get
            {
                return _direction switch
                {
                    ArrowDirection.Up => Vector2Int.up,
                    ArrowDirection.Right => Vector2Int.right,
                    ArrowDirection.Down => Vector2Int.down,
                    ArrowDirection.Left => Vector2Int.left,
                    _ => Vector2Int.right
                };
            }
        }

        private void Update()
        {
            if (!Application.isPlaying && transform.hasChanged)
            {
                SnapToTileCenter();
                transform.hasChanged = false;
            }
        }

        [ContextMenu("Snap to Tile Center")]
        public void SnapToTileCenter()
        {
            var tilemap = GetComponentInParent<UnityEngine.Tilemaps.Tilemap>() ?? Object.FindAnyObjectByType<UnityEngine.Tilemaps.Tilemap>();
            if (tilemap != null)
            {
                Vector3Int cell = tilemap.WorldToCell(transform.position);
                Vector3 target = tilemap.GetCellCenterWorld(cell);
                if ((transform.position - target).sqrMagnitude > 0.0001f)
                {
                    transform.position = new Vector3(target.x, target.y, transform.position.z);
                }
            }
        }

        private void OnValidate()
        {
            UpdateVisualRotation();
        }

        private void Start()
        {
            UpdateVisualRotation();
        }

        public void SetDirection(ArrowDirection dir)
        {
            _direction = dir;
            UpdateVisualRotation();
        }

        private void UpdateVisualRotation()
        {
            float angle = _direction switch
            {
                ArrowDirection.Up => 90f,
                ArrowDirection.Right => 0f,
                ArrowDirection.Down => 270f,
                ArrowDirection.Left => 180f,
                _ => 0f
            };

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") || other.GetComponent<PlayerController>() != null)
            {
                PlayerController player = other.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.ForceDirection(AllowedDirectionVector);
                    Debug.Log($"[OneWayArrow] Guiding player in direction {_direction}");
                }
            }
        }
    }
}
