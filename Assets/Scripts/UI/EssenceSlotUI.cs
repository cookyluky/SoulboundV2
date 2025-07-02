using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SoulBound.Systems;

namespace SoulBound.UI
{
    /// <summary>
    /// UI component for individual essence inventory slots
    /// Displays essence type, amount, and visual representation
    /// Handles selection for conversion operations
    /// </summary>
    public class EssenceSlotUI : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private Button _slotButton;
        [SerializeField] private Image _essenceIcon;
        [SerializeField] private Image _slotBackground;
        [SerializeField] private TextMeshProUGUI _essenceAmountText;
        [SerializeField] private TextMeshProUGUI _essenceTypeText;
        [SerializeField] private GameObject _selectionHighlight;

        [Header("Visual Settings")]
        [SerializeField] private Color _normalBackgroundColor = Color.white;
        [SerializeField] private Color _selectedBackgroundColor = Color.yellow;
        [SerializeField] private Color _emptySlotColor = Color.gray;

        // State
        private EssenceType _essenceType;
        private float _currentAmount;
        private bool _isSelected;
        private Action<EssenceType> _onSlotSelected;

        // Visual data based on essence type
        private struct EssenceVisualData
        {
            public Color iconColor;
            public string displayName;
            public string description;
        }

        private static readonly Dictionary<EssenceType, EssenceVisualData> _essenceVisuals = new()
        {
            { EssenceType.Vitality, new EssenceVisualData 
                { 
                    iconColor = new Color(0.8f, 0.2f, 0.2f), // Red
                    displayName = "Vitality",
                    description = "Life force essence for healing"
                }
            },
            { EssenceType.Strength, new EssenceVisualData 
                { 
                    iconColor = new Color(1.0f, 0.6f, 0.0f), // Orange
                    displayName = "Strength",
                    description = "Physical power essence"
                }
            },
            { EssenceType.Arcane, new EssenceVisualData 
                { 
                    iconColor = new Color(0.2f, 0.4f, 0.8f), // Blue
                    displayName = "Arcane",
                    description = "Magical energy essence"
                }
            },
            { EssenceType.Forbidden, new EssenceVisualData 
                { 
                    iconColor = new Color(0.5f, 0.2f, 0.5f), // Purple
                    displayName = "Forbidden",
                    description = "Dangerous corrupted essence"
                }
            }
        };

        public EssenceType EssenceType => _essenceType;
        public float CurrentAmount => _currentAmount;
        public bool IsSelected => _isSelected;

        private void Awake()
        {
            SetupButton();
        }

        private void SetupButton()
        {
            if (_slotButton != null)
            {
                _slotButton.onClick.AddListener(OnSlotClicked);
            }
        }

        /// <summary>
        /// Initialize the slot with essence type and selection callback
        /// </summary>
        public void Initialize(EssenceType essenceType, float initialAmount, Action<EssenceType> onSlotSelected)
        {
            _essenceType = essenceType;
            _currentAmount = initialAmount;
            _onSlotSelected = onSlotSelected;

            SetupVisuals();
            UpdateDisplay();
        }

        /// <summary>
        /// Update the amount of essence in this slot
        /// </summary>
        public void UpdateAmount(float newAmount)
        {
            _currentAmount = newAmount;
            UpdateDisplay();
        }

        /// <summary>
        /// Set selection state of this slot
        /// </summary>
        public void SetSelected(bool selected)
        {
            _isSelected = selected;
            UpdateSelectionVisuals();
        }

        private void SetupVisuals()
        {
            if (!_essenceVisuals.TryGetValue(_essenceType, out var visualData))
            {
                Debug.LogWarning($"[EssenceSlotUI] No visual data found for essence type: {_essenceType}");
                return;
            }

            // Set essence type text
            if (_essenceTypeText != null)
            {
                _essenceTypeText.text = visualData.displayName;
            }

            // Set icon color
            if (_essenceIcon != null)
            {
                _essenceIcon.color = visualData.iconColor;
            }

            // Setup tooltip (if system exists)
            // TODO: Add tooltip system integration for showing descriptions
        }

        private void UpdateDisplay()
        {
            // Update amount text
            if (_essenceAmountText != null)
            {
                _essenceAmountText.text = _currentAmount > 0 ? $"{_currentAmount:F1}" : "0";
            }

            // Update visual state based on amount
            UpdateSlotVisuals();
        }

        private void UpdateSlotVisuals()
        {
            bool hasEssence = _currentAmount > 0f;

            // Update icon visibility/opacity
            if (_essenceIcon != null)
            {
                var iconColor = _essenceIcon.color;
                iconColor.a = hasEssence ? 1.0f : 0.3f;
                _essenceIcon.color = iconColor;
            }

            // Update background color
            if (_slotBackground != null)
            {
                _slotBackground.color = hasEssence ? _normalBackgroundColor : _emptySlotColor;
            }

            // Update interactability
            if (_slotButton != null)
            {
                _slotButton.interactable = hasEssence;
            }

            // Update selection visuals
            UpdateSelectionVisuals();
        }

        private void UpdateSelectionVisuals()
        {
            // Update selection highlight
            if (_selectionHighlight != null)
            {
                _selectionHighlight.SetActive(_isSelected);
            }

            // Update background for selection
            if (_slotBackground != null && _isSelected && _currentAmount > 0)
            {
                _slotBackground.color = _selectedBackgroundColor;
            }
            else if (_slotBackground != null)
            {
                UpdateSlotVisuals(); // Reset to normal colors
            }
        }

        private void OnSlotClicked()
        {
            if (_currentAmount > 0f)
            {
                _onSlotSelected?.Invoke(_essenceType);
            }
        }

        /// <summary>
        /// Get tooltip text for this essence type
        /// </summary>
        public string GetTooltipText()
        {
            if (_essenceVisuals.TryGetValue(_essenceType, out var visualData))
            {
                return $"{visualData.displayName}\n{visualData.description}\nAmount: {_currentAmount:F1}";
            }
            return $"{_essenceType}\nAmount: {_currentAmount:F1}";
        }

        /// <summary>
        /// Visual feedback for essence addition
        /// </summary>
        public void PlayAdditionEffect()
        {
            // TODO: Add visual effect for when essence is added to this slot
            // Could include scale animation, glow effect, etc.
        }

        /// <summary>
        /// Visual feedback for essence removal
        /// </summary>
        public void PlayRemovalEffect()
        {
            // TODO: Add visual effect for when essence is removed from this slot
            // Could include fade animation, shrink effect, etc.
        }

        #region Context Menu

        [ContextMenu("Simulate Add Essence")]
        private void TestAddEssence()
        {
            UpdateAmount(_currentAmount + 10f);
            PlayAdditionEffect();
        }

        [ContextMenu("Simulate Remove Essence")]
        private void TestRemoveEssence()
        {
            UpdateAmount(Mathf.Max(0f, _currentAmount - 5f));
            PlayRemovalEffect();
        }

        [ContextMenu("Toggle Selection")]
        private void TestToggleSelection()
        {
            SetSelected(!_isSelected);
        }

        #endregion
    }
} 