using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using DevsDaddy.Shared.UIFramework;
using SoulBound.Systems;
using SoulBound.Core;

namespace SoulBound.UI
{
    /// <summary>
    /// OneUI_InventoryPod for @Task_26.2 - Pod-Based Container System
    /// Represents a single pod container for organizing inventory items by category
    /// Features organic-themed visual design with vine/petal motifs and smooth animations
    /// </summary>
    public class OneUI_InventoryPod : MonoBehaviour
    {
        [Header("Pod Configuration - From Task_26 PRD")]
        [SerializeField] private PodCategory _podCategory;
        [SerializeField] private string _podTitle = "Pod Title";
        [SerializeField] private Color _podAccentColor = new Color(0.2f, 0.6f, 0.5f, 1f); // Blue-green tint
        
        [Header("Pod Visual Components")]
        [SerializeField] private GameObject _podHeader;
        [SerializeField] private TextMeshProUGUI _podTitleText;
        [SerializeField] private Image _podBackground;
        [SerializeField] private Image _podAccentImage;
        [SerializeField] private Button _podToggleButton;
        
        [Header("Pod Content Area")]
        [SerializeField] private GameObject _podContentContainer;
        [SerializeField] private Transform _itemGridContainer;
        [SerializeField] private GridLayoutGroup _itemGridLayout;
        [SerializeField] private CanvasGroup _contentCanvasGroup;
        
        [Header("Animation Settings")]
        [SerializeField] private float _openCloseAnimationDuration = 0.5f;
        [SerializeField] private AnimationCurve _openCloseCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        [SerializeField] private float _hoverScaleMultiplier = 1.05f;
        [SerializeField] private float _hoverAnimationDuration = 0.2f;
        
        [Header("Pod State")]
        [SerializeField] private bool _isOpen = false;
        [SerializeField] private bool _startOpen = false;
        [SerializeField] private int _maxItemsPerPod = 15; // 5x3 grid per PRD
        
        // Pod state management
        private bool _isAnimating = false;
        private Vector2 _closedSize;
        private Vector2 _openSize;
        private RectTransform _rectTransform;
        private List<GameObject> _itemSlots = new List<GameObject>();
        
        // Events for integration
        public System.Action<OneUI_InventoryPod> OnPodOpened;
        public System.Action<OneUI_InventoryPod> OnPodClosed;
        public System.Action<OneUI_InventoryPod, Vector2Int> OnItemSlotSelected;
        
        public PodCategory Category => _podCategory;
        public bool IsOpen => _isOpen;
        public bool IsAnimating => _isAnimating;
        public int ItemCount => _itemSlots.Count;
        public int MaxItems => _maxItemsPerPod;
        
        private void Awake()
        {
            InitializePodComponents();
            SetupPodVisuals();
            SetupGridLayout();
        }
        
        private void Start()
        {
            // Set initial state based on startOpen setting
            if (_startOpen)
            {
                SetPodStateImmediate(true);
            }
            else
            {
                SetPodStateImmediate(false);
            }
        }
        
        #region Pod Initialization
        
        private void InitializePodComponents()
        {
            _rectTransform = GetComponent<RectTransform>();
            
            // Auto-find components if not assigned
            if (_podTitleText == null)
                _podTitleText = GetComponentInChildren<TextMeshProUGUI>();
            
            if (_podBackground == null)
                _podBackground = GetComponent<Image>();
            
            if (_podToggleButton == null)
                _podToggleButton = GetComponentInChildren<Button>();
            
            if (_contentCanvasGroup == null && _podContentContainer != null)
                _contentCanvasGroup = _podContentContainer.GetComponent<CanvasGroup>();
            
            if (_itemGridLayout == null && _itemGridContainer != null)
                _itemGridLayout = _itemGridContainer.GetComponent<GridLayoutGroup>();
            
            // Setup button listener
            if (_podToggleButton != null)
            {
                _podToggleButton.onClick.RemoveAllListeners();
                _podToggleButton.onClick.AddListener(TogglePod);
            }
        }
        
        private void SetupPodVisuals()
        {
            // Apply pod title
            if (_podTitleText != null)
            {
                _podTitleText.text = _podTitle;
                _podTitleText.color = Color.white;
            }
            
            // Apply organic-themed background color (dark charcoal base with blue-green tint)
            if (_podBackground != null)
            {
                Color baseColor = new Color(0.15f, 0.15f, 0.15f, 0.9f); // Semi-opaque dark charcoal
                Color tintedColor = Color.Lerp(baseColor, _podAccentColor, 0.3f);
                _podBackground.color = tintedColor;
            }
            
            // Apply accent color to accent elements
            if (_podAccentImage != null)
            {
                _podAccentImage.color = _podAccentColor;
            }
        }
        
        private void SetupGridLayout()
        {
            if (_itemGridLayout == null || _itemGridContainer == null) return;
            
            // Configure 5x3 curved grid layout per PRD specifications
            _itemGridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _itemGridLayout.constraintCount = 5; // 5 columns for 5x3 grid
            _itemGridLayout.spacing = new Vector2(8f, 8f);
            _itemGridLayout.childAlignment = TextAnchor.MiddleCenter;
            
            // Calculate cell size based on container size
            RectTransform containerRect = _itemGridContainer.GetComponent<RectTransform>();
            if (containerRect != null)
            {
                float containerWidth = containerRect.rect.width;
                float cellSize = (containerWidth - (_itemGridLayout.spacing.x * 4)) / 5; // 5 columns
                _itemGridLayout.cellSize = new Vector2(cellSize, cellSize);
            }
            
            // Store size states for animation
            CalculateSizeStates();
        }
        
