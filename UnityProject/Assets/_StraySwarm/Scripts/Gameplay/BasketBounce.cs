using System.Collections;
using UnityEngine;

namespace StraySwarm.Gameplay
{
    /// <summary>
    /// Animates the wooden basket/crate carrying an animal in the conga-line tail.
    /// Handles pop spawn, cargo sway, and follow lag.
    /// </summary>
    public class BasketBounce : MonoBehaviour
    {
        [Header("Sway & Wobble")]
        [SerializeField] private float _swaySpeed = 5f;
        [SerializeField] private float _swayAngle = 6f;

        private Vector3 _baseScale;
        private float _phaseOffset;

        private void Awake()
        {
            _baseScale = transform.localScale;
            _phaseOffset = Random.Range(0f, 100f);
        }

        private void OnEnable()
        {
            StartCoroutine(PopSpawnRoutine());
        }

        private void Update()
        {
            // Cargo sway animation
            float angle = Mathf.Sin((Time.time + _phaseOffset) * _swaySpeed) * _swayAngle;
            transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }

        private IEnumerator PopSpawnRoutine()
        {
            transform.localScale = Vector3.zero;
            float elapsed = 0f;
            float duration = 0.2f;

            // Overshoot pop: 0 -> 1.3 -> 1.0
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                float scaleMod = 1f + 0.3f * Mathf.Sin(t * Mathf.PI);
                transform.localScale = _baseScale * scaleMod * t;
                yield return null;
            }

            transform.localScale = _baseScale;
        }
    }
}
