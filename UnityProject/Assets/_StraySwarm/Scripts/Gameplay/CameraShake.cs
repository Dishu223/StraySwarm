using System.Collections;
using UnityEngine;

namespace StraySwarm.Gameplay
{
    /// <summary>
    /// Lightweight 2D Camera Shake for adding tactile impact to game actions.
    /// </summary>
    public class CameraShake : MonoBehaviour
    {
        public static CameraShake Instance { get; private set; }

        private Vector3 _originalPos;
        private bool _isShaking = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        /// <summary>
        /// Shakes the camera for a brief duration with a given intensity.
        /// </summary>
        public void Shake(float duration = 0.12f, float magnitude = 0.08f)
        {
            if (!_isShaking)
            {
                StartCoroutine(ShakeRoutine(duration, magnitude));
            }
        }

        private IEnumerator ShakeRoutine(float duration, float magnitude)
        {
            _isShaking = true;
            _originalPos = transform.localPosition;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float offsetX = Random.Range(-1f, 1f) * magnitude;
                float offsetY = Random.Range(-1f, 1f) * magnitude;

                transform.localPosition = new Vector3(_originalPos.x + offsetX, _originalPos.y + offsetY, _originalPos.z);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = _originalPos;
            _isShaking = false;
        }
    }
}
