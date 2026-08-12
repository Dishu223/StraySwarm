using UnityEngine;

namespace StraySwarm.Core
{
    /// <summary>
    /// Controls the player's movement along the grid using input from the InputHandler.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Tooltip("How fast the player moves from node to node.")]
        [SerializeField] private float _moveSpeed = 5f;
        
        [Header("Dependencies")]
        [SerializeField] private GridManager _gridManager;
        [SerializeField] private InputHandler _inputHandler;

        private NodeData _currentNode;
        private NodeData _targetNode;
        private Vector2Int _currentDirection = Vector2Int.zero;
        private bool _isMoving = false;

        [Header("Animation")]
        [SerializeField] private Animator _animator;
        [SerializeField] private Gameplay.CubeWobble _cubeWobble;

        private void Start()
        {
            // If we didn't drag these into the inspector, try to find them automatically!
            if (_gridManager == null) _gridManager = FindAnyObjectByType<GridManager>();
            if (_inputHandler == null) _inputHandler = FindAnyObjectByType<InputHandler>();
            if (_animator == null) _animator = GetComponent<Animator>();
            if (_cubeWobble == null) _cubeWobble = GetComponent<Gameplay.CubeWobble>();

            // Snap the player to the CLOSEST grid node to where you dragged them in the Scene!
            _currentNode = _gridManager.GetClosestNode(transform.position);
            if (_currentNode != null)
            {
                transform.position = _currentNode.WorldPosition;
            }
        }

        private void Update()
        {
            if (_currentNode == null) return;

            // 1. If we are moving, smoothly slide towards the target node
            if (_isMoving)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position, 
                    _targetNode.WorldPosition, 
                    _moveSpeed * Time.deltaTime
                );

                // Have we reached the target node?
                if (Vector3.Distance(transform.position, _targetNode.WorldPosition) < 0.01f)
                {
                    // Snap exactly to the position to avoid floating point errors
                    transform.position = _targetNode.WorldPosition;
                    _currentNode = _targetNode;
                    _isMoving = false;
                }
            }

            // 2. If we are NOT moving (or just arrived at a node), check for new input!
            if (!_isMoving)
            {
                Vector2Int desiredInput = _inputHandler.GetBufferedInput();

                // 2a. First, try to move in the direction the player swiped
                if (desiredInput != Vector2Int.zero)
                {
                    // Is the swiped direction a valid path?
                    if (_currentNode.ValidDirections.Contains(desiredInput))
                    {
                        // We removed the 180-degree restriction! You can now swipe backwards.
                        StartMovingToNode(_currentNode.GridPosition + desiredInput, desiredInput);
                        _inputHandler.ConsumeInput(); // We used the swipe, clear it!
                    }
                }
                
                // 2b. If we didn't move from a swipe, handle automatic continuous movement
                if (!_isMoving && _currentDirection != Vector2Int.zero)
                {
                    // Can we keep going straight?
                    if (_currentNode.ValidDirections.Contains(_currentDirection))
                    {
                        StartMovingToNode(_currentNode.GridPosition + _currentDirection, _currentDirection);
                    }
                    else
                    {
                        // We hit a wall straight ahead. Is this a corner we should automatically turn?
                        // A corner means there's exactly ONE valid way to go (other than going backwards)
                        Vector2Int backwards = -_currentDirection;
                        Vector2Int autoTurnDirection = Vector2Int.zero;
                        int availableForwardPaths = 0;

                        foreach (Vector2Int dir in _currentNode.ValidDirections)
                        {
                            if (dir != backwards)
                            {
                                availableForwardPaths++;
                                autoTurnDirection = dir;
                            }
                        }

                        // If there is EXACTLY one way to turn, automatically take the corner!
                        if (availableForwardPaths == 1)
                        {
                            StartMovingToNode(_currentNode.GridPosition + autoTurnDirection, autoTurnDirection);
                        }
                        else
                        {
                            // It's a dead end (0 paths) or a multi-way intersection (>1 paths). We stop and wait for a swipe.
                            _currentDirection = Vector2Int.zero;
                        }
                    }
                }
            }
        }

        private void StartMovingToNode(Vector2Int gridPosition, Vector2Int direction)
        {
            NodeData candidateNode = _gridManager.GetNodeAt(gridPosition);
            if (candidateNode == null) return; // Prevent NullReferenceException if swiping out of bounds!

            _targetNode = candidateNode;
            _currentDirection = direction;
            _isMoving = true;

            if (_cubeWobble != null)
            {
                float stepDuration = 1f / Mathf.Max(_moveSpeed, 0.1f);
                _cubeWobble.TriggerHop(stepDuration);
            }
        }
    }
}
