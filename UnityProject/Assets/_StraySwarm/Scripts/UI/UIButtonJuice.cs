using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StraySwarm.UI
{
    /// <summary>
    /// Attach to any UI Button to give it a juicy 3D press effect and spring bounce!
    /// </summary>
    public class UIButtonJuice : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float _pressScale = 0.92f;
        [SerializeField] private float _bounceOvershoot = 1.08f;
        [SerializeField] private float _animationDuration = 0.15f;

        private Vector3 _originalScale;
        private Coroutine _bounceRoutine;

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_bounceRoutine != null) StopCoroutine(_bounceRoutine);
            transform.localScale = _originalScale * _pressScale;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_bounceRoutine != null) StopCoroutine(_bounceRoutine);
            _bounceRoutine = StartCoroutine(BounceBackRoutine());
        }

        private IEnumerator BounceBackRoutine()
        {
            float elapsed = 0f;
            Vector3 start = transform.localScale;
            Vector3 peak = _originalScale * _bounceOvershoot;

            // Pop up to overshoot
            while (elapsed < _animationDuration * 0.6f)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / (_animationDuration * 0.6f);
                transform.localScale = Vector3.Lerp(start, peak, t);
                yield return null;
            }

            // Settle back to original scale
            elapsed = 0f;
            while (elapsed < _animationDuration * 0.4f)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / (_animationDuration * 0.4f);
                transform.localScale = Vector3.Lerp(peak, _originalScale, t);
                yield return null;
            }

            transform.localScale = _originalScale;
            _bounceRoutine = null;
        }
    }
}
