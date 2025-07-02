using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SoulBound.Systems;
using SoulBound.Core;

namespace SoulBound.UI
{
    /// <summary>
    /// IMPROVED VERSION: Essence Inventory UI with better layout management
    /// Displays banked essence amounts and provides conversion options
    /// Auto-manages slot creation and responsive layout
    /// </summary>
    public class EssenceInventoryUI : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private GameObject _inventoryPanel;
        [SerializeField] private CanvasGroup _inventoryCanvasGroup; // For smooth fade in/out

        [Header("Essence Display - AUTO-MANAGED")]
        [SerializeField] private Transform _essenceSlotContainer;
        [SerializeField] private GameObject _essenceSlotPrefab;
        [SerializeField] private GridLayoutGroup _gridLayout; // AUTO-SCALING GRID!

        [Header("Controls - SIMPLIFIED")]
        [SerializeField] private Button _toggleInventoryButton;
        [SerializeField] private Button _closeInventoryButton;
        [SerializeField] private TextMeshProUGUI _totalEssenceText;

        [Header("Conversion Panel - RESPONSIVE")]
        [SerializeField] private GameObject _conversionPanel;
        [SerializeField] private TextMeshProUGUI _conversionTitle;
        [SerializeField] private Slider _conversionSlider;
        [SerializeField] private TextMeshProUGUI _conversionAmountText;
        [SerializeField] private TextMeshProUGUI _conversionResultText;
        [SerializeField] private Button _confirmConversionButton;
        [SerializeField] private Button _cancelConversionButton;

        // AUTO-MANAGED STATE
        private Dictionary<EssenceType, EssenceSlotUI> _essenceSlots = new();
        private bool _isInventoryVisible;
        private EssenceType _selectedEssenceType;
        private EssenceConversionType _currentConversionType;
        private EssenceManager _essenceManager;

        // RESPONSIVE LAYOUT SETTINGS
        [Header("Auto-Layout Settings")]
        [SerializeField] private int _maxSlotsPerRow = 2;
        [SerializeField] private float _slotSpacing = 10f;
        [SerializeField] private float _animationDuration = 0.3f;

        private void Awake()
        {
            SetupAutoLayout();
            SetupButtons();
            HideInventoryImmediate();
        }

        private void Start()
        {
            _essenceManager = ServiceLocator.Get<EssenceManager>();
            if (_essenceManager != null)
            {
                EssenceManager.OnBankedEssenceChanged += OnBankedEssenceChanged;
                InitializeEssenceSlots();
            }
        }

        #region AUTO-LAYOUT SETUP

        private void SetupAutoLayout()
        {
            // AUTO-CONFIGURE GRID LAYOUT
            if (_gridLayout == null && _essenceSlotContainer != null)
            {
                _gridLayout = _essenceSlotContainer.GetComponent<GridLayoutGroup>();
                if (_gridLayout == null)
                {
                    _gridLayout = _essenceSlotContainer.gameObject.AddComponent<GridLayoutGroup>();
                }
            }

            if (_gridLayout != null)
            {
                // RESPONSIVE GRID SETTINGS
                _gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                _gridLayout.constraintCount = _maxSlotsPerRow;
                _gridLayout.spacing = new Vector2(_slotSpacing, _slotSpacing);
                _gridLayout.childAlignment = TextAnchor.UpperCenter;
                
                // AUTO-SIZE CELLS BASED ON CONTAINER
                var containerRect = _essenceSlotContainer.GetComponent<RectTransform>();
                if (containerRect != null)
                {
                    float cellWidth = (containerRect.rect.width - (_slotSpacing * (_maxSlotsPerRow - 1))) / _maxSlotsPerRow;
                    _gridLayout.cellSize = new Vector2(cellWidth, cellWidth * 1.2f); // Slightly taller than wide
                }
            }

            // SETUP CANVAS GROUP FOR SMOOTH ANIMATIONS
            if (_inventoryCanvasGroup == null && _inventoryPanel != null)
            {
                _inventoryCanvasGroup = _inventoryPanel.GetComponent<CanvasGroup>();
                if (_inventoryCanvasGroup == null)
                {
                    _inventoryCanvasGroup = _inventoryPanel.AddComponent<CanvasGroup>();
                }
            }
        }

        #endregion

        #region SIMPLIFIED BUTTON SETUP

