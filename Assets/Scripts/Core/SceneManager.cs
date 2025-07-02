using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SoulBound; // Add SoulBound namespace to access GameState enum

namespace SoulBound.Core
{
    /// <summary>
    /// Game Scene Management system for SoulBound RPG
    /// Handles scene loading, transitions, and game state persistence
    /// Note: Renamed from SceneManager to avoid conflict with Unity's SceneManager
    /// </summary>
    public class GameSceneManager : BaseManager
    {
        [Header("Scene Loading")]
        [SerializeField] private bool _enableLoadingScreen = true;
        [SerializeField] private float _minimumLoadingTime = 1f;
        [SerializeField] private bool _preloadAdditiveScenes = false;

        [Header("Scene Transition")]
        [SerializeField] private float _fadeInDuration = 0.5f;
        [SerializeField] private float _fadeOutDuration = 0.5f;
        [SerializeField] private bool _persistPlayerData = true;

        // Events
        public static event Action<string> OnSceneLoadStarted;
        public static event Action<string> OnSceneLoadCompleted;
        public static event Action<float> OnSceneLoadProgress;
        public static event Action<string> OnSceneLoadFailed;

        // Scene state
        private string _currentSceneName;
        private string _previousSceneName;
        private bool _isLoading;
        private List<string> _additiveScenes = new List<string>();
        private Dictionary<string, object> _persistentData = new Dictionary<string, object>();

        // Properties
        public string CurrentSceneName => _currentSceneName;
        public string PreviousSceneName => _previousSceneName;
        public bool IsLoading => _isLoading;
        public List<string> AdditiveScenes => new List<string>(_additiveScenes);

        protected override void OnInitialize()
        {
            LogInfo("Setting up scene management");
            
            // Get current scene
            _currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            
            // Subscribe to Unity scene events
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnUnitySceneLoaded;
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded += OnUnitySceneUnloaded;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnActiveSceneChanged;
            
            LogInfo($"Scene manager initialized. Current scene: {_currentSceneName}");
        }

        protected override void OnCleanup()
        {
            // Unsubscribe from Unity scene events
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnUnitySceneLoaded;
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= OnUnitySceneUnloaded;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            
            // Clear events
            OnSceneLoadStarted = null;
            OnSceneLoadCompleted = null;
            OnSceneLoadProgress = null;
            OnSceneLoadFailed = null;
            
            // Clear persistent data
            _persistentData.Clear();
        }

        #region Scene Loading

        /// <summary>
        /// Load a scene by name
        /// </summary>
        /// <param name="sceneName">Name of scene to load</param>
        /// <param name="loadMode">How to load the scene (Single or Additive)</param>
        /// <param name="showLoadingScreen">Whether to show loading screen</param>
        public void LoadScene(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single, bool showLoadingScreen = true)
        {
            if (_isLoading)
            {
                LogWarning($"Already loading a scene. Ignoring request to load: {sceneName}");
                return;
            }

            if (string.IsNullOrEmpty(sceneName))
            {
                LogError("Scene name cannot be null or empty");
                OnSceneLoadFailed?.Invoke("Invalid scene name");
                return;
            }

            LogInfo($"Loading scene: {sceneName} (Mode: {loadMode})");
            StartCoroutine(LoadSceneAsync(sceneName, loadMode, showLoadingScreen));
        }

