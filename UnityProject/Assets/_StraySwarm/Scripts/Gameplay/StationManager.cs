using System.Collections.Generic;
using UnityEngine;

namespace StraySwarm.Gameplay
{
    /// <summary>
    /// Manages all stationary delivery crates in the level.
    /// Triggers Level Win when all crates are filled.
    /// </summary>
    public class StationManager : MonoBehaviour
    {
        public static StationManager Instance { get; private set; }

        private List<DeliveryCrate> _crates = new List<DeliveryCrate>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            RegisterAllCrates();
        }

        public void RegisterAllCrates()
        {
            _crates.Clear();
            DeliveryCrate[] found = FindObjectsByType<DeliveryCrate>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            _crates.AddRange(found);
            Debug.Log($"[StationManager] Registered {_crates.Count} delivery crates.");
        }

        public void CheckAllStations()
        {
            if (_crates.Count == 0) return;

            bool allFull = true;
            foreach (var crate in _crates)
            {
                if (!crate.IsFull)
                {
                    allFull = false;
                    break;
                }
            }

            if (allFull)
            {
                Debug.Log("[StationManager] ALL CRATES FILLED! Triggering Level Win!");
                Core.GameManager gameManager = FindAnyObjectByType<Core.GameManager>();
                if (gameManager != null)
                {
                    gameManager.WinGame();
                }
            }
        }
    }
}
