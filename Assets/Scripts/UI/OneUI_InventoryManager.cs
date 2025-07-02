using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DevsDaddy.Shared.UIFramework;
using SoulBound.Systems;
using SoulBound.Core;

namespace SoulBound.UI
{
    /// <summary>
    /// OneUI_InventoryManager for @Task_26 - Complete Soulbound Inventory & UI System
    /// Manages the organic-themed pod-based inventory system using OneUI Kit framework
    /// Integrates with existing @EssenceManager and provides accessibility features
    /// </summary>
    public class OneUI_InventoryManager : MonoBehaviour
    {
        [Header("OneUI Canvas Configuration")]
        [SerializeField] private Canvas _inventoryCanvas;
        [SerializeField] private CanvasScaler _canvasScaler;
        [SerializeField] private GraphicRaycaster _graphicRaycaster;
        
        [Header("Pod Container System - From Task_26 PRD")]
        [SerializeField] private Transform _podContainer;
        [SerializeField] private OneUI_InventoryPod _combatItemsPod;
        [SerializeField] private OneUI_InventoryPod _consumablesPod;
        [SerializeField] private OneUI_InventoryPod _questItemsPod;
        [SerializeField] private OneUI_InventoryPod _essencePod;
        
        [Header("Inspector Panel & Hotkey Bar")]
        [SerializeField] private GameObject _inspectorPanel;
        [SerializeField] private Transform _hotkeyBar;
        [SerializeField] private int _hotkeySlotCount = 4; // Per PRD specifications
        
        [Header("Accessibility Features")]
        [SerializeField] private bool _enableHighContrastMode = false;
        [SerializeField] private bool _enableKeyboardNavigation = true;
        [SerializeField] private bool _enableScreenReaderSupport = true;
        
        [Header("Debug Settings")]
        [SerializeField] private bool _enableDebugLogging = true;
        
        // Private fields
        private OneUI_InventoryPod _currentActivePod;
        private bool _isInventoryVisible = false;
        private Dictionary<PodCategory, OneUI_InventoryPod> _podLookup;
        
        // Integration with existing systems
        private EssenceManager _essenceManager;
        private InputManager _inputManager;
        
        private void Awake()
        {
            ValidateComponents();
            InitializeInventoryCanvas();
        }
        
        private void Start()
        {
            // Get references to existing managers per @Task_17 singleton pattern
            _essenceManager = FindObjectOfType<EssenceManager>();
            _inputManager = FindObjectOfType<InputManager>();
            
            if (_essenceManager == null)
            {
                LogError("EssenceManager not found! Inventory system requires essence integration.");
            }
            
            InitializePodSystem();
            SetupAccessibilityFeatures();
            
            // Start with inventory hidden
            SetInventoryVisibility(false);
        }
        
        /// <summary>
        /// Initialize the OneUI canvas with proper settings for organic inventory theme
        /// Follows PRD specifications: 1920×1080 base with 30% scale support
        /// </summary>
        private void InitializeInventoryCanvas()
        {
            if (_inventoryCanvas == null)
            {
                LogError("Inventory Canvas not assigned! Please assign in inspector.");
                return;
            }
            
            // Configure canvas for overlay mode (appears over game world)
            _inventoryCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _inventoryCanvas.sortingOrder = 100; // Above other UI elements
            
            // Configure canvas scaler for responsive design per PRD requirements
            if (_canvasScaler != null)
            {
                _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                _canvasScaler.referenceResolution = new Vector2(1920, 1080); // PRD base resolution
                _canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                _canvasScaler.matchWidthOrHeight = 0.5f; // Balanced scaling
                _canvasScaler.referencePixelsPerUnit = 100f;
            }
            
            // Enable graphic raycaster for interaction
            if (_graphicRaycaster == null)
            {
                _graphicRaycaster = _inventoryCanvas.GetComponent<GraphicRaycaster>();
                if (_graphicRaycaster == null)
                {
                    _graphicRaycaster = _inventoryCanvas.gameObject.AddComponent<GraphicRaycaster>();
                }
            }
            
            LogDebug("OneUI Inventory Canvas initialized successfully");
        }
        
        /// <summary>
        /// Initialize the pod-based container system - Task 26.2 Implementation
        /// Four main categories: Combat Items, Consumables, Quest, Essence
        /// </summary>
        private void InitializePodSystem()
        {
            // Initialize pod lookup dictionary
            _podLookup = new Dictionary<PodCategory, OneUI_InventoryPod>();
            
            // Configure each pod with appropriate colors and titles per PRD specifications
            if (_combatItemsPod != null)
            {
                _combatItemsPod.ConfigurePod(PodCategory.CombatItems, "Combat Items", new Color(0.8f, 0.2f, 0.2f, 1f)); // Red tint
                _combatItemsPod.OnPodOpened += OnPodOpened;
                _combatItemsPod.OnPodClosed += OnPodClosed;
                _podLookup[PodCategory.CombatItems] = _combatItemsPod;
            }
            
            if (_consumablesPod != null)
            {
                _consumablesPod.ConfigurePod(PodCategory.Consumables, "Consumables", new Color(0.2f, 0.8f, 0.2f, 1f)); // Green tint
                _consumablesPod.OnPodOpened += OnPodOpened;
                _consumablesPod.OnPodClosed += OnPodClosed;
                _podLookup[PodCategory.Consumables] = _consumablesPod;
            }
            
            if (_questItemsPod != null)
            {
                _questItemsPod.ConfigurePod(PodCategory.Quest, "Quest Items", new Color(0.8f, 0.8f, 0.2f, 1f)); // Yellow tint
                _questItemsPod.OnPodOpened += OnPodOpened;
                _questItemsPod.OnPodClosed += OnPodClosed;
                _podLookup[PodCategory.Quest] = _questItemsPod;
            }
            
            if (_essencePod != null)
            {
                _essencePod.ConfigurePod(PodCategory.Essence, "Essence", new Color(0.2f, 0.6f, 0.8f, 1f)); // Blue tint
                _essencePod.OnPodOpened += OnPodOpened;
                _essencePod.OnPodClosed += OnPodClosed;
                _podLookup[PodCategory.Essence] = _essencePod;
            }
            
            // Ensure pod container is properly configured
            if (_podContainer == null)
            {
                GameObject podContainerObj = new GameObject("PodContainer");
                podContainerObj.transform.SetParent(_inventoryCanvas.transform, false);
                _podContainer = podContainerObj.transform;
                
                // Configure for organic pod layout
                RectTransform podRect = podContainerObj.AddComponent<RectTransform>();
                podRect.anchorMin = new Vector2(0.1f, 0.1f);
                podRect.anchorMax = new Vector2(0.9f, 0.9f);
                podRect.offsetMin = Vector2.zero;
                podRect.offsetMax = Vector2.zero;
            }
            
            LogDebug("Pod-based container system initialized with 4 categories");
        }
        
