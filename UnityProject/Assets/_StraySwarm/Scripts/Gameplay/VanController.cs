using UnityEngine;
using System.Collections;
using StraySwarm.Data;

namespace StraySwarm.Gameplay
{
    /// <summary>
    /// Represents the rescue van that accepts a specific animal species (Puppy, Kitten, etc.).
    /// Features a compact comic-style thought bubble displaying the target animal's face without scale distortion.
    /// </summary>
    public class VanController : MonoBehaviour
    {
        [Header("Species Configuration")]
        public AnimalType TargetAnimalType = AnimalType.Puppy;
        public int Capacity = 3;

        [Header("Orientation Settings")]
        [Tooltip("Z rotation angle in degrees (e.g. -90 to face right)")]
        [SerializeField] private float _vanRotationZ = -90f;

        [Header("Visual Feedback")]
        [SerializeField] private SpriteRenderer _bodyRenderer;
        [SerializeField] private Transform _thoughtBubbleRoot;
        [SerializeField] private SpriteRenderer _targetAnimalIcon;
        
        public bool IsFull => _currentLoad >= Capacity;
        public bool IsDrivingAway { get; private set; } = false;
        public bool IsParked { get; private set; } = false;
        
        private int _currentLoad = 0;

        private void Awake()
        {
            transform.rotation = Quaternion.Euler(0f, 0f, _vanRotationZ);
            if (_bodyRenderer == null) _bodyRenderer = GetComponent<SpriteRenderer>();
            if (_bodyRenderer != null) _bodyRenderer.sortingOrder = 8;
            EnsureThoughtBubble();
        }

        private void LateUpdate()
        {
            // Lock Thought Bubble to World Space top-right of the van with a gentle bob (immune to parent scale & rotation!)
            if (_thoughtBubbleRoot != null)
            {
                float bob = Mathf.Sin(Time.time * 3.5f) * 0.04f;
                _thoughtBubbleRoot.position = transform.position + new Vector3(0.55f, 0.48f + bob, 0f);
                _thoughtBubbleRoot.rotation = Quaternion.identity;
                _thoughtBubbleRoot.localScale = Vector3.one;
            }
        }

        private void EnsureThoughtBubble()
        {
            // Clean up old legacy cloud if present
            Transform oldCloud = transform.Find("CloudThoughtBubble");
            if (oldCloud != null) DestroyImmediate(oldCloud.gameObject);

            if (_thoughtBubbleRoot == null)
            {
                // 1. Root Thought Bubble created in World Space
                GameObject rootObj = new GameObject("MiniThoughtBubble");
                rootObj.transform.position = transform.position + new Vector3(0.55f, 0.48f, 0f);
                rootObj.transform.rotation = Quaternion.identity;
                rootObj.transform.localScale = Vector3.one;
                _thoughtBubbleRoot = rootObj.transform;

#if UNITY_EDITOR
                Sprite bubbleSprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_StraySwarm/Art/Placeholders/RoundedCube.png");
                if (bubbleSprite == null) bubbleSprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
#endif

                // 2. Trailing Small Dot 1 (near van roof)
                GameObject dot1 = new GameObject("Dot1");
                dot1.transform.SetParent(_thoughtBubbleRoot, false);
                dot1.transform.localPosition = new Vector3(-0.20f, -0.16f, 0f);
                dot1.transform.localScale = new Vector3(0.08f, 0.08f, 1f);
                SpriteRenderer dot1Sr = dot1.AddComponent<SpriteRenderer>();
                dot1Sr.color = Color.white;
                dot1Sr.sortingOrder = 14;
#if UNITY_EDITOR
                if (bubbleSprite != null) dot1Sr.sprite = bubbleSprite;
#endif

                // 3. Trailing Small Dot 2 (mid)
                GameObject dot2 = new GameObject("Dot2");
                dot2.transform.SetParent(_thoughtBubbleRoot, false);
                dot2.transform.localPosition = new Vector3(-0.10f, -0.08f, 0f);
                dot2.transform.localScale = new Vector3(0.12f, 0.12f, 1f);
                SpriteRenderer dot2Sr = dot2.AddComponent<SpriteRenderer>();
                dot2Sr.color = Color.white;
                dot2Sr.sortingOrder = 14;
#if UNITY_EDITOR
                if (bubbleSprite != null) dot2Sr.sprite = bubbleSprite;
#endif

                // 4. Main White Thought Bubble Pill (Horizontal)
                GameObject mainBubble = new GameObject("MainBubble");
                mainBubble.transform.SetParent(_thoughtBubbleRoot, false);
                mainBubble.transform.localPosition = Vector3.zero;
                mainBubble.transform.localScale = new Vector3(0.55f, 0.42f, 1f);
                SpriteRenderer bubbleSr = mainBubble.AddComponent<SpriteRenderer>();
                bubbleSr.color = Color.white;
                bubbleSr.sortingOrder = 14;
#if UNITY_EDITOR
                if (bubbleSprite != null) bubbleSr.sprite = bubbleSprite;
#endif

                // 5. Animal Face Photo Icon inside Main Bubble
                GameObject iconObj = new GameObject("AnimalFaceIcon");
                iconObj.transform.SetParent(_thoughtBubbleRoot, false);
                iconObj.transform.localPosition = new Vector3(0f, 0.01f, 0f);
                iconObj.transform.localScale = new Vector3(0.30f, 0.30f, 1f);
                _targetAnimalIcon = iconObj.AddComponent<SpriteRenderer>();
                _targetAnimalIcon.sortingOrder = 16;
            }
        }

