using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using SoulBound.Systems;

namespace SoulBound.Core
{
    /// <summary>
    /// Central initialization controller for SoulBound RPG
    /// Responsible for:
    /// - Setting up DontDestroyOnLoad persistence
    /// - Initializing all core managers via ServiceLocator
    /// - Loading the main menu scene after initialization
    /// 
    /// This GameObject should exist in the Bootstrap scene which is Scene 0 in Build Settings
    /// </summary>
    public class Bootstrapper : MonoBehaviour
    {
        [Header("Initialization Settings")]
        [SerializeField] private bool _enableDebugLogging = true;
        [SerializeField] private string _initialSceneName = "MainMenu";
        [SerializeField] private float _initializationDelay = 0.1f;

        [Header("Manager Prefabs (Optional)")]
        [Tooltip("If null, managers will be created as empty GameObjects")]
        [SerializeField] private GameObject _gameManagerPrefab;
        [SerializeField] private GameObject _audioManagerPrefab;
        [SerializeField] private GameObject _essenceManagerPrefab;
        [SerializeField] private GameObject _uiManagerPrefab;
        [SerializeField] private GameObject _inputManagerPrefab;
        [SerializeField] private GameObject _saveManagerPrefab;
        [SerializeField] private GameObject _sceneLoaderPrefab;

        private bool _isInitialized = false;

        private void Awake()
        {
            // Ensure this GameObject persists across all scene loads
            DontDestroyOnLoad(gameObject);
            
            // Prevent multiple instances
            if (FindObjectsByType<Bootstrapper>(FindObjectsSortMode.None).Length > 1)
            {
                Debug.LogWarning("Bootstrapper: Multiple instances detected, destroying duplicate");
                Destroy(gameObject);
                return;
            }

            if (_enableDebugLogging)
            {
                Debug.Log("Bootstrapper: Starting SoulBound RPG initialization sequence");
            }

            // Start initialization coroutine
            StartCoroutine(InitializeGameSystems());
        }

        /// <summary>
        /// Main initialization sequence
        /// Registers all core managers with ServiceLocator and loads initial scene
        /// </summary>
        private IEnumerator InitializeGameSystems()
        {
            // Small delay to ensure Unity is fully ready
            yield return new WaitForSeconds(_initializationDelay);

            // Initialize core managers in dependency order
            // Using individual try-catch blocks for each initialization to avoid yield issues
            
            if (!TryInitializeManager("GameManager", InitializeGameManager))
                yield break;
            yield return null; // Spread initialization across frames

            if (!TryInitializeManager("AudioManager", InitializeAudioManager))
                yield break;
            yield return null;

            if (!TryInitializeManager("EssenceManager", InitializeEssenceManager))
                yield break;
            yield return null;

            if (!TryInitializeManager("GameSceneManager", InitializeGameSceneManager))
                yield break;
            yield return null;

            if (!TryInitializeManager("InputManager", InitializeInputManager))
                yield break;
            yield return null;

            if (!TryInitializeManager("SaveManager", InitializeSaveManager))
                yield break;
            yield return null;

            if (!TryInitializeManager("UIManager", InitializeUIManager))
                yield break;
            yield return null;

            if (!TryInitializeManager("SceneLoader", InitializeSceneLoader))
                yield break;
            yield return null;

            // Mark initialization complete
            _isInitialized = true;

            if (_enableDebugLogging)
            {
                Debug.Log("Bootstrapper: All core systems initialized successfully");
                Debug.Log(ServiceLocator.GetDebugInfo());
            }

            // Load the initial scene (usually MainMenu)
            // Temporarily disabled for InputManager testing
            // yield return LoadInitialScene();
        }

        /// <summary>
        /// Helper method to safely initialize managers with error handling
        /// </summary>
        /// <param name="managerName">Name of the manager for logging</param>
        /// <param name="initializeAction">Action to perform initialization</param>
        /// <returns>True if successful, false if failed</returns>
        private bool TryInitializeManager(string managerName, System.Action initializeAction)
        {
            try
            {
                initializeAction.Invoke();
                return true;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Bootstrapper: Failed to initialize {managerName}: {ex.Message}");
                Debug.LogException(ex);
                return false;
            }
        }