        private void SetupButtons()
        {
            // MAIN TOGGLE BUTTON
            if (_toggleInventoryButton != null)
            {
                _toggleInventoryButton.onClick.RemoveAllListeners();
                _toggleInventoryButton.onClick.AddListener(ToggleInventory);
            }

            // CLOSE BUTTON
            if (_closeInventoryButton != null)
            {
                _closeInventoryButton.onClick.RemoveAllListeners();
                _closeInventoryButton.onClick.AddListener(HideInventory);
            }

            // CONVERSION BUTTONS
            if (_confirmConversionButton != null)
            {
                _confirmConversionButton.onClick.RemoveAllListeners();
                _confirmConversionButton.onClick.AddListener(ConfirmConversion);
            }

            if (_cancelConversionButton != null)
            {
                _cancelConversionButton.onClick.RemoveAllListeners();
                _cancelConversionButton.onClick.AddListener(CancelConversion);
            }

            // CONVERSION SLIDER
            if (_conversionSlider != null)
            {
                _conversionSlider.onValueChanged.RemoveAllListeners();
                _conversionSlider.onValueChanged.AddListener(OnConversionSliderChanged);
            }
        }

        #endregion

        #region AUTO-MANAGED SLOT CREATION

        private void InitializeEssenceSlots()
        {
            // AUTO-CREATE SLOTS FOR ALL ESSENCE TYPES
            foreach (EssenceType essenceType in System.Enum.GetValues(typeof(EssenceType)))
            {
                CreateEssenceSlot(essenceType);
            }

            // FORCE LAYOUT REBUILD
            if (_gridLayout != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(_essenceSlotContainer.GetComponent<RectTransform>());
            }

            UpdateEssenceDisplay();
        }

        private void CreateEssenceSlot(EssenceType essenceType)
        {
            if (_essenceSlotPrefab == null || _essenceSlotContainer == null)
            {
                Debug.LogError("[EssenceInventoryUI] Missing slot prefab or container!");
                return;
            }

            // INSTANTIATE AND AUTO-CONFIGURE
            GameObject slotObject = Instantiate(_essenceSlotPrefab, _essenceSlotContainer);
            EssenceSlotUI slotUI = slotObject.GetComponent<EssenceSlotUI>();

            if (slotUI != null)
            {
                float currentAmount = _essenceManager?.GetBankedEssence(essenceType) ?? 0f;
                slotUI.Initialize(essenceType, currentAmount, OnEssenceSlotSelected);
                _essenceSlots[essenceType] = slotUI;
            }
            else
            {
                Debug.LogError("[EssenceInventoryUI] EssenceSlotUI component missing from prefab!");
                Destroy(slotObject);
            }
        }

        #endregion

        #region SMOOTH ANIMATIONS

        /// <summary>
        /// SMOOTH TOGGLE with fade animation
        /// </summary>
        public void ToggleInventory()
        {
            if (_isInventoryVisible)
            {
                HideInventory();
            }
            else
            {
                ShowInventory();
            }
        }

        public void ShowInventory()
        {
            _isInventoryVisible = true;
            
            if (_inventoryPanel != null)
            {
                _inventoryPanel.SetActive(true);
            }

            // SMOOTH FADE IN
            if (_inventoryCanvasGroup != null)
            {
                StartCoroutine(FadeCanvasGroup(_inventoryCanvasGroup, 0f, 1f, _animationDuration));
            }

            UpdateEssenceDisplay();
        }

        public void HideInventory()
        {
            _isInventoryVisible = false;

            // SMOOTH FADE OUT
            if (_inventoryCanvasGroup != null)
            {
                StartCoroutine(FadeCanvasGroup(_inventoryCanvasGroup, 1f, 0f, _animationDuration, () => {
                    if (_inventoryPanel != null)
                    {
                        _inventoryPanel.SetActive(false);
                    }
                }));
            }
            else
            {
                HideInventoryImmediate();
            }

            HideConversionPanel();
        }

        private void HideInventoryImmediate()
        {
            _isInventoryVisible = false;
            if (_inventoryPanel != null)
            {
                _inventoryPanel.SetActive(false);
            }
        }