        /// <summary>
        /// Load scene asynchronously with progress tracking and error handling
        /// </summary>
        /// <param name="sceneName">Scene to load</param>
        /// <param name="loadMode">Load mode (single or additive)</param>
        /// <param name="showLoadingScreen">Whether to show loading screen</param>
        /// <returns>Coroutine for async loading</returns>
        private IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode loadMode, bool showLoadingScreen)
        {
            float startTime = Time.time;
            _isLoading = true;

            // Validate scene name
            if (string.IsNullOrEmpty(sceneName))
            {
                LogError("Scene name cannot be null or empty");
                OnSceneLoadFailed?.Invoke("Scene name cannot be null or empty");
                _isLoading = false;
                yield break;
            }

            LogInfo($"Loading scene: {sceneName} (Mode: {loadMode})");
            OnSceneLoadStarted?.Invoke(sceneName);

            // Store previous scene name
            if (loadMode == LoadSceneMode.Single)
            {
                _previousSceneName = _currentSceneName;
            }

            // Show loading screen if enabled
            if (showLoadingScreen && _enableLoadingScreen)
            {
                yield return StartCoroutine(ShowLoadingScreen());
            }

            // Persist data if needed
            if (_persistPlayerData && loadMode == LoadSceneMode.Single)
            {
                if (!TryPersistCurrentSceneData())
                {
                    LogWarning("Failed to persist scene data, but continuing with load");
                }
            }

            // Start async scene loading
            AsyncOperation asyncLoad = null;
            
            if (!TryStartSceneLoad(sceneName, loadMode, out asyncLoad))
            {
                _isLoading = false;
                yield break;
            }

            // Track loading progress
            while (asyncLoad.progress < 0.9f)
            {
                float progress = asyncLoad.progress / 0.9f;
                OnSceneLoadProgress?.Invoke(progress);
                yield return null;
            }

            // Ensure minimum loading time for UX
            float loadTime = Time.time - startTime;
            if (loadTime < _minimumLoadingTime)
            {
                yield return new WaitForSeconds(_minimumLoadingTime - loadTime);
            }

            // Activate the scene
            asyncLoad.allowSceneActivation = true;

            // Wait for scene to fully load
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            // Update scene tracking
            if (loadMode == LoadSceneMode.Single)
            {
                _currentSceneName = sceneName;
                _additiveScenes.Clear();
            }
            else
            {
                _additiveScenes.Add(sceneName);
            }

            // Hide loading screen
            if (showLoadingScreen && _enableLoadingScreen)
            {
                yield return StartCoroutine(HideLoadingScreen());
            }

            // Restore persistent data
            if (_persistPlayerData && loadMode == LoadSceneMode.Single)
            {
                if (!TryRestoreSceneData())
                {
                    LogWarning("Failed to restore scene data, but scene loaded successfully");
                }
            }

            LogInfo($"Scene loaded successfully: {sceneName}");
            OnSceneLoadCompleted?.Invoke(sceneName);
            _isLoading = false;
        }

