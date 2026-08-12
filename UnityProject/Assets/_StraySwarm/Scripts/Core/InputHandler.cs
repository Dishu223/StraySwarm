using UnityEngine;
using UnityEngine.InputSystem;

namespace StraySwarm.Core
{
    /// <summary>
    /// Detects mouse clicks (in editor) and screen touches (on mobile) to calculate swipes.
    /// Uses Unity 6's New Input System.
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        [Header("Swipe Settings")]
        [Tooltip("How far (in pixels) you need to drag to count as a swipe.")]
        [SerializeField] private float _swipeThreshold = 50f;
        
        [Tooltip("How long (in seconds) the game remembers a swipe before you reach an intersection.")]
        [SerializeField] private float _inputBufferTime = 0.5f;

        private Vector2 _startPosition;
        private bool _isSwiping;
        
        private Vector2Int _bufferedInput = Vector2Int.zero;
        private float _bufferTimer = 0f;

        private void Update()
        {
            // 1. Handle our input buffer timer
            if (_bufferTimer > 0)
            {
                _bufferTimer -= Time.deltaTime;
                if (_bufferTimer <= 0)
                {
                    _bufferedInput = Vector2Int.zero; // Forget the swipe if too much time passed
                }
            }

            // 2. Safely get the mouse or touchscreen pointer
            if (Pointer.current == null) return;
            var pointer = Pointer.current;

            // 3. Check for swipe start (mouse click down or finger touch screen)
            if (pointer.press.wasPressedThisFrame)
            {
                _startPosition = pointer.position.ReadValue();
                _isSwiping = true;
            }
            // 4. Check for swipe end (mouse release or finger lift)
            else if (pointer.press.wasReleasedThisFrame && _isSwiping)
            {
                _isSwiping = false;
                Vector2 endPosition = pointer.position.ReadValue();
                DetectSwipe(_startPosition, endPosition);
            }
        }

        private void DetectSwipe(Vector2 start, Vector2 end)
        {
            Vector2 delta = end - start;
            
            // Did we drag far enough to count as a swipe?
            if (delta.magnitude >= _swipeThreshold)
            {
                Vector2Int direction = Vector2Int.zero;

                // Is the swipe more horizontal or more vertical?
                if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                {
                    direction = delta.x > 0 ? Vector2Int.right : Vector2Int.left;
                }
                else
                {
                    direction = delta.y > 0 ? Vector2Int.up : Vector2Int.down;
                }

                // Buffer the input so the player doesn't have to swipe at the EXACT perfect millisecond
                _bufferedInput = direction;
                _bufferTimer = _inputBufferTime;
                
                Debug.Log($"[InputHandler] Swipe detected: {direction}");
            }
        }

        /// <summary>
        /// The player controller calls this to check if we recently swiped.
        /// </summary>
        public Vector2Int GetBufferedInput()
        {
            return _bufferedInput;
        }

        /// <summary>
        /// Call this once the player actually moves, so we don't move twice from one swipe.
        /// </summary>
        public void ConsumeInput()
        {
            _bufferedInput = Vector2Int.zero;
            _bufferTimer = 0f;
        }
    }
}
