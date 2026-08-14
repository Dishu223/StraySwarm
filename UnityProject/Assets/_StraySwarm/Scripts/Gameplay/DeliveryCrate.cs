using System.Collections;
using UnityEngine;
using StraySwarm.Data;

namespace StraySwarm.Gameplay
{
    /// <summary>
    /// Represents a stationary wooden delivery crate / shelter on the map.
    /// Accepts a specific animal color/type and displays collected animals inside.
    /// </summary>
    public class DeliveryCrate : MonoBehaviour
    {
        [Header("Crate Configuration")]
        public AnimalType TargetAnimalType = AnimalType.Puppy;
        public int Capacity = 3;

        [Header("Visual Feedback")]
        [SerializeField] private SpriteRenderer _crateRenderer;
        [SerializeField] private Sprite _openCrateSprite;
        [SerializeField] private Sprite _fullCrateSprite;
        [SerializeField] private GameObject _colorBanner;

        public bool IsFull => _currentLoad >= Capacity;
        public int CurrentLoad => _currentLoad;

        private int _currentLoad = 0;
        private Vector3 _originalScale;

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        public void Initialize(AnimalType type, int capacity)
        {
            TargetAnimalType = type;
            Capacity = capacity;
        }

        public bool TryAcceptAnimal(FollowerBehavior animal)
        {
            if (IsFull || animal == null) return false;

            bool isMatch = animal.AnimalType == TargetAnimalType || (animal.Data != null && animal.Data.Type == TargetAnimalType);
            if (isMatch)
            {
                _currentLoad++;

                // Play sound & particles!
                if (JuiceManager.Instance != null)
                {
                    JuiceManager.Instance.PlayDeliverParticle(transform.position);
                }

                if (Audio.AudioManager.Instance != null)
                {
                    Audio.AudioManager.Instance.PlayDeliver();
                }

                // Fly animal to crate
                animal.FlyToVan(this.transform);

                // Tactile camera shake
                if (CameraShake.Instance != null)
                {
                    CameraShake.Instance.Shake(0.1f, 0.05f);
                }

                // Juicy bounce
                StartCoroutine(BounceRoutine());

                if (IsFull)
                {
                    OnCrateFilled();
                }

                return true;
            }

            return false;
        }

        private void OnCrateFilled()
        {
            if (_fullCrateSprite != null && _crateRenderer != null)
            {
                _crateRenderer.sprite = _fullCrateSprite;
            }

            Debug.Log($"[DeliveryCrate] Crate for {TargetAnimalType} is FULL!");
            
            // 1. Check via StationManager if present
            if (StationManager.Instance != null)
            {
                StationManager.Instance.CheckAllStations();
            }
            else
            {
                // 2. Direct fallback: check all active crates in scene!
                DeliveryCrate[] allCrates = FindObjectsByType<DeliveryCrate>(FindObjectsInactive.Exclude);
                bool allFull = true;
                foreach (var crate in allCrates)
                {
                    if (!crate.IsFull)
                    {
                        allFull = false;
                        break;
                    }
                }

                if (allFull)
                {
                    Debug.Log("🎉 [DeliveryCrate] All crates filled! Triggering Level Win!");
                    Core.GameManager gm = FindAnyObjectByType<Core.GameManager>();
                    if (gm != null) gm.WinGame();
                }
            }
        }

        private IEnumerator BounceRoutine()
        {
            float elapsed = 0f;
            float duration = 0.1f;
            Vector3 squishedScale = new Vector3(_originalScale.x * 1.18f, _originalScale.y * 0.82f, _originalScale.z);

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                transform.localScale = Vector3.Lerp(_originalScale, squishedScale, elapsed / duration);
                yield return null;
            }

            elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                transform.localScale = Vector3.Lerp(squishedScale, _originalScale, elapsed / duration);
                yield return null;
            }

            transform.localScale = _originalScale;
        }
    }
}
