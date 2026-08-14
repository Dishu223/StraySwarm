using UnityEngine;
using StraySwarm.Data;

namespace StraySwarm.Gameplay
{
    /// <summary>
    /// Represents an individual animal waiting on the map or following in the player's tail.
    /// Configured with AnimalData for automatic visuals, colors, and sounds.
    /// </summary>
    public class FollowerBehavior : MonoBehaviour
    {
        [Header("Data Configuration")]
        [SerializeField] private AnimalData _animalData;

        public bool IsCollected { get; private set; } = false;
        public AnimalData Data => _animalData;
        public AnimalType AnimalType => _animalData != null ? _animalData.Type : AnimalType.BluePuppy;

        private SpriteRenderer _spriteRenderer;
        private CubeWobble _wobble;
        private BasketBounce _basketBounce;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _wobble = GetComponent<CubeWobble>();
            _basketBounce = GetComponent<BasketBounce>();

            ApplyAnimalData();
        }

        private void OnValidate()
        {
            ApplyAnimalData();
        }

        public void SetAnimalData(AnimalData data)
        {
            _animalData = data;
            ApplyAnimalData();
        }

        private void ApplyAnimalData()
        {
            if (_animalData != null)
            {
                if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
                if (_spriteRenderer != null)
                {
                    if (_animalData.WorldSprite != null)
                    {
                        _spriteRenderer.sprite = _animalData.WorldSprite;
                    }
                    _spriteRenderer.color = Color.white; // Keep original pure sprite colors!
                }
            }
        }

        public void Collect()
        {
            if (IsCollected) return;
            IsCollected = true;

            // Enable basket bounce / sway when joining the tail
            if (_basketBounce != null)
            {
                _basketBounce.enabled = true;
            }

            // Trigger pickup wobble hop
            if (_wobble != null)
            {
                _wobble.TriggerHop(0.2f);
            }
        }

        public void FlyToVan(Transform targetTransform)
        {
            StartCoroutine(FlyRoutine(targetTransform));
        }

        private System.Collections.IEnumerator FlyRoutine(Transform targetTransform)
        {
            float flySpeed = 25f;
            
            while (targetTransform != null && Vector3.Distance(transform.position, targetTransform.position) > 0.4f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, flySpeed * Time.deltaTime);
                yield return null;
            }
            
            gameObject.SetActive(false);
        }
    }
}
