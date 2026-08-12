using System.Collections.Generic;
using UnityEngine;

namespace StraySwarm.Core
{
    /// <summary>
    /// Represents a single intersection or point on the grid that the player can walk on.
    /// </summary>
    public class NodeData
    {
        // The X,Y grid coordinate (e.g., [0,0], [1,0])
        public Vector2Int GridPosition { get; private set; }
        
        // The actual physical position in the Unity world
        public Vector3 WorldPosition { get; private set; }
        
        // Which directions can a character move from this specific node?
        public List<Vector2Int> ValidDirections { get; private set; }

        public NodeData(Vector2Int gridPos, Vector3 worldPos)
        {
            GridPosition = gridPos;
            WorldPosition = worldPos;
            ValidDirections = new List<Vector2Int>();
        }

        /// <summary>
        /// Adds a valid movement direction from this node (e.g., Vector2Int.up)
        /// </summary>
        public void AddDirection(Vector2Int direction)
        {
            if (!ValidDirections.Contains(direction))
            {
                ValidDirections.Add(direction);
            }
        }
    }
}
