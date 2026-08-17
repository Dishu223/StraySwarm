using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; // Needed to read painted levels

namespace StraySwarm.Core
{
    /// <summary>
    /// Scans the painted Tilemap and builds the invisible graph that characters navigate.
    /// </summary>
    public class GridManager : MonoBehaviour
    {
        // Dictionary for lightning-fast node lookups by their Vector2Int coordinate
        private Dictionary<Vector2Int, NodeData> _gridGraph = new Dictionary<Vector2Int, NodeData>();

        [Header("Level Design")]
        [Tooltip("Drag the Unity Tilemap here that contains your walkable floor tiles!")]
        [SerializeField] private Tilemap _floorTilemap;

        [Header("Fallback Test Settings")]
        [SerializeField] private int _gridWidth = 7;
        [SerializeField] private int _gridHeight = 7;
        [SerializeField] private float _nodeSpacing = 1.5f;

        public static GridManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            RebuildGrid();
        }

        public void RebuildGrid()
        {
            if (_floorTilemap == null)
            {
                _floorTilemap = FindAnyObjectByType<Tilemap>();
            }

            if (_floorTilemap != null)
            {
                GenerateGridFromTilemap();
            }
            else
            {
                Debug.LogWarning("[GridManager] No Floor Tilemap found in scene! Generating fallback square grid.");
                GenerateTestGrid();
            }
        }