        /// <summary>
        /// Safely start scene loading with error handling
        /// </summary>
        /// <param name="sceneName">Scene name to load</param>
        /// <param name="loadMode">Load mode</param>
        /// <param name="asyncLoad">Output async operation</param>
        /// <returns>True if successful, false if failed</returns>
        private bool TryStartSceneLoad(string sceneName, LoadSceneMode loadMode, out AsyncOperation asyncLoad)
        {
            try
            {
                asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, loadMode);
                asyncLoad.allowSceneActivation = false;
                return true;
            }
            catch (Exception ex)
            {
                LogError($"Failed to start scene load for {sceneName}: {ex.Message}");
                OnSceneLoadFailed?.Invoke($"Failed to load scene: {ex.Message}");
                asyncLoad = null;
                return false;
            }
        }

        /// <summary>
        /// Safely persist current scene data
        /// </summary>
        /// <returns>True if successful, false if failed</returns>
        private bool TryPersistCurrentSceneData()
        {
            try
            {
                PersistCurrentSceneData();
                return true;
            }
            catch (Exception ex)
            {
                LogError($"Failed to persist scene data: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Safely restore scene data
        /// </summary>
        /// <returns>True if successful, false if failed</returns>
        private bool TryRestoreSceneData()
        {
            try
            {
                RestoreSceneData();
                return true;
            }
            catch (Exception ex)
            {
                LogError($"Failed to restore scene data: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Unload an additive scene
        /// </summary>
        /// <param name="sceneName">Scene to unload</param>
        public void UnloadScene(string sceneName)
        {
            if (!_additiveScenes.Contains(sceneName))
            {
                LogWarning($"Scene {sceneName} is not loaded additively");
                return;
            }

            LogInfo($"Unloading additive scene: {sceneName}");
            StartCoroutine(UnloadSceneAsync(sceneName));
        }

        /// <summary>
        /// Unload scene asynchronously
        /// </summary>
        /// <param name="sceneName">Scene to unload</param>
        /// <returns>Coroutine for async unloading</returns>
        private IEnumerator UnloadSceneAsync(string sceneName)
        {
            AsyncOperation asyncUnload = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
            
            while (!asyncUnload.isDone)
            {
                yield return null;
            }

            _additiveScenes.Remove(sceneName);
            LogInfo($"Scene unloaded: {sceneName}");
        }

        #endregion

        #region Loading Screen

        /// <summary>
        /// Show loading screen with fade effect
        /// </summary>
        /// <returns>Coroutine for fade in</returns>
        private IEnumerator ShowLoadingScreen()
        {
            // This would be implemented with actual UI loading screen
            // For now, just a placeholder fade
            
            var uiManager = ServiceLocator.TryGet<UIManager>();
            if (uiManager != null)
            {
                // uiManager.ShowLoadingScreen();
                LogInfo("Loading screen shown");
            }

            yield return new WaitForSeconds(_fadeInDuration);
        }

        /// <summary>
        /// Hide loading screen with fade effect
        /// </summary>
        /// <returns>Coroutine for fade out</returns>
        private IEnumerator HideLoadingScreen()
        {
            yield return new WaitForSeconds(_fadeOutDuration);

            var uiManager = ServiceLocator.TryGet<UIManager>();
            if (uiManager != null)
            {
                // uiManager.HideLoadingScreen();
                LogInfo("Loading screen hidden");
            }
        }

        #endregion

        #region Data Persistence

        /// <summary>
        /// Store persistent data for scene transitions
        /// </summary>
        /// <param name="key">Data key</param>
        /// <param name="value">Data value</param>
        public void SetPersistentData(string key, object value)
        {
            _persistentData[key] = value;
        }

        /// <summary>
        /// Get persistent data
        /// </summary>
        /// <typeparam name="T">Type of data to retrieve</typeparam>
        /// <param name="key">Data key</param>
        /// <param name="defaultValue">Default value if key not found</param>
        /// <returns>Stored data or default value</returns>
        public T GetPersistentData<T>(string key, T defaultValue = default)
        {
            if (_persistentData.TryGetValue(key, out object value) && value is T)
            {
                return (T)value;
            }
            return defaultValue;
        }

        /// <summary>
        /// Clear persistent data
        /// </summary>
        public void ClearPersistentData()
        {
            _persistentData.Clear();
            LogInfo("Persistent data cleared");
        }

        /// <summary>
        /// Persist data from current scene
        /// </summary>
        private void PersistCurrentSceneData()
        {
            // This would gather data from various game systems
            // For now, just basic player position
            
            var gameManager = ServiceLocator.TryGet<GameManager>();
            if (gameManager != null)
            {
                SetPersistentData("gameState", gameManager.CurrentGameState);
                SetPersistentData("currentLevel", gameManager.CurrentLevel);
                SetPersistentData("currentAct", gameManager.CurrentAct);
            }

            LogInfo("Scene data persisted");
        }

        /// <summary>
        /// Restore persistent data to new scene
        /// </summary>
        private void RestoreSceneData()
        {
            // This would restore data to various game systems
            
            var gameManager = ServiceLocator.TryGet<GameManager>();
            if (gameManager != null)
            {
                var gameState = GetPersistentData<GameState>("gameState", GameState.MainMenu);
                var currentLevel = GetPersistentData<string>("currentLevel", "");
                var currentAct = GetPersistentData<int>("currentAct", 1);
                
                // Apply restored data
                if (!string.IsNullOrEmpty(currentLevel))
                {
                    gameManager.SetCurrentLevel(currentLevel);
                }
                gameManager.SetCurrentAct(currentAct);
            }

            LogInfo("Scene data restored");
        }

        #endregion

        #region Scene Utilities

        /// <summary>
        /// Get all currently loaded scenes
        /// </summary>
        /// <returns>List of loaded scene names</returns>
        public List<string> GetLoadedScenes()
        {
            var scenes = new List<string> { _currentSceneName };
            scenes.AddRange(_additiveScenes);
            return scenes;
        }

        /// <summary>
        /// Check if a scene is currently loaded
        /// </summary>
        /// <param name="sceneName">Scene name to check</param>
        /// <returns>True if scene is loaded</returns>
        public bool IsSceneLoaded(string sceneName)
        {
            return _currentSceneName == sceneName || _additiveScenes.Contains(sceneName);
        }

        /// <summary>
        /// Reload the current scene
        /// </summary>
        public void ReloadCurrentScene()
        {
            LoadScene(_currentSceneName, LoadSceneMode.Single, _enableLoadingScreen);
        }

        /// <summary>
        /// Return to the previous scene
        /// </summary>
        public void ReturnToPreviousScene()
        {
            if (!string.IsNullOrEmpty(_previousSceneName))
            {
                LoadScene(_previousSceneName, LoadSceneMode.Single, _enableLoadingScreen);
            }
            else
            {
                LogWarning("No previous scene to return to");
            }
        }

        #endregion

        #region Unity Scene Events

        /// <summary>
        /// Called when a Unity scene is loaded
        /// </summary>
        /// <param name="scene">Loaded scene</param>
        /// <param name="mode">Load mode used</param>
        private void OnUnitySceneLoaded(Scene scene, LoadSceneMode mode)
        {
            LogInfo($"Unity scene loaded: {scene.name} (Mode: {mode})");
            
            // Preload additive scenes if enabled
            if (mode == LoadSceneMode.Single && _preloadAdditiveScenes)
            {
                PreloadAdditiveScenes();
            }
        }

        /// <summary>
        /// Called when a Unity scene is unloaded
        /// </summary>
        /// <param name="scene">Unloaded scene</param>
        private void OnUnitySceneUnloaded(Scene scene)
        {
            LogInfo($"Unity scene unloaded: {scene.name}");
            _additiveScenes.Remove(scene.name);
        }

        /// <summary>
        /// Called when the active scene changes
        /// </summary>
        /// <param name="current">Current scene</param>
        /// <param name="next">Next scene</param>
        private void OnActiveSceneChanged(Scene current, Scene next)
        {
            LogInfo($"Active scene changed: {current.name} -> {next.name}");
            
            if (next.isLoaded)
            {
                _previousSceneName = _currentSceneName;
                _currentSceneName = next.name;
            }
        }

        #endregion

        #region Additive Scene Management

        /// <summary>
        /// Preload common additive scenes
        /// </summary>
        private void PreloadAdditiveScenes()
        {
            // This would load common UI scenes, audio scenes, etc.
            // Implementation depends on project structure
            LogInfo("Preloading additive scenes...");
        }

        #endregion

        #region Debug

        protected override void AppendCustomDebugInfo(System.Text.StringBuilder info)
        {
            info.AppendLine($"Current Scene: {_currentSceneName}");
            info.AppendLine($"Previous Scene: {_previousSceneName}");
            info.AppendLine($"Is Loading: {_isLoading}");
            info.AppendLine($"Additive Scenes: {_additiveScenes.Count}");
            
            if (_additiveScenes.Count > 0)
            {
                foreach (string scene in _additiveScenes)
                {
                    info.AppendLine($"  - {scene}");
                }
            }
            
            info.AppendLine($"Persistent Data: {_persistentData.Count} entries");
            info.AppendLine($"Loading Screen Enabled: {_enableLoadingScreen}");
            info.AppendLine($"Minimum Loading Time: {_minimumLoadingTime}s");
        }

        #endregion
    }
} 