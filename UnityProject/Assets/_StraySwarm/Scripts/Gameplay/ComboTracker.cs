using System.Collections;
using UnityEngine;
using TMPro;

namespace StraySwarm.Gameplay
{
    /// <summary>
    /// Tracks rapid-succession animal collections and displays floating combo callouts.
    /// </summary>
    public class ComboTracker : MonoBehaviour
    {
        public static ComboTracker Instance { get; private set; }

        [Header("Combo Timing")]
        [SerializeField] private float _comboWindow = 2.0f;
        [SerializeField] private TextMeshProUGUI _comboText;

        public int CurrentCombo { get; private set; } = 0;
        private float _lastCollectTime = -10f;
        private Coroutine _comboDisplayRoutine;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            if (_comboText != null) _comboText.gameObject.SetActive(false);
        }

        public void RegisterAnimalPickup(Vector3 worldPos)
        {
            if (Time.time - _lastCollectTime <= _comboWindow)
            {
                CurrentCombo++;
            }
            else
            {
                CurrentCombo = 1;
            }

            _lastCollectTime = Time.time;

            if (CurrentCombo >= 2)
            {
                ShowCombo(CurrentCombo);
            }
        }

        private void ShowCombo(int count)
        {
            if (_comboText == null) return;

            string message = count switch
            {
                2 => "NICE! x2",
                3 => "SWEET! x3",
                4 => "GREAT! x4",
                5 => "SWARM! x5",
                _ => $"SUPER x{count}!"
            };

            _comboText.text = message;
            _comboText.gameObject.SetActive(true);

            if (_comboDisplayRoutine != null) StopCoroutine(_comboDisplayRoutine);
            _comboDisplayRoutine = StartCoroutine(ComboAnimRoutine());
        }

        private IEnumerator ComboAnimRoutine()
        {
            Transform t = _comboText.transform;
            t.localScale = Vector3.zero;

            // Pop in
            float elapsed = 0f;
            while (elapsed < 0.15f)
            {
                elapsed += Time.deltaTime;
                t.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 1.3f, elapsed / 0.15f);
                yield return null;
            }

            // Settle to 1.0
            elapsed = 0f;
            while (elapsed < 0.1f)
            {
                elapsed += Time.deltaTime;
                t.localScale = Vector3.Lerp(Vector3.one * 1.3f, Vector3.one, elapsed / 0.1f);
                yield return null;
            }

            yield return new WaitForSeconds(0.6f);

            // Fade/shrink out
            elapsed = 0f;
            while (elapsed < 0.15f)
            {
                elapsed += Time.deltaTime;
                t.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, elapsed / 0.15f);
                yield return null;
            }

            _comboText.gameObject.SetActive(false);
            _comboDisplayRoutine = null;
        }
    }
}
