using System.Collections.Generic;
using UnityEngine;

namespace StraySwarm.Gameplay
{
    public struct PathPoint
    {
        public Vector3 Position;
        public float TotalDistance;
    }

    /// <summary>
    /// Records the player's movement path like a breadcrumb trail so followers can walk exactly the same path.
    /// </summary>
    public class PathHistory : MonoBehaviour
    {
        [Tooltip("How often to drop a breadcrumb. Smaller = smoother tail, but more memory.")]
        [SerializeField] private float _recordInterval = 0.1f;
        
        // A linked list is efficient for adding at the head and removing from the tail
        private LinkedList<PathPoint> _pathPoints = new LinkedList<PathPoint>();
        
        private float _totalDistanceTraveled = 0f;
        private Vector3 _lastRecordPosition;

        private void Start()
        {
            _lastRecordPosition = transform.position;
            RecordPoint();
        }

        private void Update()
        {
            float distanceSinceLastRecord = Vector3.Distance(transform.position, _lastRecordPosition);
            
            if (distanceSinceLastRecord >= _recordInterval)
            {
                _totalDistanceTraveled += distanceSinceLastRecord;
                _lastRecordPosition = transform.position;
                RecordPoint();
            }
        }

        private void RecordPoint()
        {
            _pathPoints.AddFirst(new PathPoint 
            { 
                Position = _lastRecordPosition, 
                TotalDistance = _totalDistanceTraveled 
            });

            // Keep the memory footprint small. If we have 100 followers at 1 unit spacing, we need 100 units of history.
            // 100 units / 0.1 interval = 1000 points max.
            if (_pathPoints.Count > 2000)
            {
                _pathPoints.RemoveLast();
            }
        }

        /// <summary>
        /// Finds the exact position along the path that is 'distanceBehindHead' units away from the player.
        /// </summary>
        public Vector3 GetPositionAtDistance(float distanceBehindHead)
        {
            if (_pathPoints.Count == 0) return transform.position;

            float targetDistance = _totalDistanceTraveled - distanceBehindHead;

            // If the target is further back than we've traveled, just stay at the start point
            if (targetDistance <= 0f) 
            {
                return _pathPoints.Last.Value.Position;
            }

            LinkedListNode<PathPoint> currentNode = _pathPoints.First;

            while (currentNode.Next != null)
            {
                // We found the segment that contains our target distance
                if (currentNode.Value.TotalDistance >= targetDistance && 
                    currentNode.Next.Value.TotalDistance <= targetDistance)
                {
                    PathPoint pointAhead = currentNode.Value;
                    PathPoint pointBehind = currentNode.Next.Value;

                    // Lerp precisely between the two breadcrumbs for buttery smooth movement
                    float segmentLength = pointAhead.TotalDistance - pointBehind.TotalDistance;
                    if (segmentLength <= 0.0001f) return pointAhead.Position;

                    float t = (targetDistance - pointBehind.TotalDistance) / segmentLength;
                    return Vector3.Lerp(pointBehind.Position, pointAhead.Position, t);
                }
                currentNode = currentNode.Next;
            }

            // Fallback to the oldest point
            return _pathPoints.Last.Value.Position;
        }
    }
}
