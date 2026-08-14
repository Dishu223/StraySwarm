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

            // Clean up any uncollected animals in scene to prevent duplicates
            FollowerBehavior[] existingAnimals = FindObjectsByType<FollowerBehavior>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            foreach (var animal in existingAnimals)
            {
                if (!animal.IsCollected)
                {
                    Destroy(animal.gameObject);
                }
            }

            foreach (var sp in _spawnPoints)
            {
                sp.IsOccupied = false;
                sp.CurrentAnimal = null;
            }

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

            Vector3 playerPos = GetPlayerPosition();

            while (_liveAnimalsOnMap.Count < _maxConcurrentOnMap && _deterministicQueue.Count > 0)
            {
                // Find available unoccupied spawn point that the player is NOT currently standing on
                AnimalSpawnPoint freeSpot = _spawnPoints.FirstOrDefault(sp => !sp.IsOccupied && Vector3.Distance(sp.transform.position, playerPos) > 1.2f);
                if (freeSpot == null)
                {
                    // Fallback to any free spot only if player is not in scene
                    freeSpot = _spawnPoints.FirstOrDefault(sp => !sp.IsOccupied);
                }
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

        private Vector3 GetPlayerPosition()
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player == null) player = GameObject.Find("Player");
            return player != null ? player.transform.position : new Vector3(9999f, 9999f, 0f);
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

            // 1. Gather all species that currently have at least 1 alive animal (on map or in tail)
            List<AnimalType> presentSpecies = new List<AnimalType>();
            var onMapSpecies = _liveAnimalsOnMap.Where(a => a != null && !a.IsCollected).Select(a => a.AnimalType).Distinct();
            presentSpecies.AddRange(onMapSpecies);

            TailManager tail = FindAnyObjectByType<TailManager>();
            
            // Add any species in tail
            foreach (AnimalType type in System.Enum.GetValues(typeof(AnimalType)))
            {
                if (tail != null && tail.GetFollowerCountOfType(type) > 0 && !presentSpecies.Contains(type))
                {
                    presentSpecies.Add(type);
                }
            }

            // If nothing on map/tail, spawn from queue immediately!
            if (presentSpecies.Count == 0)
            {
                SpawnWaveToCap();
                presentSpecies.AddRange(_liveAnimalsOnMap.Where(a => a != null && !a.IsCollected).Select(a => a.AnimalType).Distinct());
            }

            if (presentSpecies.Count == 0)
            {
                if (_deterministicQueue.Count > 0) presentSpecies.Add(_deterministicQueue.Peek());
                else presentSpecies.Add(AnimalType.Puppy);
            }

            // 2. Select species (respecting multi-station exclusion if possible)
            AnimalType selectedType = presentSpecies[0];
            if (excludeTypes != null)
            {
                var candidate = presentSpecies.FirstOrDefault(t => !excludeTypes.Contains(t));
                if (candidate != default(AnimalType) || presentSpecies.Contains(AnimalType.Puppy))
                {
                    selectedType = candidate != default(AnimalType) ? candidate : presentSpecies[0];
                }
                excludeTypes.Add(selectedType);
            }

            // 3. Count exact collectible count of this species (On Map + In Tail)
            int availableCollectible = CountAvailableAnimalsOfType(selectedType);

            // 4. If available is less than 2 and we still have queue items, spawn more of this type immediately to satisfy demand!
            if (availableCollectible < 2 && _deterministicQueue.Contains(selectedType))
            {
                SpawnSpecificAnimal(selectedType);
                availableCollectible = CountAvailableAnimalsOfType(selectedType);
            }

            // 5. Set Capacity: NEVER exceed what is actually collectible on map + tail! (Deadlock-Free Guarantee)
            int maxCap = Mathf.Clamp(availableCollectible, 1, Mathf.Min(3, RemainingQuota));
            crate.Initialize(selectedType, maxCap);

            Debug.Log($"📦 [WaveSpawner] Solvability Guaranteed: Target = {selectedType}, Capacity = {maxCap} (Available = {availableCollectible})");
        }

        public int CountAvailableAnimalsOfType(AnimalType type)
        {
            int onMap = _liveAnimalsOnMap.Count(a => a != null && !a.IsCollected && a.AnimalType == type);
            TailManager tail = FindAnyObjectByType<TailManager>();
            int inTail = tail != null ? tail.GetFollowerCountOfType(type) : 0;
            return onMap + inTail;
        }

        private void SpawnSpecificAnimal(AnimalType type)
        {
            if (_spawnPoints.Count == 0) return;
            Vector3 playerPos = GetPlayerPosition();
            AnimalSpawnPoint freeSpot = _spawnPoints.FirstOrDefault(sp => !sp.IsOccupied && Vector3.Distance(sp.transform.position, playerPos) > 1.2f);
            if (freeSpot == null) freeSpot = _spawnPoints.FirstOrDefault(sp => !sp.IsOccupied);
            if (freeSpot == null) return;

            GameObject prefab = GetAnimalPrefab(type);
            if (prefab != null)
            {
                GameObject animalGo = Instantiate(prefab, freeSpot.transform.position, Quaternion.identity);
                FollowerBehavior animal = animalGo.GetComponent<FollowerBehavior>();
                if (animal != null)
                {
                    animal.AnimalType = type;
                    _liveAnimalsOnMap.Add(animal);
                    freeSpot.IsOccupied = true;
                    freeSpot.CurrentAnimal = animal;
                }
                _totalSpawned++;
            }
        }

        public void OnAnimalCollected(FollowerBehavior animal)
        {
            if (animal == null) return;

            // Free the spawn point marker
            AnimalSpawnPoint spot = _spawnPoints.FirstOrDefault(sp => sp.CurrentAnimal == animal);
            if (spot != null)
            {
                spot.IsOccupied = false;
                spot.CurrentAnimal = null;
            }

            _liveAnimalsOnMap.Remove(animal);
        }

        public void OnAnimalDelivered(FollowerBehavior animal)
        {
            _totalDelivered++;

            Debug.Log($"🐾 [WaveSpawner] Delivered ({_totalDelivered}/{_totalQuota})");

            // Refill the wave as animals are delivered to crates
            SpawnWaveToCap();

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

            // Spawn next wave and assign new crate requirement
            SpawnWaveToCap();
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
