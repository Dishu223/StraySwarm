using System.Collections.Generic;
using UnityEngine;
using StraySwarm.Data;
using StraySwarm.Core;

namespace StraySwarm.Gameplay
{
    /// <summary>
    /// Spawns animals, delivery stations, and puzzle obstacles dynamically from LevelData.
    /// Manages clean level transitions and entity lifetimes.
    /// </summary>
    public class LevelBuilder : MonoBehaviour
    {
        public static LevelBuilder Instance { get; private set; }

        [Header("Animal Prefabs")]
        [SerializeField] private GameObject _puppyPrefab;
        [SerializeField] private GameObject _kittenPrefab;
        [SerializeField] private GameObject _frogPrefab;
        [SerializeField] private GameObject _mousePrefab;
        [SerializeField] private GameObject _pigeonPrefab;
        [SerializeField] private GameObject _bunnyPrefab;

        [Header("Obstacle Prefabs - Arrows")]
        [SerializeField] private GameObject _arrowUpPrefab;
        [SerializeField] private GameObject _arrowRightPrefab;
        [SerializeField] private GameObject _arrowDownPrefab;
        [SerializeField] private GameObject _arrowLeftPrefab;

        [Header("Obstacle Prefabs - Walls")]
        [SerializeField] private GameObject _wall1Prefab;
        [SerializeField] private GameObject _wall2Prefab;
        [SerializeField] private GameObject _wall3Prefab;

        [Header("Scene References")]
        [SerializeField] private Transform _spawnedContainer;
        [SerializeField] private Transform _playerTransform;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            if (_spawnedContainer == null)
            {
                GameObject container = new GameObject("SpawnedLevelEntities");
                _spawnedContainer = container.transform;
            }

            if (_playerTransform == null)
            {
                GameObject player = GameObject.Find("Player");
                if (player != null) _playerTransform = player.transform;
            }
        }

        private void Start()
        {
            LevelData currentLevel = null;
            if (LevelManager.Instance != null)
            {
                currentLevel = LevelManager.Instance.GetCurrentLevelData();
            }

            if (currentLevel != null)
            {
                BuildLevel(currentLevel);
            }
        }

        public void BuildLevel(LevelData data)
        {
            if (data == null) return;

            ClearSpawnedEntities();

            // 1. If LevelData has a handcrafted MapPrefab, instantiate it!
            if (data.MapPrefab != null)
            {
                GameObject mapInstance = Instantiate(data.MapPrefab, Vector3.zero, Quaternion.identity, _spawnedContainer);
                mapInstance.name = $"{data.LevelName}_Map";
            }

            // 2. Position Player
            if (_playerTransform != null && data.PlayerStartCell != Vector2Int.zero)
            {
                _playerTransform.position = new Vector3(data.PlayerStartCell.x, data.PlayerStartCell.y, 0);
            }

            // 3. Spawn One-Way Arrows from data if defined
            if (data.OneWayArrows != null && data.OneWayArrows.Count > 0)
            {
                foreach (var arrow in data.OneWayArrows)
                {
                    GameObject prefab = GetArrowPrefab(arrow.Direction);
                    if (prefab != null)
                    {
                        Vector3 worldPos = new Vector3(arrow.GridPosition.x, arrow.GridPosition.y, 0);
                        Instantiate(prefab, worldPos, Quaternion.identity, _spawnedContainer);
                    }
                }
            }

            // 4. Spawn Numbered Walls from data if defined
            if (data.NumberedWalls != null && data.NumberedWalls.Count > 0)
            {
                foreach (var wall in data.NumberedWalls)
                {
                    GameObject prefab = GetWallPrefab(wall.HitPoints);
                    if (prefab != null)
                    {
                        Vector3 worldPos = new Vector3(wall.GridPosition.x, wall.GridPosition.y, 0);
                        Instantiate(prefab, worldPos, Quaternion.identity, _spawnedContainer);
                    }
                }
            }

            // 5. Initialize Wave Spawner on the active / instantiated map
            if (WaveSpawner.Instance != null)
            {
                WaveSpawner.Instance.InitializeWaveSystem();
            }

            Debug.Log($"🎉 [LevelBuilder] Built Level: {data.LevelName} (MapPrefab: {(data.MapPrefab != null ? data.MapPrefab.name : "Active Scene")})");
        }

        public void ClearSpawnedEntities()
        {
            if (_spawnedContainer != null)
            {
                for (int i = _spawnedContainer.childCount - 1; i >= 0; i--)
                {
                    Destroy(_spawnedContainer.GetChild(i).gameObject);
                }
            }
        }

        private GameObject GetAnimalPrefab(AnimalType type)
        {
            return type switch
            {
                AnimalType.Puppy => _puppyPrefab,
                AnimalType.Kitten => _kittenPrefab,
                AnimalType.Frog => _frogPrefab,
                AnimalType.Mouse => _mousePrefab,
                AnimalType.Pigeon => _pigeonPrefab,
                AnimalType.Bunny => _bunnyPrefab,
                _ => _puppyPrefab
            };
        }

        private GameObject GetArrowPrefab(ArrowDirection dir)
        {
            return dir switch
            {
                ArrowDirection.Up => _arrowUpPrefab,
                ArrowDirection.Right => _arrowRightPrefab,
                ArrowDirection.Down => _arrowDownPrefab,
                ArrowDirection.Left => _arrowLeftPrefab,
                _ => _arrowRightPrefab
            };
        }

        private GameObject GetWallPrefab(int hp)
        {
            return hp switch
            {
                1 => _wall1Prefab,
                2 => _wall2Prefab,
                _ => _wall3Prefab
            };
        }
    }
}
