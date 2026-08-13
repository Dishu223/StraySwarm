using UnityEngine;
using UnityEngine.SceneManagement;

namespace StraySwarm.UI
{
    /// <summary>
    /// In-game Pause Menu overlay for 3_Gameplay.
    /// Handles Pausing Time, Resuming, Restarting, and Returning to Main Menu.
    /// </summary>
    public class PauseMenuUI : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private GameObject _settingsPanel;

        [Header("Scene Names")]
        [SerializeField] private string _mainMenuScene = "1_MainMenu";

        public bool IsPaused { get; private set; } = false;

        private void Start()
        {
            if (_pausePanel != null) _pausePanel.SetActive(false);
            if (_settingsPanel != null) _settingsPanel.SetActive(false);
        }

        public void PauseGame()
        {
            IsPaused = true;
            Time.timeScale = 0f; // Freeze game physics and timer

            if (_pausePanel != null) _pausePanel.SetActive(true);

            if (Audio.AudioManager.Instance != null)
            {
                Audio.AudioManager.Instance.PlayButtonClick();
            }
        }

        public void ResumeGame()
        {
            IsPaused = false;
            Time.timeScale = 1f; // Unfreeze time

            if (_pausePanel != null) _pausePanel.SetActive(false);
            if (_settingsPanel != null) _settingsPanel.SetActive(false);

            if (Audio.AudioManager.Instance != null)
            {
                Audio.AudioManager.Instance.PlayButtonClick();
            }
        }

        public void OnRestartButtonClicked()
        {
            Time.timeScale = 1f;

            if (Audio.AudioManager.Instance != null)
            {
                Audio.AudioManager.Instance.PlayButtonClick();
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void OnSettingsButtonClicked()
        {
            if (Audio.AudioManager.Instance != null)
            {
                Audio.AudioManager.Instance.PlayButtonClick();
            }

            if (_settingsPanel != null)
            {
                _settingsPanel.SetActive(true);
            }
        }

        public void OnMainMenuButtonClicked()
        {
            Time.timeScale = 1f;

            if (Audio.AudioManager.Instance != null)
            {
                Audio.AudioManager.Instance.PlayButtonClick();
            }

            SceneManager.LoadScene(_mainMenuScene);
        }
    }
}
