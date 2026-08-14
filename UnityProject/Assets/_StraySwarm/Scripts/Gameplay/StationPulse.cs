using System.Collections;
using UnityEngine;

namespace StraySwarm.Gameplay
{
    /// <summary>
    /// Code-driven breathing pulse and celebration bounce for Rescue Stations.
    /// Provides tactile visual feedback when animals are received or crates are completed.
    /// </summary>
    public class StationPulse : MonoBehaviour
    {
        [Header("Idle Breathing Pulse")]
        [SerializeField] private bool _enableIdlePulse = true;
        [SerializeField] private float _pulseSpeed = 2f;
        [SerializeField] private float _pulseAmount = 0.03f;

        private Vector3 _originalScale;
        private Coroutine _bounceRoutine;

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        private void Update()
        {
            if (_enableIdlePulse && _bounceRoutine == null)
            {
                float scaleOffset = Mathf.Sin(Time.time * _pulseSpeed) * _pulseAmount;
                transform.localScale = _originalScale + new Vector3(scaleOffset, scaleOffset, 0f);
            }
        }

        /// <summary>
        /// Call this when an animal arrives at the station for a juicy squash & stretch gulp!
        /// </summary>
        public void TriggerReceivePop()
        {
            if (_bounceRoutine != null) StopCoroutine(_bounceRoutine);
            _bounceRoutine = StartCoroutine(ReceivePopRoutine());
        }

        private IEnumerator ReceivePopRoutine()
        {
            Vector3 popScale = new Vector3(_originalScale.x * 1.15f, _originalScale.y * 0.88f, _originalScale.z);
            float elapsed = 0f;
            float duration = 0.15f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                transform.localScale = Vector3.Lerp(_originalScale, popScale, Mathf.Sin(t * Mathf.PI));
                yield return null;
            }

            transform.localScale = _originalScale;
            _bounceRoutine = null;
        }
    }
}
