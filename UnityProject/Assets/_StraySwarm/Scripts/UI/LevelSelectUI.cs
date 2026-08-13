using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace StraySwarm.UI
{
    /// <summary>
    /// Manages the 2_LevelSelect World Map screen.
    /// Dynamically displays level unlock states and earned stars from SaveManager.
    /// </summary>
    public class LevelSelectUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI _totalStarsText;
        [SerializeField] private TextMeshProUGUI _worldTitleText;

        [Header("Level Buttons")]
        [SerializeField] private List<Button> _levelButtons = new List<Button>();

        [Header("Navigation")]
        [SerializeField] private string _mainMenuScene = "1_MainMenu";
        [SerializeField] private string _gameplayScene = "3_Gameplay";

        private int _selectedWorld = 0;

        private void Start()
        {
            RefreshUI();
        }

        public void RefreshUI()
        {
            // 1. Update total stars counter
            if (_totalStarsText != null && Utils.SaveManager.Instance != null)
            {
                _totalStarsText.text = Utils.SaveManager.Instance.GetTotalStars().ToString();
            }

            // 2. Refresh level button grid
            int unlockedLevel = Utils.SaveManager.Instance != null ? Utils.SaveManager.Instance.Data.UnlockedLevel : 1;

            for (int i = 0; i < _levelButtons.Count; i++)
            {
                int levelNum = i + 1;
                Button btn = _levelButtons[i];

                if (btn != null)
                {
                    bool isUnlocked = levelNum <= unlockedLevel;
                    btn.interactable = isUnlocked;

                    // Click handler
                    btn.onClick.RemoveAllListeners();
                    if (isUnlocked)
                    {
                        btn.onClick.AddListener(() => OnLevelSelected(levelNum));
                    }
                }
            }
        }

        public void OnLevelSelected(int levelIndex)
        {
            if (Audio.AudioManager.Instance != null)
            {
                Audio.AudioManager.Instance.PlayButtonClick();
            }

            // Tell LevelManager which exact level was chosen!
            if (Core.LevelManager.Instance != null)
            {
                Core.LevelManager.Instance.SelectLevel(levelIndex - 1);
            }

            // Load gameplay scene with fallback
            if (Application.CanStreamedLevelBeLoaded(_gameplayScene))
            {
                SceneManager.LoadScene(_gameplayScene);
            }
            else if (Application.CanStreamedLevelBeLoaded("SampleScene"))
            {
                SceneManager.LoadScene("SampleScene");
            }
        }

        public void OnBackButtonClicked()
        {
            if (Audio.AudioManager.Instance != null)
            {
                Audio.AudioManager.Instance.PlayButtonClick();
            }

            SceneManager.LoadScene(_mainMenuScene);
        }
    }
}
