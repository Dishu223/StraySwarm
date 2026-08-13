using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace StraySwarm.UI
{
    /// <summary>
    /// Controls the 1_MainMenu scene interface, mascot animations, and navigation.
    /// </summary>
    public class MainMenuUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI _coinText;
        [SerializeField] private GameObject _settingsPanel;
        [SerializeField] private GameObject _mascotCat;

        [Header("Scene Navigation")]
        [SerializeField] private string _levelSelectScene = "2_LevelSelect";
        [SerializeField] private string _gameplayScene = "3_Gameplay";

        private void Start()
        {
            // Start Menu BGM
            if (Audio.AudioManager.Instance != null)
            {
                Audio.AudioManager.Instance.PlayMenuMusic();
            }

            // Hide settings panel on start
            if (_settingsPanel != null)
            {
                _settingsPanel.SetActive(false);
            }

            UpdateCoinDisplay();
        }

        public void UpdateCoinDisplay()
        {
            if (_coinText != null)
            {
                int coins = Utils.SaveManager.Instance != null ? Utils.SaveManager.Instance.Data.TotalCoins : 0;
                _coinText.text = coins.ToString();
            }
        }

        // --- BUTTON CLICKS ---
        public void OnPlayButtonClicked()
        {
            if (Audio.AudioManager.Instance != null)
            {
                Audio.AudioManager.Instance.PlayButtonClick();
            }

            // Load LevelSelect if scene exists, otherwise load Gameplay directly
            if (Application.CanStreamedLevelBeLoaded(_levelSelectScene))
            {
                SceneManager.LoadScene(_levelSelectScene);
            }
            else
            {
                SceneManager.LoadScene(_gameplayScene);
            }
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

        public void OnCloseSettingsClicked()
        {
            if (Audio.AudioManager.Instance != null)
            {
                Audio.AudioManager.Instance.PlayButtonClick();
            }

            if (_settingsPanel != null)
            {
                _settingsPanel.SetActive(false);
            }
        }
    }
}