        private void CalculateSizeStates()
        {
            if (_rectTransform == null) return;
            
            // Calculate closed size (header only)
            float headerHeight = _podHeader != null ? _podHeader.GetComponent<RectTransform>().rect.height : 60f;
            _closedSize = new Vector2(_rectTransform.rect.width, headerHeight);
            
            // Calculate open size (header + content)
            float contentHeight = _podContentContainer != null ? 
                _podContentContainer.GetComponent<RectTransform>().rect.height : 200f;
            _openSize = new Vector2(_rectTransform.rect.width, headerHeight + contentHeight);
        }
        
        #endregion
        
        #region Pod Animation & State Management
        
        public void TogglePod()
        {
            if (_isAnimating) return;
            SetPodState(!_isOpen);
        }
        
        public void SetPodState(bool open)
        {
            if (_isAnimating || _isOpen == open) return;
            
            _isAnimating = true;
            StartCoroutine(AnimatePodState(open));
        }
        
        private void SetPodStateImmediate(bool open)
        {
            _isOpen = open;
            
            if (_podContentContainer != null)
            {
                _podContentContainer.SetActive(open);
            }
            
            if (_contentCanvasGroup != null)
            {
                _contentCanvasGroup.alpha = open ? 1f : 0f;
                _contentCanvasGroup.interactable = open;
                _contentCanvasGroup.blocksRaycasts = open;
            }
            
            // Resize to appropriate state
            CalculateSizeStates();
            if (_rectTransform != null)
            {
                _rectTransform.sizeDelta = open ? _openSize : _closedSize;
            }
        }
        
        private IEnumerator AnimatePodState(bool targetOpen)
        {
            float elapsedTime = 0f;
            Vector2 startSize = _rectTransform.sizeDelta;
            Vector2 targetSize = targetOpen ? _openSize : _closedSize;
            
            float startAlpha = _contentCanvasGroup?.alpha ?? (targetOpen ? 0f : 1f);
            float targetAlpha = targetOpen ? 1f : 0f;
            
            // Ensure content is active during opening animation
            if (targetOpen && _podContentContainer != null)
            {
                _podContentContainer.SetActive(true);
            }
            
            while (elapsedTime < _openCloseAnimationDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                float progress = elapsedTime / _openCloseAnimationDuration;
                float curveValue = _openCloseCurve.Evaluate(progress);
                
                // Animate size
                _rectTransform.sizeDelta = Vector2.Lerp(startSize, targetSize, curveValue);
                
                // Animate content alpha
                if (_contentCanvasGroup != null)
                {
                    _contentCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, curveValue);
                }
                
                yield return null;
            }
            
            // Finalize animation
            _rectTransform.sizeDelta = targetSize;
            if (_contentCanvasGroup != null)
            {
                _contentCanvasGroup.alpha = targetAlpha;
                _contentCanvasGroup.interactable = targetOpen;
                _contentCanvasGroup.blocksRaycasts = targetOpen;
            }
            
            // Deactivate content when fully closed
            if (!targetOpen && _podContentContainer != null)
            {
                _podContentContainer.SetActive(false);
            }
            
            _isOpen = targetOpen;
            _isAnimating = false;
            
            // Fire events
            if (targetOpen)
                OnPodOpened?.Invoke(this);
            else
                OnPodClosed?.Invoke(this);
        }
        
        #endregion
        
        #region Item Management
        
        public void AddItemSlot(GameObject itemSlot)
        {
            if (_itemSlots.Count >= _maxItemsPerPod)
            {
                Debug.LogWarning($"[OneUI_InventoryPod] Pod '{_podTitle}' is at maximum capacity ({_maxItemsPerPod} items)");
                return;
            }
            
            if (itemSlot != null && _itemGridContainer != null)
            {
                itemSlot.transform.SetParent(_itemGridContainer, false);
                _itemSlots.Add(itemSlot);
            }
        }
        
        public void RemoveItemSlot(GameObject itemSlot)
        {
            if (_itemSlots.Contains(itemSlot))
            {
                _itemSlots.Remove(itemSlot);
                if (itemSlot != null)
                {
                    Destroy(itemSlot);
                }
            }
        }
        
        public void ClearAllItems()
        {
            foreach (GameObject item in _itemSlots)
            {
                if (item != null)
                {
                    Destroy(item);
                }
            }
            _itemSlots.Clear();
        }
        
        #endregion
        
        #region Public Configuration
        
        public void ConfigurePod(PodCategory category, string title, Color accentColor)
        {
            _podCategory = category;
            _podTitle = title;
            _podAccentColor = accentColor;
            
            SetupPodVisuals();
        }
        
        #endregion
    }
    
    /// <summary>
    /// Pod categories for inventory organization per Task_26 PRD
    /// </summary>
    public enum PodCategory
    {
        CombatItems,
        Consumables,
        Quest,
        Essence
    }
} 