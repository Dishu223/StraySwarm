using UnityEngine;
using System.Collections.Generic;

namespace StraySwarm.Gameplay
{
    /// <summary>
    /// Controls the sequence of vans arriving at the Rescue Station.
    /// </summary>
    public class VanQueue : MonoBehaviour
    {
        [SerializeField] private RescueStation _station;
        [SerializeField] private GameObject _vanPrefab; 
        
        [Header("Level Design")]
        [Tooltip("The sequence of animal species vans for this level.")]
        [SerializeField] private List<Data.AnimalType> _levelVanSequence = new List<Data.AnimalType> { Data.AnimalType.BluePuppy, Data.AnimalType.PinkKitten, Data.AnimalType.BluePuppy };
        
        private int _currentVanIndex = 0;
        private VanController _activeVan;

        private void Start()
        {
            if (_station == null) _station = FindAnyObjectByType<RescueStation>();
            
            // Wait a tiny bit before spawning the first van so the game has time to load
            Invoke(nameof(SpawnNextVan), 0.5f);
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
                
                // Tell the GameManager we won!
                Core.GameManager gm = FindAnyObjectByType<Core.GameManager>();
                if (gm != null) gm.WinGame();
                
                return;
            }

            Data.AnimalType nextType = _levelVanSequence[_currentVanIndex];
            _currentVanIndex++;

            // Spawn the van off-screen to the left, and drive it to the parking spot
            Vector3 spawnPos = _station.VanParkingSpot.position + (Vector3.left * 15f);
            GameObject vanObj = Instantiate(_vanPrefab, spawnPos, Quaternion.identity);
            
            _activeVan = vanObj.GetComponent<VanController>();
            _activeVan.SetTargetAnimal(nextType);
            
            // For testing, just need 2 animals to fill a van
            _activeVan.Capacity = 2; 

            // Simple drive-in animation
            StartCoroutine(DriveInRoutine(_activeVan.transform, _station.VanParkingSpot.position, _activeVan));
        }

        private System.Collections.IEnumerator DriveInRoutine(Transform vanTransform, Vector3 target, VanController vanController)
        {
            float speed = 15f;
            while (Vector3.Distance(vanTransform.position, target) > 0.1f)
            {
                // If van was told to drive away mid-arrival, cancel drive-in immediately!
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
                    // If player was already waiting at the station, trigger delivery!
                    _station.AttemptDelivery();
                }
            }
        }
    }
}
