using UnityEngine;
using System.Collections.Generic;
using StraySwarm.Core;
using StraySwarm.Data;

namespace StraySwarm.Gameplay
{
    /// <summary>
    /// Controls the sequence of vans arriving at the Rescue Station.
    /// Perfectly synchronizes van capacities with the exact animals present on the map.
    /// </summary>
    public class VanQueue : MonoBehaviour
    {
        [System.Serializable]
        public struct VanDemand
        {
            public AnimalType Type;
            public int Capacity;
        }

        [SerializeField] private RescueStation _station;
        [SerializeField] private GameObject _vanPrefab; 
        
        [Header("Runtime Van Schedule")]
        [SerializeField] private List<VanDemand> _vanDemands = new List<VanDemand>();
        
        private int _currentVanIndex = 0;
        private VanController _activeVan;

        private void Awake()
        {
            if (_station == null) _station = GetComponent<RescueStation>();
            if (_station == null) _station = FindAnyObjectByType<RescueStation>();

            if (_vanPrefab == null)
            {
#if UNITY_EDITOR
                _vanPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_StraySwarm/Prefabs/Core/VanPrefab.prefab");
#endif
            }
        }

        private void Start()
        {
            if (_station == null) _station = GetComponent<RescueStation>();
            if (_station == null) _station = FindAnyObjectByType<RescueStation>();

            // Small delay to let WaveSpawner instantiate map animals first
            Invoke(nameof(InitializeVanSchedule), 0.15f);
        }

        public void InitializeVanSchedule()
        {
            BuildVanSequenceFromLevelAnimals();
            SpawnNextVan();
        }

        private void BuildVanSequenceFromLevelAnimals()
        {
            _vanDemands.Clear();
            _currentVanIndex = 0;

            // 1. Count all alive animals on the map
            var allAnimals = FindObjectsByType<FollowerBehavior>(FindObjectsInactive.Exclude);
            Dictionary<AnimalType, int> counts = new Dictionary<AnimalType, int>();

            foreach (var a in allAnimals)
            {
                if (a != null)
                {
                    if (!counts.ContainsKey(a.AnimalType)) counts[a.AnimalType] = 0;
                    counts[a.AnimalType]++;
                }
            }

            // 2. If no animals found on map yet, fallback to LevelData
            if (counts.Count == 0)
            {
                LevelData data = LevelManager.Instance != null ? LevelManager.Instance.GetCurrentLevelData() : null;
                var allowed = data != null ? data.AllowedAnimalTypes : null;
                if (allowed == null || allowed.Count == 0) allowed = new List<AnimalType> { AnimalType.Puppy, AnimalType.Kitten };
                foreach (var t in allowed)
                {
                    counts[t] = 3;
                }
            }

            // 3. Split counts into van deliveries (max 3 per van)
            foreach (var kvp in counts)
            {
                int remaining = kvp.Value;
                while (remaining > 0)
                {
                    int cap = Mathf.Min(3, remaining);
                    _vanDemands.Add(new VanDemand { Type = kvp.Key, Capacity = cap });
                    remaining -= cap;
                }
            }

            Debug.Log($"🚐 [VanQueue] Built {_vanDemands.Count} vans perfectly matching {allAnimals.Length} map animals.");
        }

        public VanController GetCurrentVan()
        {
            return _activeVan;
        }

        public void SpawnNextVan()
        {
            if (_currentVanIndex >= _vanDemands.Count)
            {
                Debug.Log("🎉 [VanQueue] ALL VANS FILLED! Triggering Victory! 🎉");
                GameManager gm = FindAnyObjectByType<GameManager>();
                if (gm != null) gm.WinGame();
                return;
            }

            if (_station == null) _station = FindAnyObjectByType<RescueStation>();
            if (_station == null) return;

            Transform parkingTransform = _station.VanParkingSpot != null ? _station.VanParkingSpot : _station.transform;
            Vector3 parkPos = parkingTransform.position;
            Vector3 spawnPos = parkPos + (Vector3.left * 12f);

            if (_vanPrefab == null)
            {
#if UNITY_EDITOR
                _vanPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_StraySwarm/Prefabs/Core/VanPrefab.prefab");
#endif
            }

            if (_vanPrefab == null)
            {
                Debug.LogError("[VanQueue] VanPrefab is missing! Cannot spawn rescue van.");
                return;
            }

            VanDemand demand = _vanDemands[_currentVanIndex];
            _currentVanIndex++;

            GameObject vanObj = Instantiate(_vanPrefab, spawnPos, _vanPrefab != null ? _vanPrefab.transform.rotation : Quaternion.Euler(0f, 0f, -90f));
            vanObj.name = $"RescueVan_{demand.Type}";
            
            _activeVan = vanObj.GetComponent<VanController>();
            if (_activeVan != null)
            {
                _activeVan.SetTargetAnimal(demand.Type);
                _activeVan.Capacity = demand.Capacity;
                StartCoroutine(DriveInRoutine(_activeVan.transform, parkPos, _activeVan));
            }
        }

        private System.Collections.IEnumerator DriveInRoutine(Transform vanTransform, Vector3 target, VanController vanController)
        {
            float speed = 14f;
            while (vanTransform != null && Vector3.Distance(vanTransform.position, target) > 0.1f)
            {
                if (vanController != null && vanController.IsDrivingAway) yield break;

                vanTransform.position = Vector3.MoveTowards(vanTransform.position, target, speed * Time.deltaTime);
                yield return null;
            }
            
            if (vanTransform != null)
            {
                vanTransform.position = target;

                // Brief 0.25s arrival settle pause before opening doors
                yield return new WaitForSeconds(0.25f);

                if (vanController != null && !vanController.IsDrivingAway)
                {
                    vanController.SetParked();
                    if (_station != null)
                    {
                        _station.AttemptDelivery();
                    }
                }
            }
        }
    }
}