        private void OnDestroy()
        {
            if (_thoughtBubbleRoot != null)
            {
                Destroy(_thoughtBubbleRoot.gameObject);
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
            EnsureThoughtBubble();

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
#if UNITY_EDITOR
                if (_bodyRenderer.sprite == null || _bodyRenderer.sprite.name == "RoundedCube" || _bodyRenderer.sprite.name == "Knob")
                {
                    Sprite vanSprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_StraySwarm/Art/Characters/rescue_van.png");
                    if (vanSprite == null) vanSprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_StraySwarm/Art/Characters/rescue_van.jpeg");
                    if (vanSprite != null) _bodyRenderer.sprite = vanSprite;
                }
#endif
                if (_bodyRenderer.sprite != null && _bodyRenderer.sprite.name.Contains("rescue_van"))
                {
                    _bodyRenderer.color = Color.white;
                }
                else
                {
                    _bodyRenderer.color = themeColor;
                }
                _bodyRenderer.sortingOrder = 8;
            }

            // 2. Resolve Exact Animal WorldSprite from AnimalData
            Sprite iconSprite = customIcon;
            if (iconSprite == null)
            {
#if UNITY_EDITOR
                string assetName = "";
                switch (type)
                {
                    case AnimalType.Puppy:  assetName = "BluePuppy"; break;
                    case AnimalType.Kitten: assetName = "PinkKitten"; break;
                    case AnimalType.Frog:   assetName = "GreenFrog"; break;
                    case AnimalType.Mouse:  assetName = "OrangeHamster"; break;
                    case AnimalType.Pigeon: assetName = "YellowPigeon"; break;
                    case AnimalType.Bunny:  assetName = "PurpleBunny"; break;
                }

                AnimalData aData = UnityEditor.AssetDatabase.LoadAssetAtPath<AnimalData>($"Assets/_StraySwarm/Data/Animals/{assetName}.asset");
                if (aData != null && aData.WorldSprite != null)
                {
                    iconSprite = aData.WorldSprite;
                }

                if (iconSprite == null)
                {
                    string[] guids = UnityEditor.AssetDatabase.FindAssets($"Animal_{type} t:GameObject");
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
                _targetAnimalIcon.sortingOrder = 16;
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

                // Notify WaveSpawner so delivery count and HUD update immediately!
                if (WaveSpawner.Instance != null)
                {
                    WaveSpawner.Instance.OnAnimalDelivered(animal);
                }
                
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
                
                // Trigger camera shake
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
            Vector3 originalScale = new Vector3(1.6f, 1.1f, 1f);
            Vector3 squishedScale = new Vector3(originalScale.x * 1.12f, originalScale.y * 0.88f, originalScale.z);

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
