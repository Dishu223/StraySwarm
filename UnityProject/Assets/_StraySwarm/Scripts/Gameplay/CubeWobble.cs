using System.Collections;
using UnityEngine;

namespace StraySwarm.Gameplay
{
    /// <summary>
    /// Code-driven wobble, squash & stretch animation for Kawaii Cube characters.
    /// Replaces heavy frame-by-frame sprite sheets with juicy physics!
    /// </summary>
    public class CubeWobble : MonoBehaviour
    {
        [Header("Idle Bob Settings")]
        [SerializeField] private bool _enableIdleBob = true;
        [SerializeField] private float _idleSpeed = 3f;
        [SerializeField] private float _idleScaleAmount = 0.04f;

        [Header("Movement Squash & Stretch")]
        [SerializeField] private Vector3 _squashScale = new Vector3(1.25f, 0.75f, 1f);
        [SerializeField] private Vector3 _stretchScale = new Vector3(0.8f, 1.3f, 1f);

        private Vector3 _originalScale;
        private Coroutine _wobbleRoutine;
        private float _randomPhaseOffset;

        private void Awake()
        {
            _originalScale = transform.localScale;
            _randomPhaseOffset = Random.Range(0f, 100f);
        }

        private void Update()
        {
            // Idle breathing effect via scale modulation (never overrides position!)
            if (_enableIdleBob && _wobbleRoutine == null)
            {
                float scaleOffset = Mathf.Sin((Time.time + _randomPhaseOffset) * _idleSpeed) * _idleScaleAmount;
                transform.localScale = _originalScale + new Vector3(-scaleOffset * 0.5f, scaleOffset, 0f);
            }
        }

        /// <summary>
        /// Call this method whenever the character moves a grid step!
        /// Performs a juicy stretch -> hop -> squash land cycle.
        /// </summary>
        public void TriggerHop(float stepDuration = 0.2f)
        {
            if (_wobbleRoutine != null)
            {
                StopCoroutine(_wobbleRoutine);
            }
            _wobbleRoutine = StartCoroutine(HopRoutine(stepDuration));
        }

        private IEnumerator HopRoutine(float duration)
        {
            float elapsed = 0f;
            Vector3 startScale = transform.localScale;

            // Phase 1: Stretch Up during move
            while (elapsed < duration * 0.5f)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / (duration * 0.5f);
                transform.localScale = Vector3.Lerp(startScale, Vector3.Scale(_originalScale, _stretchScale), t);
                yield return null;
            }

            // Phase 2: Land & Squash Down
            elapsed = 0f;
            while (elapsed < duration * 0.5f)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / (duration * 0.5f);
                transform.localScale = Vector3.Lerp(Vector3.Scale(_originalScale, _stretchScale), Vector3.Scale(_originalScale, _squashScale), t);
                yield return null;
            }

            // Phase 3: Snap back to original scale
            elapsed = 0f;
            float returnTime = 0.08f;
            while (elapsed < returnTime)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / returnTime;
                transform.localScale = Vector3.Lerp(Vector3.Scale(_originalScale, _squashScale), _originalScale, t);
                yield return null;
            }

            transform.localScale = _originalScale;
            _wobbleRoutine = null;
        }
    }
}
