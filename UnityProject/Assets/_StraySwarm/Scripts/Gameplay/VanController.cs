using UnityEngine;
using System.Collections;
using StraySwarm.Data;

namespace StraySwarm.Gameplay
{
    /// <summary>
    /// Represents the rescue van that accepts a specific animal species (Puppy, Kitten, etc.).
    /// Features dynamic theme coloring and a high-contrast floating species icon cue.
    /// </summary>
    public class VanController : MonoBehaviour
    {
        [Header("Species Configuration")]
        public AnimalType TargetAnimalType = AnimalType.Puppy;
        public int Capacity = 3;

        [Header("Visual Feedback")]
        [SerializeField] private SpriteRenderer _bodyRenderer;
        [SerializeField] private SpriteRenderer _targetAnimalIcon;
        
        public bool IsFull => _currentLoad >= Capacity;
        public bool IsDrivingAway { get; private set; } = false;
        public bool IsParked { get; private set; } = false;
        
        private int _currentLoad = 0;

        private void Awake()
        {
            if (_bodyRenderer == null) _bodyRenderer = GetComponent<SpriteRenderer>();
            if (_bodyRenderer != null) _bodyRenderer.sortingOrder = 8;
            EnsureIconBadge();
        }

        private void EnsureIconBadge()
        {
            if (_targetAnimalIcon == null)
            {
                Transform existingBadge = transform.Find("AnimalIconBadge");
                if (existingBadge != null)
                {
                    _targetAnimalIcon = existingBadge.GetComponent<SpriteRenderer>();
                }
                else
                {
                    GameObject badgeObj = new GameObject("AnimalIconBadge");
                    badgeObj.transform.SetParent(transform, false);
                    badgeObj.transform.localPosition = new Vector3(0f, 0.95f, 0f);
                    badgeObj.transform.localScale = new Vector3(0.7f, 0.7f, 1f);

                    _targetAnimalIcon = badgeObj.AddComponent<SpriteRenderer>();
                    _targetAnimalIcon.sortingOrder = 14;
                }
            }
        }

        public void SetParked()
        {
            IsParked = true;
        }

        public void SetTargetAnimal(AnimalType type, Sprite icon = null)
        {
            TargetAnimalType = type;
            ApplySpeciesTheme(type, icon);
        }

        private void ApplySpeciesTheme(AnimalType type, Sprite customIcon = null)
        {
            EnsureIconBadge();

            // 1. Set Body Color matching Kawaii species theme
            Color themeColor;
            switch (type)
            {
                case AnimalType.Puppy:  themeColor = new Color(0.36f, 0.72f, 1f); break; // Sky Blue #5CB8FF
                case AnimalType.Kitten: themeColor = new Color(1f, 0.49f, 0.70f); break; // Bubblegum Pink #FF7EB3
                case AnimalType.Frog:   themeColor = new Color(0.49f, 0.85f, 0.62f); break; // Mint Green #7ED89E
                case AnimalType.Mouse:  themeColor = new Color(1f, 0.62f, 0.26f); break; // Tangerine Orange #FF9F43
                case AnimalType.Pigeon: themeColor = new Color(1f, 0.80f, 0.01f); break; // Sunbeam Yellow #FFCC02
                case AnimalType.Bunny:  themeColor = new Color(0.64f, 0.61f, 1f); break; // Lavender Purple #A29BFE
                default: themeColor = new Color(0.98f, 0.45f, 0.09f); break;
            }

            if (_bodyRenderer != null)
            {
                _bodyRenderer.color = themeColor;
                _bodyRenderer.sortingOrder = 8;
            }

            // 2. Resolve Species Icon Sprite
            Sprite iconSprite = customIcon;
            if (iconSprite == null)
            {
#if UNITY_EDITOR
                string assetName = type.ToString();
                string[] guids = UnityEditor.AssetDatabase.FindAssets($"Animal_{assetName} t:GameObject");
                if (guids.Length > 0)
                {
                    string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
                    GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    if (prefab != null)
                    {
                        var sr = prefab.GetComponent<SpriteRenderer>();
                        if (sr != null) iconSprite = sr.sprite;
                    }
                }

                if (iconSprite == null)
                {
                    iconSprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_StraySwarm/Art/Placeholders/RoundedCube.png");
                }
#endif
            }

            if (_targetAnimalIcon != null)
            {
                if (iconSprite != null) _targetAnimalIcon.sprite = iconSprite;
                _targetAnimalIcon.color = Color.white;
                _targetAnimalIcon.sortingOrder = 14;
                StopAllCoroutines();
                StartCoroutine(BadgeBobRoutine());
            }
        }

        private IEnumerator BadgeBobRoutine()
        {
            if (_targetAnimalIcon == null) yield break;
            Transform badgeTransform = _targetAnimalIcon.transform;
            Vector3 baseLocalPos = new Vector3(0f, 0.95f, 0f);

            while (true)
            {
                float bob = Mathf.Sin(Time.time * 4f) * 0.08f;
                badgeTransform.localPosition = baseLocalPos + new Vector3(0f, bob, 0f);
                yield return null;
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

                // Trigger a squash-and-stretch bounce on the van
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
            // Wait 0.4 seconds for delivery animation to finish
            yield return new WaitForSeconds(0.4f);

            float speed = 15f;
            while (transform.position.x < 16f)
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
                yield return null;
            }
            
            // Tell the queue to bring the next van
            FindAnyObjectByType<VanQueue>()?.SpawnNextVan();
            Destroy(gameObject);
        }

        private IEnumerator BounceRoutine()
        {
            float elapsed = 0f;
            float duration = 0.1f;
            Vector3 originalScale = new Vector3(1.5f, 1f, 1f);
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
