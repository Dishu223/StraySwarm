using UnityEngine;

namespace StraySwarm.Gameplay
{
    /// <summary>
    /// Creates a grounded soft oval drop shadow beneath characters and baskets.
    /// Gives flat 2D sprites a grounded, tactile 3D feel!
    /// </summary>
    public class DropShadow : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset = new Vector3(0f, -0.3f, 0f);
        [SerializeField] private Vector3 _shadowScale = new Vector3(0.8f, 0.3f, 1f);
        [SerializeField] private float _alpha = 0.25f;

        private GameObject _shadowObj;

        private void Start()
        {
            CreateShadow();
        }

        private void CreateShadow()
        {
            _shadowObj = new GameObject("Shadow_Visual");
            _shadowObj.transform.SetParent(transform, false);
            _shadowObj.transform.localPosition = _offset;
            _shadowObj.transform.localScale = _shadowScale;

            SpriteRenderer sr = _shadowObj.AddComponent<SpriteRenderer>();
            sr.sprite = GetOrCreateShadowSprite();
            sr.color = new Color(0f, 0f, 0f, _alpha);
            sr.sortingOrder = -1; // Render directly underneath the entity
        }

        private static Sprite _cachedShadowSprite;

        private static Sprite GetOrCreateShadowSprite()
        {
            if (_cachedShadowSprite != null) return _cachedShadowSprite;

            int size = 64;
            Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            Color[] colors = new Color[size * size];

            Vector2 center = new Vector2(size * 0.5f, size * 0.5f);
            float radius = size * 0.45f;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dist = Vector2.Distance(new Vector2(x, y), center);
                    if (dist <= radius)
                    {
                        float edgeFade = Mathf.Clamp01((radius - dist) / (radius * 0.35f));
                        colors[y * size + x] = new Color(1f, 1f, 1f, edgeFade);
                    }
                    else
                    {
                        colors[y * size + x] = Color.clear;
                    }
                }
            }

            tex.SetPixels(colors);
            tex.Apply();
            _cachedShadowSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
            return _cachedShadowSprite;
        }
    }
}