        /// <summary>
        /// Setup accessibility features as specified in PRD
        /// High-contrast mode, keyboard navigation, screen reader support
        /// </summary>
        private void SetupAccessibilityFeatures()
        {
            // Accessibility implementation will be completed in Task 26.8
            LogDebug($"Accessibility features configured: " +
                    $"HighContrast={_enableHighContrastMode}, " +
                    $"Keyboard={_enableKeyboardNavigation}, " +
                    $"ScreenReader={_enableScreenReaderSupport}");
        }
        
        /// <summary>
        /// Toggle inventory visibility with smooth animation
        /// Called by input system or UI buttons
        /// </summary>
        public void ToggleInventory()
        {
            SetInventoryVisibility(!_isInventoryVisible);
        }
        
        /// <summary>
        /// Set inventory visibility state
        /// </summary>
        /// <param name="visible">True to show inventory, false to hide</param>
        public void SetInventoryVisibility(bool visible)
        {
            _isInventoryVisible = visible;
            _inventoryCanvas.gameObject.SetActive(_isInventoryVisible);
            
            LogDebug($"Inventory visibility set to: {_isInventoryVisible}");
            
            // TODO: Add smooth animation in later tasks (Task 26.10)
        }
        
        /// <summary>
        /// Validate required components are assigned
        /// </summary>
        private void ValidateComponents()
        {
            if (_inventoryCanvas == null)
            {
                LogError("Inventory Canvas is not assigned in OneUI_InventoryManager");
            }
            
            if (_canvasScaler == null && _inventoryCanvas != null)
            {
                _canvasScaler = _inventoryCanvas.GetComponent<CanvasScaler>();
                if (_canvasScaler == null)
                {
                    _canvasScaler = _inventoryCanvas.gameObject.AddComponent<CanvasScaler>();
                }
            }
        }
        
        /// <summary>
        /// Debug logging with toggle
        /// </summary>
        private void LogDebug(string message)
        {
            if (_enableDebugLogging)
            {
                Debug.Log($"[OneUI_InventoryManager] {message}");
            }
        }
        
        /// <summary>
        /// Error logging (always enabled)
        /// </summary>
        private void LogError(string message)
        {
            Debug.LogError($"[OneUI_InventoryManager] {message}");
        }
        
        #region Pod Event Handling - Task 26.2
        
        /// <summary>
        /// Handle pod opened event
        /// </summary>
        private void OnPodOpened(OneUI_InventoryPod openedPod)
        {
            _currentActivePod = openedPod;
            LogDebug($"Pod opened: {openedPod.Category}");
            
            // Future: Add audio feedback, analytics tracking
        }
        
        /// <summary>
        /// Handle pod closed event
        /// </summary>
        private void OnPodClosed(OneUI_InventoryPod closedPod)
        {
            if (_currentActivePod == closedPod)
            {
                _currentActivePod = null;
            }
            LogDebug($"Pod closed: {closedPod.Category}");
        }
        
        /// <summary>
        /// Get pod by category
        /// </summary>
        public OneUI_InventoryPod GetPod(PodCategory category)
        {
            _podLookup?.TryGetValue(category, out OneUI_InventoryPod pod);
            return pod;
        }
        
        /// <summary>
        /// Open specific pod by category
        /// </summary>
        public void OpenPod(PodCategory category)
        {
            OneUI_InventoryPod pod = GetPod(category);
            pod?.SetPodState(true);
        }
        
        /// <summary>
        /// Close specific pod by category
        /// </summary>
        public void ClosePod(PodCategory category)
        {
            OneUI_InventoryPod pod = GetPod(category);
            pod?.SetPodState(false);
        }
        
        /// <summary>
        /// Close all pods
        /// </summary>
        public void CloseAllPods()
        {
            if (_podLookup == null) return;
            
            foreach (var pod in _podLookup.Values)
            {
                pod?.SetPodState(false);
            }
        }
        
        #endregion
        
        #region Public API for Integration
        
        /// <summary>
        /// Get reference to essence manager for integration
        /// Used by pod system to display essence items
        /// </summary>
        public EssenceManager GetEssenceManager()
        {
            return _essenceManager;
        }
        
        /// <summary>
        /// Check if inventory is currently visible
        /// </summary>
        public bool IsInventoryVisible => _isInventoryVisible;
        
        /// <summary>
        /// Get the pod container for child pod setup
        /// </summary>
        public Transform GetPodContainer()
        {
            return _podContainer;
        }
        
        #endregion
    }
} 