        /// <summary>
        /// Initialize GameManager - Central game state and progression controller
        /// </summary>
        private void InitializeGameManager()
        {
            var gameManager = CreateOrFindManager<GameManager>("GameManager", _gameManagerPrefab);
            
            // Initialize the manager if it inherits from BaseManager
            if (gameManager is BaseManager baseManager)
            {
                baseManager.Initialize();
            }
            
            ServiceLocator.Register(gameManager);
            
            if (_enableDebugLogging)
            {
                Debug.Log("Bootstrapper: GameManager initialized and registered");
            }
        }

        /// <summary>
        /// Initialize AudioManager - Music and sound effects controller
        /// </summary>
        private void InitializeAudioManager()
        {
            var audioManager = CreateOrFindManager<AudioManager>("AudioManager", _audioManagerPrefab);
            
            // Initialize the manager if it inherits from BaseManager
            if (audioManager is BaseManager baseManager)
            {
                baseManager.Initialize();
            }
            
            ServiceLocator.Register(audioManager);
            
            if (_enableDebugLogging)
            {
                Debug.Log("Bootstrapper: AudioManager initialized and registered");
            }
        }

        /// <summary>
        /// Initialize EssenceManager - Soul essence collection and banking system
        /// </summary>
        private void InitializeEssenceManager()
        {
            var essenceManager = CreateOrFindManager<EssenceManager>("EssenceManager", _essenceManagerPrefab);
            
            // Initialize the manager if it inherits from BaseManager
            if (essenceManager is BaseManager baseManager)
            {
                baseManager.Initialize();
            }
            
            ServiceLocator.Register(essenceManager);
            
            if (_enableDebugLogging)
            {
                Debug.Log("Bootstrapper: EssenceManager initialized and registered");
            }
        }

        /// <summary>
        /// Initialize GameSceneManager - Scene loading and transition management
        /// </summary>
        private void InitializeGameSceneManager()
        {
            var sceneManager = CreateOrFindManager<GameSceneManager>("GameSceneManager");
            
            // Initialize the manager if it inherits from BaseManager
            if (sceneManager is BaseManager baseManager)
            {
                baseManager.Initialize();
            }
            
            ServiceLocator.Register(sceneManager);
            
            if (_enableDebugLogging)
            {
                Debug.Log("Bootstrapper: GameSceneManager initialized and registered");
            }
        }

        /// <summary>
        /// Initialize InputManager - Input system integration and event handling
        /// </summary>
        private void InitializeInputManager()
        {
            var inputManager = CreateOrFindManager<InputManager>("InputManager", _inputManagerPrefab);
            
            // Initialize the manager if it inherits from BaseManager
            if (inputManager is BaseManager baseManager)
            {
                baseManager.Initialize();
            }
            
            ServiceLocator.Register(inputManager);
            
            if (_enableDebugLogging)
            {
                Debug.Log("Bootstrapper: InputManager initialized and registered");
            }
        }

        /// <summary>
        /// Initialize SaveManager - Game save/load functionality
        /// </summary>
        private void InitializeSaveManager()
        {
            var saveManager = CreateOrFindManager<SaveManager>("SaveManager", _saveManagerPrefab);
            
            // Initialize the manager if it inherits from BaseManager
            if (saveManager is BaseManager baseManager)
            {
                baseManager.Initialize();
            }
            
            ServiceLocator.Register(saveManager);
            
            if (_enableDebugLogging)
            {
                Debug.Log("Bootstrapper: SaveManager initialized and registered");
            }
        }

        /// <summary>
        /// Initialize UIManager - User interface management and navigation
        /// </summary>
        private void InitializeUIManager()
        {
            var uiManager = CreateOrFindManager<UIManager>("UIManager", _uiManagerPrefab);
            
            // Initialize the manager if it inherits from BaseManager
            if (uiManager is BaseManager baseManager)
            {
                baseManager.Initialize();
            }
            
            ServiceLocator.Register(uiManager);
            
            if (_enableDebugLogging)
            {
                Debug.Log("Bootstrapper: UIManager initialized and registered");
            }
        }

