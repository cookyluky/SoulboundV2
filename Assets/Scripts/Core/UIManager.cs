using System.Collections.Generic;
using UnityEngine;

namespace SoulBound.Core
{
    /// <summary>
    /// UI management system for SoulBound RPG
    /// Handles UI panel navigation, state, and transitions
    /// </summary>
    public class UIManager : BaseManager
    {
        [Header("UI Settings")]
        [SerializeField] private bool _enableUIAnimations = true;
        [SerializeField] private float _transitionDuration = 0.3f;

        // UI panel tracking
        private Dictionary<string, GameObject> _uiPanels = new Dictionary<string, GameObject>();
        private Stack<string> _panelHistory = new Stack<string>();
        private string _currentPanel = "";

        // Properties
        public string CurrentPanel => _currentPanel;
        public bool HasPanelHistory => _panelHistory.Count > 0;

        protected override void OnInitialize()
        {
            LogInfo("Setting up UI management system");
            
            // Find and register existing UI panels
            RegisterExistingPanels();
        }

        protected override void OnCleanup()
        {
            _uiPanels.Clear();
            _panelHistory.Clear();
        }

        /// <summary>
        /// Register existing UI panels in the scene
        /// </summary>
        private void RegisterExistingPanels()
        {
            // This would scan for UI panels and register them
            LogInfo("UI panel registration system ready");
        }

        /// <summary>
        /// Show a UI panel by name
        /// </summary>
        /// <param name="panelName">Name of the panel to show</param>
        /// <param name="addToHistory">Whether to add current panel to history</param>
        public void ShowPanel(string panelName, bool addToHistory = true)
        {
            if (string.IsNullOrEmpty(panelName))
            {
                LogError("Panel name cannot be null or empty");
                return;
            }

            // Add current panel to history if requested
            if (addToHistory && !string.IsNullOrEmpty(_currentPanel))
            {
                _panelHistory.Push(_currentPanel);
            }

            // Hide current panel
            if (!string.IsNullOrEmpty(_currentPanel))
            {
                HidePanel(_currentPanel, false);
            }

            // Show new panel
            if (_uiPanels.TryGetValue(panelName, out GameObject panel))
            {
                panel.SetActive(true);
                _currentPanel = panelName;
                LogInfo($"Showing UI panel: {panelName}");
            }
            else
            {
                LogError($"UI panel not found: {panelName}");
            }
        }

        /// <summary>
        /// Hide a UI panel by name
        /// </summary>
        /// <param name="panelName">Name of the panel to hide</param>
        /// <param name="updateCurrent">Whether to update current panel tracking</param>
        public void HidePanel(string panelName, bool updateCurrent = true)
        {
            if (_uiPanels.TryGetValue(panelName, out GameObject panel))
            {
                panel.SetActive(false);
                
                if (updateCurrent && _currentPanel == panelName)
                {
                    _currentPanel = "";
                }
                
                LogInfo($"Hiding UI panel: {panelName}");
            }
        }

        /// <summary>
        /// Go back to the previous panel in history
        /// </summary>
        public void GoBack()
        {
            if (_panelHistory.Count > 0)
            {
                string previousPanel = _panelHistory.Pop();
                ShowPanel(previousPanel, false);
                LogInfo($"Navigated back to: {previousPanel}");
            }
            else
            {
                LogWarning("No panel history to go back to");
            }
        }

        /// <summary>
        /// Register a UI panel
        /// </summary>
        /// <param name="panelName">Name to register the panel under</param>
        /// <param name="panelObject">Panel GameObject</param>
        public void RegisterPanel(string panelName, GameObject panelObject)
        {
            _uiPanels[panelName] = panelObject;
            LogInfo($"Registered UI panel: {panelName}");
        }

        protected override void AppendCustomDebugInfo(System.Text.StringBuilder info)
        {
            info.AppendLine($"Current Panel: {_currentPanel}");
            info.AppendLine($"Registered Panels: {_uiPanels.Count}");
            info.AppendLine($"Panel History Count: {_panelHistory.Count}");
            info.AppendLine($"UI Animations: {_enableUIAnimations}");
            info.AppendLine($"Transition Duration: {_transitionDuration}");
        }
    }
} 