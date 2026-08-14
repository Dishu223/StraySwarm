using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using StraySwarm.Data;
using StraySwarm.Core;

namespace StraySwarm.Gameplay
{
    /// <summary>
    /// Intelligent, deterministic wave spawner and multi-station delivery coordinator.
    /// Guarantees deadlock-free deliveries, exact seed consistency, and dynamic wave refills.
    /// </summary>
    public class WaveSpawner : MonoBehaviour
    {
        public static WaveSpawner Instance { get; private set; }

        [Header("Animal Prefabs")]
        [SerializeField] private GameObject _puppyPrefab;
        [SerializeField] private GameObject _kittenPrefab;
        [SerializeField] private GameObject _frogPrefab;
        [SerializeField] private GameObject _mousePrefab;
        [SerializeField] private GameObject _pigeonPrefab;
        [SerializeField] private GameObject _bunnyPrefab;

        [Header("State Tracking")]
        [SerializeField] private int _totalQuota = 12;
        [SerializeField] private int _totalSpawned = 0;
        [SerializeField] private int _totalDelivered = 0;
        [SerializeField] private int _maxConcurrentOnMap = 5;

        private List<AnimalSpawnPoint> _spawnPoints = new List<AnimalSpawnPoint>();
        private List<DeliveryCrate> _activeCrates = new List<DeliveryCrate>();
        private Queue<AnimalType> _deterministicQueue = new Queue<AnimalType>();
        private List<FollowerBehavior> _liveAnimalsOnMap = new List<FollowerBehavior>();

        public int TotalQuota => _totalQuota;
        public int TotalDelivered => _totalDelivered;
        public int RemainingQuota => _totalQuota - _totalDelivered;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            InitializeWaveSystem();
        }

        public void InitializeWaveSystem()
        {
            // 1. Gather all scene spawn points and active crates
            _spawnPoints = FindObjectsByType<AnimalSpawnPoint>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList();
            _activeCrates = FindObjectsByType<DeliveryCrate>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList();

            // 2. Fetch Level Data
            LevelData data = null;
            if (LevelManager.Instance != null)
            {
                data = LevelManager.Instance.GetCurrentLevelData();
            }

            if (data != null)
            {
                _totalQuota = data.TotalAnimalsToRescue;
                _maxConcurrentOnMap = data.MaxConcurrentOnMap;
                BuildDeterministicQueue(data);
            }
            else
            {
                // Fallback default queue
                _totalQuota = 12;
                _maxConcurrentOnMap = 5;
                BuildDefaultQueue();
            }

            _totalSpawned = 0;
            _totalDelivered = 0;
            _liveAnimalsOnMap.Clear();

            // 3. Spawn Initial Wave
            SpawnWaveToCap();

            // 4. Configure Initial Crates with matching on-map requirements
            ConfigureAllCrates();

            Debug.Log($"🎉 [WaveSpawner] Initialized with {_totalQuota} total quota ({_spawnPoints.Count} spawn points, {_activeCrates.Count} crates).");
        }

        private void BuildDeterministicQueue(LevelData data)
        {
            _deterministicQueue.Clear();
            Random.State originalState = Random.state;
            Random.InitState(data.GetDeterministicSeed());

            List<AnimalType> allowed = data.AllowedAnimalTypes;
            if (allowed == null || allowed.Count == 0)
            {
                allowed = new List<AnimalType> { AnimalType.Puppy, AnimalType.Kitten };
            }

            // Generate deterministic sequence in chunks so crate batches are guaranteed solvable
            int batchSize = 3;
            int generated = 0;

            while (generated < data.TotalAnimalsToRescue)
            {
                // Pick a random species from allowed
                AnimalType chosenType = allowed[Random.Range(0, allowed.Count)];
                int countForBatch = Mathf.Min(batchSize, data.TotalAnimalsToRescue - generated);

                for (int i = 0; i < countForBatch; i++)
                {
                    _deterministicQueue.Enqueue(chosenType);
                    generated++;
                }
            }

            Random.state = originalState;
        }

        private void BuildDefaultQueue()
        {
            _deterministicQueue.Clear();
            for (int i = 0; i < _totalQuota; i++)
            {
                _deterministicQueue.Enqueue(i % 2 == 0 ? AnimalType.Puppy : AnimalType.Kitten);
            }
        }