        /// <summary>
        /// Initialize SceneLoader - Async scene loading with transitions
        /// </summary>
        private void InitializeSceneLoader()
        {
            var sceneLoader = CreateOrFindManager<SceneLoader>("SceneLoader", _sceneLoaderPrefab);
            
            // Initialize the manager if it inherits from BaseManager
            if (sceneLoader is BaseManager baseManager)
            {
                baseManager.Initialize();
            }
            
            ServiceLocator.Register(sceneLoader);
            
            if (_enableDebugLogging)
            {
                Debug.Log("Bootstrapper: SceneLoader initialized and registered");
            }
        }

        /// <summary>
        /// Generic method to create or find manager instances
        /// </summary>
        /// <typeparam name="T">Manager component type</typeparam>
        /// <param name="managerName">Name for the GameObject</param>
        /// <param name="prefab">Optional prefab to instantiate</param>
        /// <returns>Manager component instance</returns>
        private T CreateOrFindManager<T>(string managerName, GameObject prefab = null) where T : MonoBehaviour
        {
            // First try to find existing manager in scene
            T existingManager = FindFirstObjectByType<T>();
            if (existingManager != null)
            {
                DontDestroyOnLoad(existingManager.gameObject);
                return existingManager;
            }

            // Create new manager
            GameObject managerObject;
            
            if (prefab != null)
            {
                managerObject = Instantiate(prefab);
                managerObject.name = managerName; // Remove (Clone) suffix
            }
            else
            {
                managerObject = new GameObject(managerName);
                managerObject.AddComponent<T>();
            }

            DontDestroyOnLoad(managerObject);
            return managerObject.GetComponent<T>();
        }

        /// <summary>
        /// Load the initial scene after all systems are initialized
        /// </summary>
        private IEnumerator LoadInitialScene()
        {
            if (string.IsNullOrEmpty(_initialSceneName))
            {
                Debug.LogWarning("Bootstrapper: No initial scene specified, staying in Bootstrap scene");
                yield break;
            }

            if (_enableDebugLogging)
            {
                Debug.Log($"Bootstrapper: Loading initial scene: {_initialSceneName}");
            }

            // Check if SceneLoader is available for managed transitions
            var sceneLoader = ServiceLocator.TryGet<SceneLoader>();
            if (sceneLoader != null)
            {
                // Use SceneLoader for managed transition
                yield return sceneLoader.LoadSceneAsync(_initialSceneName);
            }
            else
            {
                // Fallback to direct Unity scene loading
                yield return SceneManager.LoadSceneAsync(_initialSceneName);
            }

            if (_enableDebugLogging)
            {
                Debug.Log($"Bootstrapper: Successfully loaded {_initialSceneName}");
            }
        }

        /// <summary>
        /// Public accessor to check if initialization is complete
        /// </summary>
        public bool IsInitialized => _isInitialized;

        /// <summary>
        /// Force re-initialization (primarily for development/testing)
        /// </summary>
        [ContextMenu("Force Re-Initialize")]
        public void ForceReInitialize()
        {
            if (_enableDebugLogging)
            {
                Debug.Log("Bootstrapper: Force re-initialization requested");
            }

            ServiceLocator.Clear();
            _isInitialized = false;
            StartCoroutine(InitializeGameSystems());
        }

        private void OnDestroy()
        {
            if (_enableDebugLogging)
            {
                Debug.Log("Bootstrapper: Destroyed - clearing ServiceLocator");
            }
            
            ServiceLocator.Clear();
        }

        /// <summary>
        /// Get initialization status for debugging
        /// </summary>
        public string GetInitializationStatus()
        {
            var status = new System.Text.StringBuilder();
            status.AppendLine("=== Bootstrapper Status ===");
            status.AppendLine($"Initialized: {_isInitialized}");
            status.AppendLine($"Initial Scene: {_initialSceneName}");
            status.AppendLine($"Debug Logging: {_enableDebugLogging}");
            status.AppendLine();
            status.Append(ServiceLocator.GetDebugInfo());
            
            return status.ToString();
        }
    }
} 