        private System.Collections.IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration, System.Action onComplete = null)
        {
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
                yield return null;
            }
            
            canvasGroup.alpha = endAlpha;
            onComplete?.Invoke();
        }

        #endregion

        #region Essence Slot Management

        private void OnBankedEssenceChanged(Dictionary<EssenceType, float> bankedEssence)
        {
            UpdateEssenceDisplay();
        }

        private void OnEssenceSlotSelected(EssenceType essenceType)
        {
            // Clear previous selections
            foreach (var slot in _essenceSlots.Values)
            {
                slot.SetSelected(false);
            }

            // Set new selection
            if (_essenceSlots.TryGetValue(essenceType, out var selectedSlot))
            {
                selectedSlot.SetSelected(true);
                _selectedEssenceType = essenceType;
                
                // Show conversion options
                ShowConversionOptions(essenceType);
            }
        }

        #endregion

        #region Conversion System

        private void ShowConversionOptions(EssenceType essenceType)
        {
            if (_conversionPanel == null) return;

            _conversionPanel.SetActive(true);
            
            if (_conversionTitle != null)
            {
                _conversionTitle.text = $"Convert {essenceType} Essence";
            }

            if (_conversionSlider != null)
            {
                float maxAmount = _essenceManager?.GetBankedEssence(essenceType) ?? 0f;
                _conversionSlider.maxValue = maxAmount;
                _conversionSlider.value = 0f;
            }

            UpdateConversionPreview();
        }

        private void HideConversionPanel()
        {
            if (_conversionPanel != null)
            {
                _conversionPanel.SetActive(false);
            }
        }

        private void OnConversionSliderChanged(float value)
        {
            UpdateConversionPreview();
        }

        private void UpdateConversionPreview()
        {
            if (_conversionSlider == null) return;

            float amount = _conversionSlider.value;
            
            if (_conversionAmountText != null)
            {
                _conversionAmountText.text = $"Amount: {amount:F1}";
            }

            // Calculate conversion result based on essence type
            string result = CalculateConversionResult(_selectedEssenceType, amount);
            if (_conversionResultText != null)
            {
                _conversionResultText.text = result;
            }
        }

        private string CalculateConversionResult(EssenceType essenceType, float amount)
        {
            // Simple conversion rates for now
            switch (essenceType)
            {
                case EssenceType.Vitality:
                    int health = Mathf.RoundToInt(amount * 10f);
                    return $"→ {health} Health";
                
                case EssenceType.Arcane:
                    int mana = Mathf.RoundToInt(amount * 8f);
                    return $"→ {mana} Mana";
                
                case EssenceType.Strength:
                    int experience = Mathf.RoundToInt(amount * 5f);
                    return $"→ {experience} XP";
                
                case EssenceType.Forbidden:
                    int power = Mathf.RoundToInt(amount * 15f);
                    int corruption = Mathf.RoundToInt(amount * 2f);
                    return $"→ {power} Power\n⚠️ +{corruption} Corruption";
                
                default:
                    return "Unknown conversion";
            }
        }

        private void ConfirmConversion()
        {
            if (_conversionSlider == null || _essenceManager == null) return;

            float amount = _conversionSlider.value;
            if (amount > 0f)
            {
                // Determine conversion type based on essence type
                EssenceConversionType conversionType = _selectedEssenceType switch
                {
                    EssenceType.Vitality => EssenceConversionType.Health,
                    EssenceType.Arcane => EssenceConversionType.Mana,
                    EssenceType.Strength => EssenceConversionType.Experience,
                    EssenceType.Forbidden => EssenceConversionType.SoulEnergy,
                    _ => EssenceConversionType.Health
                };

                // Perform the conversion using correct API
                bool success = _essenceManager.ConvertBankedEssence(_selectedEssenceType, amount, conversionType);
                
                if (success)
                {
                    Debug.Log($"[EssenceInventoryUI] Converted {amount:F1} {_selectedEssenceType} essence");
                    HideConversionPanel();
                    
                    // Clear selection
                    foreach (var slot in _essenceSlots.Values)
                    {
                        slot.SetSelected(false);
                    }
                }
                else
                {
                    Debug.LogWarning("[EssenceInventoryUI] Conversion failed - insufficient essence");
                }
            }
        }

        private void CancelConversion()
        {
            HideConversionPanel();
            
            // Clear selection
            foreach (var slot in _essenceSlots.Values)
            {
                slot.SetSelected(false);
            }
        }

        #endregion

        private void UpdateEssenceDisplay()
        {
            if (_essenceManager == null) return;

            var bankedEssence = _essenceManager.BankedEssence;
            float totalEssence = 0f;

            // UPDATE ALL SLOTS
            foreach (var kvp in _essenceSlots)
            {
                float amount = bankedEssence.TryGetValue(kvp.Key, out float value) ? value : 0f;
                kvp.Value.UpdateAmount(amount);
                totalEssence += amount;
            }

            // UPDATE TOTAL DISPLAY
            if (_totalEssenceText != null)
            {
                _totalEssenceText.text = $"Total Essence: {totalEssence:F1}";
            }
        }

        private void OnDestroy()
        {
            // Fix: Use static event unsubscription
            EssenceManager.OnBankedEssenceChanged -= OnBankedEssenceChanged;
        }

        // Helper properties
        public bool IsInventoryVisible => _isInventoryVisible;
    }
} 