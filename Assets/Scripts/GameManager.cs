using System;
using UnityEngine;
using SoulBound.Core;

namespace SoulBound
{
    /// <summary>
    /// Central game state and progression controller for SoulBound RPG
    /// Manages overall game flow, act transitions, and global game state
    /// Registered with ServiceLocator during bootstrap initialization
    /// </summary>
    public class GameManager : BaseManager
    {
        [Header("Game State")]
        [SerializeField] private GameState _currentGameState = GameState.MainMenu;
        [SerializeField] private int _currentAct = 1;
        [SerializeField] private string _currentLevel = "";

        [Header("Game Settings")]
        [SerializeField] private bool _pauseGameOnFocusLoss = true;
        [SerializeField] private float _timeScale = 1.0f;

        // Events for game state changes
        public static event Action<GameState, GameState> OnGameStateChanged;
        public static event Action<int> OnActChanged;
        public static event Action<bool> OnGamePaused;

        // Properties
        public GameState CurrentGameState => _currentGameState;
        public int CurrentAct => _currentAct;
        public string CurrentLevel => _currentLevel;
        public bool IsPaused { get; private set; }

        protected override void OnInitialize()
        {
            LogInfo("Setting up game state management");
            
            // Initialize time scale
            Time.timeScale = _timeScale;
            
            // Set initial game state
            SetGameState(GameState.MainMenu);
            
            // Subscribe to application focus events
            Application.focusChanged += OnApplicationFocusChanged;
        }

        protected override void OnCleanup()
        {
            // Unsubscribe from events
            Application.focusChanged -= OnApplicationFocusChanged;
            
            // Clear static events
            OnGameStateChanged = null;
            OnActChanged = null;
            OnGamePaused = null;
        }

        #region Game State Management

        /// <summary>
        /// Change the current game state
        /// </summary>
        /// <param name="newState">New game state to transition to</param>
        public void SetGameState(GameState newState)
        {
            if (_currentGameState == newState)
            {
                LogWarning($"Already in game state: {newState}");
                return;
            }

            GameState previousState = _currentGameState;
            _currentGameState = newState;

            LogInfo($"Game state changed: {previousState} → {newState}");

            // Handle state-specific logic
            OnGameStateEnter(newState, previousState);

            // Notify listeners
            OnGameStateChanged?.Invoke(newState, previousState);
        }

        /// <summary>
        /// Handle logic when entering a new game state
        /// </summary>
        /// <param name="newState">The state being entered</param>
        /// <param name="previousState">The previous state</param>
        private void OnGameStateEnter(GameState newState, GameState previousState)
        {
            switch (newState)
            {
                case GameState.MainMenu:
                    Time.timeScale = 1.0f;
                    IsPaused = false;
                    break;

                case GameState.Playing:
                    Time.timeScale = _timeScale;
                    IsPaused = false;
                    break;

                case GameState.Paused:
                    Time.timeScale = 0f;
                    IsPaused = true;
                    break;

                case GameState.GameOver:
                    Time.timeScale = 0f;
                    break;

                case GameState.Loading:
                    // Keep current time scale during loading
                    break;
            }
        }

        #endregion

        #region Act and Level Management

        /// <summary>
        /// Progress to the next act
        /// </summary>
        public void AdvanceToNextAct()
        {
            int previousAct = _currentAct;
            _currentAct++;

            LogInfo($"Advanced to Act {_currentAct}");
            OnActChanged?.Invoke(_currentAct);
        }

        /// <summary>
        /// Set the current act directly
        /// </summary>
        /// <param name="actNumber">Act number to set</param>
        public void SetCurrentAct(int actNumber)
        {
            if (actNumber < 1)
            {
                LogError($"Invalid act number: {actNumber}. Acts start from 1.");
                return;
            }

            _currentAct = actNumber;
            LogInfo($"Current act set to: {_currentAct}");
            OnActChanged?.Invoke(_currentAct);
        }

        /// <summary>
        /// Set the current level identifier
        /// </summary>
        /// <param name="levelName">Name/identifier of the current level</param>
        public void SetCurrentLevel(string levelName)
        {
            _currentLevel = levelName;
            LogInfo($"Current level set to: {levelName}");
        }

        #endregion

        #region Pause Management

        /// <summary>
        /// Pause or unpause the game
        /// </summary>
        /// <param name="pauseState">True to pause, false to unpause</param>
        public void SetPauseState(bool pauseState)
        {
            if (pauseState)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        /// <summary>
        /// Pause the game
        /// </summary>
        public void PauseGame()
        {
            if (IsPaused)
            {
                LogWarning("Game is already paused");
                return;
            }

            LogInfo("Pausing game");
            SetGameState(GameState.Paused);
            OnGamePaused?.Invoke(true);
        }

        /// <summary>
        /// Resume the game from pause
        /// </summary>
        public void ResumeGame()
        {
            if (!IsPaused)
            {
                LogWarning("Game is not paused");
                return;
            }

            LogInfo("Resuming game");
            SetGameState(GameState.Playing);
            OnGamePaused?.Invoke(false);
        }

        /// <summary>
        /// Toggle pause state
        /// </summary>
        public void TogglePause()
        {
            SetPauseState(!IsPaused);
        }

        #endregion

        #region Application Events

        /// <summary>
        /// Handle application focus changes
        /// </summary>
        /// <param name="hasFocus">True if application has focus</param>
        private void OnApplicationFocusChanged(bool hasFocus)
        {
            if (!_pauseGameOnFocusLoss)
                return;

            if (!hasFocus && _currentGameState == GameState.Playing)
            {
                LogInfo("Application lost focus - auto-pausing game");
                PauseGame();
            }
        }

        #endregion

        #region Debug and Utilities

        protected override void AppendCustomDebugInfo(System.Text.StringBuilder info)
        {
            info.AppendLine($"Current State: {_currentGameState}");
            info.AppendLine($"Current Act: {_currentAct}");
            info.AppendLine($"Current Level: {_currentLevel}");
            info.AppendLine($"Is Paused: {IsPaused}");
            info.AppendLine($"Time Scale: {Time.timeScale}");
            info.AppendLine($"Pause on Focus Loss: {_pauseGameOnFocusLoss}");
        }

        /// <summary>
        /// Get current game status summary
        /// </summary>
        /// <returns>Game status string</returns>
        public string GetGameStatus()
        {
            return $"State: {_currentGameState} | Act: {_currentAct} | Level: {_currentLevel} | Paused: {IsPaused}";
        }

        #endregion
    }

    /// <summary>
    /// Enum representing different game states
    /// </summary>
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        Loading,
        GameOver,
        Settings,
        Inventory,
        Dialogue
    }
} 