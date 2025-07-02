using System.Collections;
using UnityEngine;

namespace SoulBound.Core
{
    /// <summary>
    /// Scene loading utility for SoulBound RPG
    /// Provides async scene loading with progress tracking
    /// Used by both SceneManager and Bootstrapper
    /// </summary>
    public class SceneLoader : BaseManager
    {
        [Header("Loading Settings")]
        [SerializeField] private float _minimumLoadTime = 1f;
        [SerializeField] private bool _allowSceneActivation = true;

        protected override void OnInitialize()
        {
            LogInfo("Scene loader initialized");
        }

        /// <summary>
        /// Load a scene asynchronously
        /// </summary>
        /// <param name="sceneName">Name of scene to load</param>
        /// <returns>Coroutine for the loading operation</returns>
        public IEnumerator LoadSceneAsync(string sceneName)
        {
            LogInfo($"Starting async load of scene: {sceneName}");
            
            AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = _allowSceneActivation;
            
            float startTime = Time.time;
            
            while (!operation.isDone)
            {
                float progress = operation.progress;
                LogInfo($"Loading progress: {progress:F2}");
                
                // Ensure minimum load time
                if (operation.progress >= 0.9f && Time.time - startTime >= _minimumLoadTime)
                {
                    operation.allowSceneActivation = true;
                }
                
                yield return null;
            }
            
            LogInfo($"Scene loading completed: {sceneName}");
        }

        protected override void AppendCustomDebugInfo(System.Text.StringBuilder info)
        {
            info.AppendLine($"Minimum Load Time: {_minimumLoadTime}");
            info.AppendLine($"Allow Scene Activation: {_allowSceneActivation}");
        }
    }
} 