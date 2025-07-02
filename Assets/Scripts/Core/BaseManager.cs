using UnityEngine;

namespace SoulBound.Core
{
    /// <summary>
    /// Base class for all core managers in SoulBound RPG
    /// Provides common lifecycle methods and utilities
    /// All managers should inherit from this to ensure consistent behavior
    /// </summary>
    public abstract class BaseManager : MonoBehaviour
    {
        [Header("Base Manager Settings")]
        [SerializeField] protected bool _enableDebugLogging = true;
        
        protected bool _isInitialized = false;

        /// <summary>
        /// Called by Bootstrapper during initialization sequence
        /// Override this in derived classes for custom initialization logic
        /// </summary>
        public virtual void Initialize()
        {
            if (_isInitialized)
            {
                LogWarning("Manager already initialized");
                return;
            }

            LogInfo($"Initializing {GetType().Name}...");
            
            OnInitialize();
            
            _isInitialized = true;
            LogInfo($"{GetType().Name} initialized successfully");
        }

        /// <summary>
        /// Override this method in derived classes for custom initialization logic
        /// Called once during the Initialize() method
        /// </summary>
        protected virtual void OnInitialize()
        {
            // Base implementation - override in derived classes
        }

        /// <summary>
        /// Called when the manager should clean up resources
        /// Override in derived classes for custom cleanup logic
        /// </summary>
        public virtual void Cleanup()
        {
            LogInfo($"Cleaning up {GetType().Name}...");
            OnCleanup();
            _isInitialized = false;
        }

        /// <summary>
        /// Override this method in derived classes for custom cleanup logic
        /// </summary>
        protected virtual void OnCleanup()
        {
            // Base implementation - override in derived classes
        }

        /// <summary>
        /// Check if this manager has been initialized
        /// </summary>
        public bool IsInitialized => _isInitialized;

        /// <summary>
        /// Get the name of this manager for debugging
        /// </summary>
        public string ManagerName => GetType().Name;

        #region Logging Utilities

        /// <summary>
        /// Log an info message with manager prefix
        /// </summary>
        /// <param name="message">Message to log</param>
        protected void LogInfo(string message)
        {
            if (_enableDebugLogging)
            {
                Debug.Log($"[{ManagerName}] {message}");
            }
        }

        /// <summary>
        /// Log a warning message with manager prefix
        /// </summary>
        /// <param name="message">Warning message to log</param>
        protected void LogWarning(string message)
        {
            if (_enableDebugLogging)
            {
                Debug.LogWarning($"[{ManagerName}] {message}");
            }
        }

        /// <summary>
        /// Log an error message with manager prefix
        /// </summary>
        /// <param name="message">Error message to log</param>
        protected void LogError(string message)
        {
            Debug.LogError($"[{ManagerName}] {message}");
        }

        /// <summary>
        /// Log an exception with manager prefix
        /// </summary>
        /// <param name="exception">Exception to log</param>
        protected void LogException(System.Exception exception)
        {
            Debug.LogException(exception);
        }

        #endregion

        #region Unity Lifecycle

        protected virtual void Awake()
        {
            // Ensure this manager persists across scene loads if attached to DontDestroyOnLoad
            // Note: Actual DontDestroyOnLoad is handled by Bootstrapper
        }

        protected virtual void Start()
        {
            // Override in derived classes if needed
        }

        protected virtual void OnDestroy()
        {
            if (_isInitialized)
            {
                Cleanup();
            }
        }

        #endregion

        #region Debug Tools

        /// <summary>
        /// Get debug information about this manager
        /// Override in derived classes to provide specific debug info
        /// </summary>
        /// <returns>Debug information string</returns>
        public virtual string GetDebugInfo()
        {
            var info = new System.Text.StringBuilder();
            info.AppendLine($"=== {ManagerName} Debug Info ===");
            info.AppendLine($"Initialized: {_isInitialized}");
            info.AppendLine($"Debug Logging: {_enableDebugLogging}");
            info.AppendLine($"GameObject: {gameObject.name}");
            info.AppendLine($"Active: {gameObject.activeInHierarchy}");
            
            // Add custom debug info from derived classes
            AppendCustomDebugInfo(info);
            
            return info.ToString();
        }

        /// <summary>
        /// Override in derived classes to add custom debug information
        /// </summary>
        /// <param name="info">StringBuilder to append debug info to</param>
        protected virtual void AppendCustomDebugInfo(System.Text.StringBuilder info)
        {
            // Override in derived classes
        }

        /// <summary>
        /// Context menu item for debugging in editor
        /// </summary>
        [ContextMenu("Print Debug Info")]
        private void PrintDebugInfo()
        {
            Debug.Log(GetDebugInfo());
        }

        #endregion
    }
} 