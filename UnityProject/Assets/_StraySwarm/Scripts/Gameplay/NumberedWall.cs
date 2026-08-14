using System.Collections;
using UnityEngine;
using TMPro;
using StraySwarm.Core;

namespace StraySwarm.Gameplay
{
    /// <summary>
    /// Breakable stone wall that counts down each time the player hits/passes it.
    /// Shatters and clears the path when its count reaches 0!
    /// </summary>
    public class NumberedWall : MonoBehaviour
    {
        [Header("Wall Configuration")]
        [SerializeField] private int _hitPoints = 3;
        [SerializeField] private TextMeshPro _numberText;
        [SerializeField] private SpriteRenderer _wallRenderer;
        [SerializeField] private Collider2D _wallCollider;

        [Header("Audio & Effects")]
        [SerializeField] private AudioClip _hitSound;
        [SerializeField] private AudioClip _breakSound;
        [SerializeField] private ParticleSystem _crumbleParticles;

        public bool IsBroken => _hitPoints <= 0;

        private void Start()
        {
            UpdateVisuals();
        }

        public void HitWall()
        {
            if (IsBroken) return;

            _hitPoints--;
            UpdateVisuals();

            // Squash bounce animation
            StartCoroutine(PunchScaleRoutine());

            // Play sound
            if (_hitPoints > 0)
            {
                if (Audio.AudioManager.Instance != null)
                {
                    Audio.AudioManager.Instance.PlaySound(_hitSound != null ? _hitSound : Audio.AudioManager.Instance.WallBreakSound);
                }
            }
            else
            {
                BreakWall();
            }
        }

        private void BreakWall()
        {
            if (_wallCollider != null) _wallCollider.enabled = false;
            if (_numberText != null) _numberText.gameObject.SetActive(false);

            if (_crumbleParticles != null)
            {
                _crumbleParticles.Play();
            }

            if (Audio.AudioManager.Instance != null)
            {
                Audio.AudioManager.Instance.PlaySound(_breakSound != null ? _breakSound : Audio.AudioManager.Instance.WallBreakSound);
            }

            if (CameraShake.Instance != null)
            {
                CameraShake.Instance.Shake(0.15f, 0.1f);
            }

            // Fade out renderer or disable
            StartCoroutine(FadeAndDestroyRoutine());
        }

        private void UpdateVisuals()
        {
            if (_numberText != null)
            {
                _numberText.text = _hitPoints.ToString();
            }
        }

        private IEnumerator PunchScaleRoutine()
        {
            Vector3 original = transform.localScale;
            transform.localScale = original * 1.25f;
            float elapsed = 0f;

            while (elapsed < 0.15f)
            {
                elapsed += Time.deltaTime;
                transform.localScale = Vector3.Lerp(original * 1.25f, original, elapsed / 0.15f);
                yield return null;
            }
            transform.localScale = original;
        }

        private IEnumerator FadeAndDestroyRoutine()
        {
            yield return new WaitForSeconds(0.1f);
            if (_wallRenderer != null) _wallRenderer.enabled = false;
            yield return new WaitForSeconds(1.0f); // Allow particles to finish
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") || other.GetComponent<PlayerController>() != null)
            {
                HitWall();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.GetComponent<PlayerController>() != null)
            {
                HitWall();
            }
        }
    }
}
