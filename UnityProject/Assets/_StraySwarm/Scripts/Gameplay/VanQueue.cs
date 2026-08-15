using UnityEngine;
using System.Collections.Generic;
using StraySwarm.Core;
using StraySwarm.Data;

namespace StraySwarm.Gameplay
{
    /// <summary>
    /// Controls the sequence of vans arriving at the Rescue Station.
    /// Automatically derives van demand from level data and animates smooth arrival/departure.
    /// </summary>
    public class VanQueue : MonoBehaviour
    {
        [SerializeField] private RescueStation _station;
        [SerializeField] private GameObject _vanPrefab; 
        
        [Header("Level Design")]
        [Tooltip("The sequence of animal species vans for this level.")]
        [SerializeField] private List<AnimalType> _levelVanSequence = new List<AnimalType>();
        
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

            BuildVanSequenceFromLevelData();

            // Wait a tiny bit before spawning the first van so the level is fully initialized
            Invoke(nameof(SpawnNextVan), 0.3f);
        }

        private void BuildVanSequenceFromLevelData()
        {
            if (_levelVanSequence != null && _levelVanSequence.Count > 0) return;

            _levelVanSequence = new List<AnimalType>();
            LevelData data = LevelManager.Instance != null ? LevelManager.Instance.GetCurrentLevelData() : null;

            if (data != null && data.AllowedAnimalTypes != null && data.AllowedAnimalTypes.Count > 0)
            {
                foreach (var species in data.AllowedAnimalTypes)
                {
                    _levelVanSequence.Add(species);
                }
            }
            else
            {
                _levelVanSequence.Add(AnimalType.Puppy);
                _levelVanSequence.Add(AnimalType.Kitten);
            }
        }

        public VanController GetCurrentVan()
        {
            return _activeVan;
        }

        public void SpawnNextVan()
        {
            if (_currentVanIndex >= _levelVanSequence.Count)
            {
                Debug.Log("🎉 [VanQueue] LEVEL COMPLETE! All vans filled! 🎉");
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

            AnimalType nextType = _levelVanSequence[_currentVanIndex];
            _currentVanIndex++;

            GameObject vanObj = Instantiate(_vanPrefab, spawnPos, Quaternion.identity);
            vanObj.name = $"RescueVan_{nextType}";
            
            _activeVan = vanObj.GetComponent<VanController>();
            if (_activeVan != null)
            {
                _activeVan.SetTargetAnimal(nextType);
                _activeVan.Capacity = 3;
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
                if (vanController != null)
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
