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
            if (other.CompareTag("Player"))
            {
                PlayerController player = other.GetComponent<PlayerController>();
                if (player != null)
                {
                    // Force the player to continue in this allowed direction
                    Debug.Log($"[OneWayArrow] Guiding player in direction {_direction}");
                }
            }
        }
    }
}
