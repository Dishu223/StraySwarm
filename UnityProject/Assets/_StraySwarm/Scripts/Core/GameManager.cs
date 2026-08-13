using UnityEngine;
using StraySwarm.Data;
using StraySwarm.Events;
using StraySwarm.Utils;

namespace StraySwarm.Core
{
    public enum GameState { Playing, Won, Lost }

    /// <summary>
    /// Controls the overall flow of the level (timer, winning, losing).
    /// Raises ScriptableObject events and persists progress to SaveManager.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Header("Level Configuration")]
        [SerializeField] private LevelData _currentLevel;
        [SerializeField] private int _currentLevelIndex = 0;

        [Header("Event Channels")]
        [SerializeField] private GameEvent _onLevelWonEvent;
        [SerializeField] private GameEvent _onLevelLostEvent;
        
        public GameState CurrentState { get; private set; } = GameState.Playing;
        public float TimeRemaining { get; private set; }
        public int StarsEarned { get; private set; } = 0;
        
        private void Start()
        {
            // Auto-fetch level data from LevelManager if available
            if (LevelManager.Instance != null && LevelManager.Instance.GetCurrentLevelData() != null)
            {
                _currentLevel = LevelManager.Instance.GetCurrentLevelData();
                _currentLevelIndex = LevelManager.Instance.CurrentLevelIndex;
            }

            if (_currentLevel != null)
            {
                TimeRemaining = _currentLevel.TimeLimit;
            }
            else
            {
                TimeRemaining = 60f; // Safe fallback if we forget to assign LevelData
            }

            // Start playing the level BGM!
            if (Audio.AudioManager.Instance != null)
            {
                Audio.AudioManager.Instance.PlayGameplayMusic();
            }
        }

        private void Update()
        {
            if (CurrentState != GameState.Playing) return;

            // Tick the timer down!
            TimeRemaining -= Time.deltaTime;

            if (TimeRemaining <= 0)
            {
                TimeRemaining = 0;
                LoseGame();
            }
        }

        public void WinGame()
        {
            if (CurrentState != GameState.Playing) return;
            
            CurrentState = GameState.Won;
            StarsEarned = CalculateStars();
            
            Debug.Log($"🎉 YOU WIN! Stars earned: {StarsEarned}");

            // Persist progress to SaveManager
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.SaveLevelStars(_currentLevelIndex, StarsEarned);
                int coinsToAward = _currentLevel != null ? _currentLevel.CoinReward : (StarsEarned * 10);
                SaveManager.Instance.AddCoins(coinsToAward);
            }

            // Raise Event Channel
            if (_onLevelWonEvent != null)
            {
                _onLevelWonEvent.Raise();
            }
            
            if (Gameplay.JuiceManager.Instance != null)
            {
                Gameplay.JuiceManager.Instance.PlayWinConfetti();
            }

            if (Audio.AudioManager.Instance != null)
            {
                Audio.AudioManager.Instance.PlayWin();
            }
        }

        private void LoseGame()
        {
            if (CurrentState != GameState.Playing) return;
            
            CurrentState = GameState.Lost;
            Debug.Log("💀 TIME'S UP! YOU LOSE!");

            // Raise Event Channel
            if (_onLevelLostEvent != null)
            {
                _onLevelLostEvent.Raise();
            }
            
            if (Audio.AudioManager.Instance != null)
            {
                Audio.AudioManager.Instance.PlayLose();
            }
        }

        private int CalculateStars()
        {
            if (_currentLevel == null) return 1;

            float ratio = _currentLevel.TimeLimit > 0 ? (TimeRemaining / _currentLevel.TimeLimit) : 0.5f;

            if (ratio >= _currentLevel.ThreeStarPercentage) return 3;
            if (ratio >= _currentLevel.TwoStarPercentage) return 2;
            return 1;
        }
    }
}
