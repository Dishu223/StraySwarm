using UnityEngine;
using TMPro; // Unity's built-in beautiful text system

namespace StraySwarm.UI
{
    /// <summary>
    /// Connects our Game logic to what the player sees on screen (the HUD).
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("HUD Elements")]
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameObject _losePanel;

        [Header("Stars UI")]
        [Tooltip("The 3 star GameObjects on the Win Panel")]
        [SerializeField] private GameObject[] _stars = new GameObject[3];

        [Header("Timer Colors")]
        [SerializeField] private Color _normalTimerColor = new Color(0.6f, 0.2f, 0.07f); // Deep Rust (#9A3412)
        [SerializeField] private Color _warningTimerColor = Color.red;

        [Header("Dependencies")]
        [SerializeField] private Core.GameManager _gameManager;

        private void Start()
        {
            if (_gameManager == null) _gameManager = FindAnyObjectByType<Core.GameManager>();
            
            // Hide panels at the start of the game
            if (_winPanel != null) _winPanel.SetActive(false);
            if (_losePanel != null) _losePanel.SetActive(false);
        }

        private void Update()
        {
            if (_gameManager == null) return;

            // 1. Update the Timer UI (Format it nicely as MM:SS)
            if (_timerText != null)
            {
                int minutes = Mathf.FloorToInt(_gameManager.TimeRemaining / 60F);
                int seconds = Mathf.FloorToInt(_gameManager.TimeRemaining - minutes * 60);
                _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                
                // Add a little tension: turn the timer red when under 10 seconds!
                if (_gameManager.TimeRemaining < 10f)
                {
                    _timerText.color = _warningTimerColor;
                }
                else
                {
                    _timerText.color = _normalTimerColor;
                }
            }

            // 2. Show the correct screen if the game ends
            if (_gameManager.CurrentState == Core.GameState.Won && _winPanel != null && !_winPanel.activeSelf)
            {
                StartCoroutine(AnimateWinScreen());
            }
            else if (_gameManager.CurrentState == Core.GameState.Lost && _losePanel != null && !_losePanel.activeSelf)
            {
                _losePanel.SetActive(true);
            }
        }

        /// <summary>
        /// Pure C# Juicy Bouncy Animation (EaseOutBack) for the Win Panel and Stars!
        /// </summary>
        private System.Collections.IEnumerator AnimateWinScreen()
        {
            _winPanel.SetActive(true);
            
            // 1. Bounce the main panel into view (Scale from 0 to 1 with juicy overshoot!)
            Transform panelTransform = _winPanel.transform;
            panelTransform.localScale = Vector3.zero;
            
            float elapsed = 0f;
            float duration = 0.4f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                
                // BackOut Easing Formula (overshoots up to 1.2x scale)
                float overshootT = 1f + 2.70158f * Mathf.Pow(t - 1f, 3f) + 1.70158f * Mathf.Pow(t - 1f, 2f);
                panelTransform.localScale = Vector3.one * Mathf.Clamp(overshootT, 0f, 1.3f);
                yield return null;
            }
            panelTransform.localScale = Vector3.one;

            // 2. Pop each star in one-by-one (POP! POP! POP!)
            for (int i = 0; i < _stars.Length; i++)
            {
                if (_stars[i] != null)
                {
                    bool isEarned = i < _gameManager.StarsEarned;
                    _stars[i].SetActive(isEarned);

                    if (isEarned)
                    {
                        // Play an ascending musical chime for each star!
                        if (Audio.AudioManager.Instance != null)
                        {
                            Audio.AudioManager.Instance.PlayStar(i);
                        }
                        
                        // Bounce scale animation for individual star
                        Transform starT = _stars[i].transform;
                        starT.localScale = Vector3.zero;
                        
                        float starElapsed = 0f;
                        while (starElapsed < 0.25f)
                        {
                            starElapsed += Time.deltaTime;
                            float st = starElapsed / 0.25f;
                            float starOvershoot = 1f + 2.70158f * Mathf.Pow(st - 1f, 3f) + 1.70158f * Mathf.Pow(st - 1f, 2f);
                            starT.localScale = Vector3.one * Mathf.Clamp(starOvershoot, 0f, 1.4f);
                            yield return null;
                        }
                        starT.localScale = Vector3.one;
                        
                        yield return new WaitForSeconds(0.15f); // Short pause before popping next star!
                    }
                }
            }
        }

        // --- BUTTON CLICK HANDLERS ---
        public void OnNextLevelButtonClicked()
        {
            if (Core.LevelManager.Instance != null)
            {
                Core.LevelManager.Instance.LoadNextLevel();
            }
            else
            {
                // Fallback: reload scene
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            }
        }

        public void OnRestartButtonClicked()
        {
            if (Core.LevelManager.Instance != null)
            {
                Core.LevelManager.Instance.RestartCurrentLevel();
            }
            else
            {
                // Fallback: reload scene
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
