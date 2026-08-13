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
        [Header("Level Buttons")]
        [SerializeField] private List<Button> _levelButtons = new List<Button>();
        [SerializeField] private List<GameObject[]> _levelStarIcons = new List<GameObject[]>();

        [Header("Navigation")]
        [SerializeField] private string _mainMenuScene = "1_MainMenu";
        [SerializeField] private string _gameplayScene = "3_Gameplay";

        private void Start()
        {
            RefreshLevelGrid();
        }

        public void RefreshLevelGrid()
        {
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

            // In our full game, load level index into LevelManager or load gameplay scene
            SceneManager.LoadScene(_gameplayScene);
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
