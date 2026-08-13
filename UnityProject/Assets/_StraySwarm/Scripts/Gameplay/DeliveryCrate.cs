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
        public AnimalType TargetAnimalType;
        public string RequiredColor = "Blue";
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
            RequiredColor = type.ToString().Replace("Puppy", "").Replace("Kitten", "").Replace("Pigeon", "").Replace("Frog", "").Replace("Hamster", "").Replace("Bunny", "");
        }

        public bool TryAcceptAnimal(FollowerBehavior animal)
        {
            if (IsFull || animal == null) return false;

            bool isMatch = (animal.Data != null && animal.Data.Type == TargetAnimalType) || (animal.AnimalColor == RequiredColor);
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

            if (JuiceManager.Instance != null)
            {
                JuiceManager.Instance.PlayWinConfetti();
            }

            Debug.Log($"[DeliveryCrate] Crate for {RequiredColor} is FULL!");
            
            // Notify StationManager
            StationManager.Instance?.CheckAllStations();
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