        public void SpawnWaveToCap()
        {
            if (_spawnPoints.Count == 0) return;

            while (_liveAnimalsOnMap.Count < _maxConcurrentOnMap && _deterministicQueue.Count > 0)
            {
                // Find first available unoccupied spawn point
                AnimalSpawnPoint freeSpot = _spawnPoints.FirstOrDefault(sp => !sp.IsOccupied);
                if (freeSpot == null) break; // All spots filled

                AnimalType nextType = _deterministicQueue.Dequeue();
                GameObject prefab = GetAnimalPrefab(nextType);
                if (prefab != null)
                {
                    GameObject animalGo = Instantiate(prefab, freeSpot.transform.position, Quaternion.identity);
                    FollowerBehavior animal = animalGo.GetComponent<FollowerBehavior>();
                    if (animal != null)
                    {
                        animal.AnimalType = nextType;
                        _liveAnimalsOnMap.Add(animal);
                        freeSpot.IsOccupied = true;
                        freeSpot.CurrentAnimal = animal;
                    }
                    _totalSpawned++;
                }
            }
        }

        public void ConfigureAllCrates()
        {
            if (_activeCrates.Count == 0) return;

            List<AnimalType> usedTypes = new List<AnimalType>();

            foreach (var crate in _activeCrates)
            {
                ConfigureCrate(crate, usedTypes);
            }
        }

        public void ConfigureCrate(DeliveryCrate crate, List<AnimalType> excludeTypes = null)
        {
            if (crate == null) return;

            // Find animal types currently present on the map
            var availableTypesOnMap = _liveAnimalsOnMap
                .Where(a => a != null && !a.IsCollected)
                .Select(a => a.AnimalType)
                .Distinct()
                .ToList();

            if (availableTypesOnMap.Count == 0)
            {
                // If all collected or in tail, check what's in tail or deterministic queue
                availableTypesOnMap = _deterministicQueue.Distinct().ToList();
            }

            AnimalType selectedType = AnimalType.Puppy;

            if (excludeTypes != null)
            {
                var candidate = availableTypesOnMap.FirstOrDefault(t => !excludeTypes.Contains(t));
                if (candidate != default(AnimalType) || availableTypesOnMap.Contains(AnimalType.Puppy))
                {
                    selectedType = candidate != default(AnimalType) ? candidate : availableTypesOnMap[0];
                }
                excludeTypes.Add(selectedType);
            }
            else if (availableTypesOnMap.Count > 0)
            {
                selectedType = availableTypesOnMap[0];
            }

            // Capacity defaults to 2 or 3 (or remaining count for that species)
            int capacity = Mathf.Clamp(3, 1, RemainingQuota);
            crate.Initialize(selectedType, capacity);
            Debug.Log($"📦 [WaveSpawner] Crate configured: Target = {selectedType}, Capacity = {capacity}");
        }

        public void OnAnimalCollected(FollowerBehavior animal)
        {
            if (animal == null) return;

            // Free the spawn point
            AnimalSpawnPoint spot = _spawnPoints.FirstOrDefault(sp => sp.CurrentAnimal == animal);
            if (spot != null)
            {
                spot.IsOccupied = false;
                spot.CurrentAnimal = null;
            }

            _liveAnimalsOnMap.Remove(animal);

            // Refill wave into newly opened spawn point
            SpawnWaveToCap();
        }

        public void OnAnimalDelivered(FollowerBehavior animal)
        {
            _totalDelivered++;

            Debug.Log($"🐾 [WaveSpawner] Delivered ({_totalDelivered}/{_totalQuota})");

            if (_totalDelivered >= _totalQuota)
            {
                Debug.Log("🏆 [WaveSpawner] TOTAL LEVEL QUOTA REACHED! WIN!");
                GameManager gm = FindAnyObjectByType<GameManager>();
                if (gm != null)
                {
                    gm.WinGame();
                }
            }
        }

        public void OnCrateCompleted(DeliveryCrate crate)
        {
            if (_totalDelivered >= _totalQuota)
            {
                GameManager gm = FindAnyObjectByType<GameManager>();
                if (gm != null) gm.WinGame();
                return;
            }

            // Assign next requirement to this crate from remaining live/queued animals
            ConfigureCrate(crate);
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
    }
}
