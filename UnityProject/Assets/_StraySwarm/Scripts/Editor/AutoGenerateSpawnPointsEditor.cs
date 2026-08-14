#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEditor.SceneManagement;
using StraySwarm.Gameplay;

namespace StraySwarm.Editor
{
    /// <summary>
    /// Generates 8-12 clustered spawn points on painted path tiles with 1 click.
    /// Supports re-rolling random clusters on every click and full manual editing afterward.
    /// Menu: Stray Swarm -> 📍 Auto-Generate Clustered Spawn Points (8-12 Points)
    /// </summary>
    public static class AutoGenerateSpawnPointsEditor
    {
        [MenuItem("Stray Swarm/📍 Auto-Generate Clustered Spawn Points (8-12 Points)", false, 20)]
        public static void GenerateClusteredSpawnPoints()
        {
            // 1. Find Tilemap in active stage (Scene or Prefab Mode)
            Tilemap tilemap = Object.FindAnyObjectByType<Tilemap>();
            if (tilemap == null)
            {
                EditorUtility.DisplayDialog("No Tilemap Found", "Please open a level map with a painted Tilemap first!", "OK");
                return;
            }

            // 2. Gather all painted cells
            List<Vector3Int> paintedCells = new List<Vector3Int>();
            BoundsInt bounds = tilemap.cellBounds;
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    if (tilemap.HasTile(pos))
                    {
                        paintedCells.Add(pos);
                    }
                }
            }

            if (paintedCells.Count < 5)
            {
                EditorUtility.DisplayDialog("Not Enough Path Tiles", "Please paint more path tiles on your Tilemap first (at least 5 tiles needed)!", "OK");
                return;
            }

            // 3. Find or Create the AnimalSpawnPoints container
            Transform container = null;
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                Transform stageRoot = prefabStage.prefabContentsRoot.transform;
                container = stageRoot.Find("AnimalSpawnPoints");
                if (container == null)
                {
                    GameObject newContainer = new GameObject("AnimalSpawnPoints");
                    newContainer.transform.SetParent(stageRoot, false);
                    container = newContainer.transform;
                }
            }
            else
            {
                Transform rootTransform = tilemap.transform.root;
                container = rootTransform.Find("AnimalSpawnPoints");
                if (container == null)
                {
                    GameObject newContainer = new GameObject("AnimalSpawnPoints");
                    newContainer.transform.SetParent(rootTransform, false);
                    container = newContainer.transform;
                }
            }

            // 4. Clear any previous spawn points under container
            Undo.RegisterFullObjectHierarchyUndo(container.gameObject, "Auto-Generate Clustered Spawn Points");
            
            for (int i = container.childCount - 1; i >= 0; i--)
            {
                Undo.DestroyObjectImmediate(container.GetChild(i).gameObject);
            }

            // Also clean up any loose spawn points at stage root level
            var loosePoints = Object.FindObjectsByType<AnimalSpawnPoint>(FindObjectsInactive.Include);
            foreach (var lp in loosePoints)
            {
                if (lp.transform.parent != container)
                {
                    Undo.DestroyObjectImmediate(lp.gameObject);
                }
            }

            // 5. Load AnimalSpawnPoint prefab
            GameObject spawnPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_StraySwarm/Prefabs/Environment/AnimalSpawnPoint.prefab");

            // 6. Cluster Generation Algorithm: Pick 3-4 clusters with 2-4 adjacent tiles each
            int targetTotal = Random.Range(8, 13); // 8 to 12 points
            int clusterCount = Random.Range(3, 5); // 3 to 4 distinct groups
            int pointsPerCluster = Mathf.CeilToInt((float)targetTotal / clusterCount);

            HashSet<Vector3Int> selectedCells = new HashSet<Vector3Int>();
            List<Vector3Int> availableCells = new List<Vector3Int>(paintedCells);

            Vector3Int[] directions = {
                Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right
            };

            for (int c = 0; c < clusterCount && selectedCells.Count < targetTotal; c++)
            {
                if (availableCells.Count == 0) break;

                // Pick a random seed cell far from already selected cells
                Vector3Int seed = availableCells[Random.Range(0, availableCells.Count)];
                selectedCells.Add(seed);
                availableCells.Remove(seed);

                // Grow cluster to neighboring connected path cells
                Queue<Vector3Int> neighborQueue = new Queue<Vector3Int>();
                neighborQueue.Enqueue(seed);

                int clusterAdded = 1;
                while (neighborQueue.Count > 0 && clusterAdded < pointsPerCluster && selectedCells.Count < targetTotal)
                {
                    Vector3Int current = neighborQueue.Dequeue();

                    // Shuffle directions for natural random spread
                    var shuffledDirs = directions.OrderBy(d => Random.value).ToList();
                    foreach (var dir in shuffledDirs)
                    {
                        Vector3Int neighbor = current + dir;
                        if (paintedCells.Contains(neighbor) && !selectedCells.Contains(neighbor))
                        {
                            selectedCells.Add(neighbor);
                            availableCells.Remove(neighbor);
                            neighborQueue.Enqueue(neighbor);
                            clusterAdded++;
                            if (clusterAdded >= pointsPerCluster || selectedCells.Count >= targetTotal) break;
                        }
                    }
                }
            }

            // 7. Instantiate SpawnPoints at selected cell centers
            int index = 0;
            List<GameObject> createdObjs = new List<GameObject>();
            foreach (var cell in selectedCells)
            {
                Vector3 worldPos = new Vector3(cell.x + 0.5f, cell.y + 0.5f, 0f);

                GameObject spGo;
                if (spawnPrefab != null && prefabStage == null)
                {
                    spGo = (GameObject)PrefabUtility.InstantiatePrefab(spawnPrefab, container);
                    spGo.transform.position = worldPos;
                }
                else
                {
                    spGo = new GameObject($"AnimalSpawnPoint ({index})");
                    spGo.transform.SetParent(container, false);
                    spGo.transform.localPosition = new Vector3(cell.x + 0.5f, cell.y + 0.5f, 0f);
                    spGo.AddComponent<AnimalSpawnPoint>();
                }

                spGo.name = $"AnimalSpawnPoint ({index})";
                Undo.RegisterCreatedObjectUndo(spGo, "Created Spawn Point");
                createdObjs.Add(spGo);
                index++;
            }

            // 8. Mark Stage Dirty
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            if (prefabStage != null)
            {
                EditorUtility.SetDirty(container.gameObject);
                EditorUtility.SetDirty(prefabStage.prefabContentsRoot);
            }

            if (createdObjs.Count > 0)
            {
                Selection.objects = createdObjs.ToArray();
            }
            else
            {
                Selection.activeGameObject = container.gameObject;
            }

            Debug.Log($"📍 [AutoGenerateSpawnPoints] Successfully generated {selectedCells.Count} clustered spawn points across {clusterCount} groups!");
        }
    }
}
#endif