        /// <summary>
        /// The professional way: Scans the level you painted in Unity and automatically builds paths!
        /// </summary>
        private void GenerateGridFromTilemap()
        {
            _gridGraph.Clear();

            // 1. Find every single tile you painted
            BoundsInt bounds = _floorTilemap.cellBounds;
            TileBase[] allTiles = _floorTilemap.GetTilesBlock(bounds);

            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    TileBase tile = allTiles[x + y * bounds.size.x];
                    if (tile != null)
                    {
                        // Found a floor tile! Create a node exactly at its center.
                        Vector2Int gridPos = new Vector2Int(bounds.xMin + x, bounds.yMin + y);
                        Vector3 worldPos = _floorTilemap.GetCellCenterWorld((Vector3Int)gridPos);
                        
                        _gridGraph.Add(gridPos, new NodeData(gridPos, worldPos));
                    }
                }
            }

            // 2. Connect the nodes so the cat knows where it can walk
            ConnectNodes();

            // 3. Dynamically center and scale the camera to fit any level size comfortably!
            AutoFrameCamera();

            Debug.Log($"[GridManager] Scanned Tilemap and built graph with {_gridGraph.Count} nodes.");
        }

        /// <summary>
        /// Automatically centers and zooms the Main Camera to frame any level layout with proper HUD padding.
        /// </summary>
        public void AutoFrameCamera()
        {
            Camera cam = Camera.main;
            if (cam == null || _gridGraph.Count == 0) return;

            float minX = float.MaxValue, maxX = float.MinValue;
            float minY = float.MaxValue, maxY = float.MinValue;

            foreach (var kvp in _gridGraph)
            {
                Vector3 pos = kvp.Value.WorldPosition;
                if (pos.x < minX) minX = pos.x;
                if (pos.x > maxX) maxX = pos.x;
                if (pos.y < minY) minY = pos.y;
                if (pos.y > maxY) maxY = pos.y;
            }

            // Include any rescue stations on the map (and van parking spots)
            var stations = Object.FindObjectsByType<Gameplay.RescueStation>(FindObjectsInactive.Exclude);
            foreach (var st in stations)
            {
                Vector3 pos = st.transform.position;
                if (pos.x < minX) minX = pos.x;
                if (pos.x > maxX) maxX = pos.x;
                if (pos.y < minY) minY = pos.y;
                if (pos.y > maxY) maxY = pos.y;

                if (st.VanParkingSpot != null)
                {
                    Vector3 vPos = st.VanParkingSpot.position;
                    if (vPos.x < minX) minX = vPos.x;
                    if (vPos.x > maxX) maxX = vPos.x;
                    if (vPos.y < minY) minY = vPos.y;
                    if (vPos.y > maxY) maxY = vPos.y;
                }
            }

            float centerX = (minX + maxX) * 0.5f;
            float centerY = (minY + maxY) * 0.5f;

            // Extra top headroom for the Top HUD and Timer (+4.2 units height headroom)
            float width = (maxX - minX) + 2.5f;
            float height = (maxY - minY) + 4.2f;

            // Position camera slightly downward so the top HUD has ample breathing room
            cam.transform.position = new Vector3(centerX, centerY - 0.6f, -10f);

            float targetOrtho = Mathf.Max(height * 0.5f, (width / cam.aspect) * 0.5f);
            cam.orthographicSize = Mathf.Clamp(targetOrtho, 6.0f, 20f);
        }

        /// <summary>
        /// Generates a simple square grid for early prototyping (used if no Tilemap is assigned).
        /// </summary>
        private void GenerateTestGrid()
        {
            _gridGraph.Clear();

            // 1. Create all the nodes
            for (int x = 0; x < _gridWidth; x++)
            {
                for (int y = 0; y < _gridHeight; y++)
                {
                    Vector2Int gridPos = new Vector2Int(x, y);
                    // Center the grid around world position (0,0,0)
                    Vector3 worldPos = new Vector3(
                        (x - _gridWidth / 2f) * _nodeSpacing, 
                        (y - _gridHeight / 2f) * _nodeSpacing, 
                        0f);

                    _gridGraph.Add(gridPos, new NodeData(gridPos, worldPos));
                }
            }

            ConnectNodes();
            Debug.Log($"[GridManager] Generated fallback grid with {_gridGraph.Count} nodes.");
        }

        private void ConnectNodes()
        {
            foreach (var kvp in _gridGraph)
            {
                Vector2Int pos = kvp.Key;
                NodeData node = kvp.Value;

                // Check Up
                if (_gridGraph.ContainsKey(pos + Vector2Int.up)) node.AddDirection(Vector2Int.up);
                // Check Down
                if (_gridGraph.ContainsKey(pos + Vector2Int.down)) node.AddDirection(Vector2Int.down);
                // Check Right
                if (_gridGraph.ContainsKey(pos + Vector2Int.right)) node.AddDirection(Vector2Int.right);
                // Check Left
                if (_gridGraph.ContainsKey(pos + Vector2Int.left)) node.AddDirection(Vector2Int.left);
            }

            Debug.Log($"[GridManager] Generated grid with {_gridGraph.Count} nodes.");
        }

        /// <summary>
        /// Returns the NodeData at a specific coordinate, or null if it doesn't exist (like a wall or out of bounds).
        /// </summary>
        public NodeData GetNodeAt(Vector2Int gridPosition)
        {
            if (_gridGraph.TryGetValue(gridPosition, out NodeData node))
            {
                return node;
            }
            return null;
        }

        /// <summary>
        /// Finds the closest valid path node to a given world position.
        /// This lets us drag-and-drop the player anywhere in the editor to start!
        /// </summary>
        public NodeData GetClosestNode(Vector3 worldPosition)
        {
            NodeData closestNode = null;
            float closestDistance = float.MaxValue;

            foreach (var node in _gridGraph.Values)
            {
                float dist = Vector3.Distance(worldPosition, node.WorldPosition);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closestNode = node;
                }
            }

            return closestNode;
        }

        // --- GIZMOS FOR DEBUGGING ---
        // This makes the invisible grid nodes visible in the Unity Editor!
        private void OnDrawGizmos()
        {
            if (_gridGraph == null || _gridGraph.Count == 0) return;

            foreach (var node in _gridGraph.Values)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(node.WorldPosition, 0.15f);

                // Draw lines to valid neighbors
                Gizmos.color = Color.white;
                foreach (var dir in node.ValidDirections)
                {
                    if (_gridGraph.TryGetValue(node.GridPosition + dir, out NodeData neighbor))
                    {
                        Gizmos.DrawLine(node.WorldPosition, neighbor.WorldPosition);
                    }
                }
            }
        }
    }
}
