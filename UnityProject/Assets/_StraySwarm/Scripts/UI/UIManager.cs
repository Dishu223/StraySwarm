using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StraySwarm.Core;
using StraySwarm.Gameplay;

namespace StraySwarm.UI
{
    /// <summary>
    /// Connects our Game logic to what the player sees on screen (the HUD).
    /// Displays Timer, Level Title, Rescued Quota, and Current Van Demand HUD indicators.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("HUD Elements")]
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private TextMeshProUGUI _levelTitleText;
        [SerializeField] private TextMeshProUGUI _quotaText;
        [SerializeField] private TextMeshProUGUI _vanStatusText;
        [SerializeField] private TextMeshProUGUI _coinText;

        [Header("Panels")]
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameObject _losePanel;
        [SerializeField] private GameObject _pausePanel;

        [Header("Stars UI")]
        [Tooltip("The 3 star GameObjects on the Win Panel")]
        [SerializeField] private GameObject[] _stars = new GameObject[3];

        [Header("Timer Colors")]
        [SerializeField] private Color _normalTimerColor = new Color(0.6f, 0.2f, 0.07f); // Deep Rust (#9A3412)
        [SerializeField] private Color _warningTimerColor = Color.red;

        [Header("Dependencies")]
        [SerializeField] private GameManager _gameManager;

        private bool _isPaused = false;

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        private void Start()
        {
            if (_gameManager == null) _gameManager = FindAnyObjectByType<GameManager>();
            
            // Hide panels at start of the game
            if (_winPanel != null) _winPanel.SetActive(false);
            if (_losePanel != null) _losePanel.SetActive(false);
            if (_pausePanel != null) _pausePanel.SetActive(false);

            UpdateLevelTitle();
        }

        private void UpdateLevelTitle()
        {
            if (_levelTitleText != null)
            {
                int currentLvl = LevelManager.Instance != null ? LevelManager.Instance.CurrentLevelIndex + 1 : 1;
                int currentWorld = LevelManager.Instance != null ? LevelManager.Instance.CurrentWorldIndex + 1 : 1;
                _levelTitleText.text = $"LEVEL {currentWorld}-{currentLvl}";
            }
        }

        private void Update()
        {
            if (_gameManager == null) return;

            // 1. Update Timer HUD
            if (_timerText != null)
            {
                int minutes = Mathf.FloorToInt(_gameManager.TimeRemaining / 60F);
                int seconds = Mathf.FloorToInt(_gameManager.TimeRemaining - minutes * 60);
                _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                
                if (_gameManager.TimeRemaining < 10f)
                {
                    _timerText.color = _warningTimerColor;
                }
                else
                {
                    _timerText.color = _normalTimerColor;
                }
            }

            // 2. Update Rescued Quota HUD
            if (_quotaText != null && WaveSpawner.Instance != null)
            {
                _quotaText.text = $"🐾 {WaveSpawner.Instance.TotalDelivered} / {WaveSpawner.Instance.TotalQuota}";
            }

            // 3. Update Current Van Target HUD Indicator
            if (_vanStatusText != null)
            {
                VanQueue vq = FindAnyObjectByType<VanQueue>();
                if (vq != null)
                {
                    VanController currentVan = vq.GetCurrentVan();
                    if (currentVan != null && currentVan.IsParked && !currentVan.IsDrivingAway)
                    {
                        string speciesEmoji = GetSpeciesEmoji(currentVan.TargetAnimalType);
                        _vanStatusText.text = $"🚐 Van: {speciesEmoji} {currentVan.TargetAnimalType}";
                    }
                    else if (currentVan != null && !currentVan.IsParked)
                    {
                        _vanStatusText.text = "🚐 Van Arriving...";
                    }
                    else
                    {
                        _vanStatusText.text = "🚐 Waiting for Van...";
                    }
                }
            }

            // 4. Update Coin Balance HUD
            if (_coinText != null)
            {
                int coins = Utils.SaveManager.Instance != null ? Utils.SaveManager.Instance.GetCoins() : 0;
                _coinText.text = $"🪙 {coins}";
            }

            // 5. Show Win/Lose screens when state changes
            if (_gameManager.CurrentState == GameState.Won && _winPanel != null && !_winPanel.activeSelf)
            {
                StartCoroutine(AnimateWinScreen());
            }
            else if (_gameManager.CurrentState == GameState.Lost && _losePanel != null && !_losePanel.activeSelf)
            {
                _losePanel.SetActive(true);
            }
        }

        private string GetSpeciesEmoji(Data.AnimalType type)
        {
            switch (type)
            {
                case Data.AnimalType.Puppy:  return "🐶";
                case Data.AnimalType.Kitten: return "🐱";
                case Data.AnimalType.Frog:   return "🐸";
                case Data.AnimalType.Mouse:  return "🐹";
                case Data.AnimalType.Pigeon: return "🐦";
                case Data.AnimalType.Bunny:  return "🐰";
                default: return "🐾";
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
                
                // BackOut Easing Formula
                float overshootT = 1f + 2.70158f * Mathf.Pow(t - 1f, 3f) + 1.70158f * Mathf.Pow(t - 1f, 2f);
                panelTransform.localScale = Vector3.one * Mathf.Clamp(overshootT, 0f, 1.3f);
                yield return null;
            }
            panelTransform.localScale = Vector3.one;

            // 2. Pop each star in one-by-one
            for (int i = 0; i < _stars.Length; i++)
            {
                if (_stars[i] != null)
                {
                    bool isEarned = i < _gameManager.StarsEarned;
                    _stars[i].SetActive(isEarned);

                    if (isEarned)
                    {
                        if (Audio.AudioManager.Instance != null)
                        {
                            Audio.AudioManager.Instance.PlayStar(i);
                        }
                        
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
                        
                        yield return new WaitForSeconds(0.15f);
                    }
                }
            }
        }

        // --- BUTTON CLICK HANDLERS ---
        public void TogglePause()
        {
            _isPaused = !_isPaused;
            Time.timeScale = _isPaused ? 0f : 1f;
            if (_pausePanel != null) _pausePanel.SetActive(_isPaused);
        }

        public void OnResumeButtonClicked()
        {
            _isPaused = false;
            Time.timeScale = 1f;
            if (_pausePanel != null) _pausePanel.SetActive(false);
        }

        public void OnNextLevelButtonClicked()
        {
            Time.timeScale = 1f;
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.LoadNextLevel();
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            }
        }

        public void OnRestartButtonClicked()
        {
            Time.timeScale = 1f;
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.RestartCurrentLevel();
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            }
        }

        public void OnMuteToggleClicked()
        {
            if (Audio.AudioManager.Instance != null)
            {
                Audio.AudioManager.Instance.ToggleMute();
            }
        }
    }
}
