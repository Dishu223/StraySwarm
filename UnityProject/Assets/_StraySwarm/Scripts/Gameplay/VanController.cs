using UnityEngine;
using System.Collections;
using StraySwarm.Data;

namespace StraySwarm.Gameplay
{
    /// <summary>
    /// Represents the delivery truck that accepts a specific animal species (Puppy, Kitten, etc.).
    /// </summary>
    public class VanController : MonoBehaviour
    {
        [Header("Species Configuration")]
        public AnimalType TargetAnimalType = AnimalType.BluePuppy;
        public int Capacity = 3;

        [Header("Visual Feedback")]
        [SerializeField] private SpriteRenderer _targetAnimalIcon;
        
        public bool IsFull => _currentLoad >= Capacity;
        public bool IsDrivingAway { get; private set; } = false;
        public bool IsParked { get; private set; } = false;
        
        private int _currentLoad = 0;

        public void SetParked()
        {
            IsParked = true;
        }

        public void SetTargetAnimal(AnimalType type, Sprite icon = null)
        {
            TargetAnimalType = type;
            if (_targetAnimalIcon != null && icon != null)
            {
                _targetAnimalIcon.sprite = icon;
                _targetAnimalIcon.color = Color.white;
            }
        }

        public bool TryAcceptAnimal(FollowerBehavior animal)
        {
            // Only accept animals if the van is fully parked and not full/driving away!
            if (!IsParked || IsFull || IsDrivingAway || animal == null) return false;
            
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
                
                // Make the animal visually zip into the van!
                animal.FlyToVan(this.transform);
                
                // Trigger a micro camera shake for subtle tactile impact!
                if (CameraShake.Instance != null)
                {
                    CameraShake.Instance.Shake(0.1f, 0.05f);
                }

                // Trigger a juicy squash-and-stretch bounce on the van for each animal!
                StartCoroutine(BounceRoutine());
                
                if (IsFull)
                {
                    DriveAway();
                }
                return true;
            }
            return false; // Wrong animal!
        }

        private void DriveAway()
        {
            IsDrivingAway = true;
            Debug.Log($"[VanController] {TargetAnimalType} Van is FULL! Driving away!");
            
            StartCoroutine(DelayedDriveAwayRoutine());
        }

        private IEnumerator DelayedDriveAwayRoutine()
        {
            // Wait 0.4 seconds so the final animal finishes zipping inside and the van finishes its happy bounce!
            yield return new WaitForSeconds(0.4f);

            float speed = 15f;
            while (transform.position.x < 15f) // Drive off screen to the right
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
                yield return null;
            }
            
            // Tell the queue to bring the next van
            FindAnyObjectByType<VanQueue>()?.SpawnNextVan();
            Destroy(gameObject);
        }

        /// <summary>
        /// A juicy Squash & Stretch bounce animation when an animal jumps inside!
        /// </summary>
        private IEnumerator BounceRoutine()
        {
            float elapsed = 0f;
            float duration = 0.1f;
            Vector3 originalScale = transform.localScale;
            Vector3 squishedScale = new Vector3(originalScale.x * 1.15f, originalScale.y * 0.85f, originalScale.z);

            // 1. Squash down
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                transform.localScale = Vector3.Lerp(originalScale, squishedScale, elapsed / duration);
                yield return null;
            }

            // 2. Stretch back
            elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                transform.localScale = Vector3.Lerp(squishedScale, originalScale, elapsed / duration);
                yield return null;
            }

            transform.localScale = originalScale;
        }
    }
}
