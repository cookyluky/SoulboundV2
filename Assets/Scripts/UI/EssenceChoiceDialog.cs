using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SoulBound.Systems;
using SoulBound.Core;

namespace SoulBound.UI
{
    /// <summary>
    /// Dialog UI for essence absorption choices
    /// Displays when player can choose between immediate consumption or banking
    /// Shows essence details, benefits, and potential risks
    /// </summary>
    public class EssenceChoiceDialog : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private GameObject _dialogPanel;
        [SerializeField] private TextMeshProUGUI _essenceTypeText;
        [SerializeField] private TextMeshProUGUI _essenceAmountText;
        [SerializeField] private TextMeshProUGUI _essenceDescriptionText;
        [SerializeField] private Image _essenceIcon;

        [Header("Choice Buttons")]
        [SerializeField] private Button _consumeImmediatelyButton;
        [SerializeField] private Button _bankEssenceButton;
        [SerializeField] private Button _cancelButton;

        [Header("Information Displays")]
        [SerializeField] private TextMeshProUGUI _immediateEffectText;
        [SerializeField] private TextMeshProUGUI _bankingEffectText;
        [SerializeField] private TextMeshProUGUI _corruptionWarningText;
        [SerializeField] private GameObject _corruptionWarningPanel;

        [Header("Timer")]
        [SerializeField] private Slider _timeRemainingSlider;
        [SerializeField] private TextMeshProUGUI _timeRemainingText;
        [SerializeField] private Image _timerFillImage;

        [Header("Visual Settings")]
        [SerializeField] private Color _safeEssenceColor = Color.green;
        [SerializeField] private Color _dangerousEssenceColor = Color.red;
        [SerializeField] private float _autoCloseTime = 5f;

        // State
        private AbsorptionOpportunity _currentOpportunity;
        private Action<bool> _onChoiceMade; // true = immediate, false = bank
        private float _timeRemaining;
        private bool _isDialogActive;

        // Component references
        private EssenceManager _essenceManager;
        private AudioManager _audioManager;

        // Events
        public static event Action<AbsorptionOpportunity, bool> OnChoiceMade;

        private void Awake()
        {
            InitializeComponents();
            SetupButtons();
            HideDialog();
        }

        private void Start()
        {
            _essenceManager = ServiceLocator.Get<EssenceManager>();
            _audioManager = ServiceLocator.Get<AudioManager>();
        }

        private void Update()
        {
            if (_isDialogActive)
            {
                UpdateTimer();
            }
        }

        #region Initialization

        private void InitializeComponents()
        {
            // Ensure dialog starts hidden
            if (_dialogPanel != null)
            {
                _dialogPanel.SetActive(false);
            }
        }

        private void SetupButtons()
        {
            if (_consumeImmediatelyButton != null)
            {
                _consumeImmediatelyButton.onClick.AddListener(() => MakeChoice(true));
            }

            if (_bankEssenceButton != null)
            {
                _bankEssenceButton.onClick.AddListener(() => MakeChoice(false));
            }

            if (_cancelButton != null)
            {
                _cancelButton.onClick.AddListener(CancelChoice);
            }
        }

        #endregion

        #region Dialog Control

        /// <summary>
        /// Show choice dialog for essence absorption
        /// </summary>
        public void ShowChoiceDialog(AbsorptionOpportunity opportunity, Action<bool> onChoiceMade)
        {
            _currentOpportunity = opportunity;
            _onChoiceMade = onChoiceMade;
            _timeRemaining = _autoCloseTime;
            _isDialogActive = true;

            UpdateDialogContent();
            ShowDialog();

            // Play dialog open sound
            if (_audioManager != null)
            {
                // _audioManager.PlaySFX("dialog_open"); // When audio clips are added
            }
        }

        /// <summary>
        /// Hide the choice dialog
        /// </summary>
        public void HideDialog()
        {
            _isDialogActive = false;
            
            if (_dialogPanel != null)
            {
                _dialogPanel.SetActive(false);
            }

            // Reset state
            _currentOpportunity = null;
            _onChoiceMade = null;
        }

        private void ShowDialog()
        {
            if (_dialogPanel != null)
            {
                _dialogPanel.SetActive(true);
            }

            // Pause game or set time scale (optional)
            // Time.timeScale = 0.5f; // Slow motion effect during choice
        }

        #endregion

        #region Content Updates

        private void UpdateDialogContent()
        {
            if (_currentOpportunity?.Essence == null) return;

            var essence = _currentOpportunity.Essence;
            var displayInfo = essence.GetDisplayInfo();

            // Basic essence information
            UpdateEssenceInfo(essence, displayInfo);
            
            // Effect previews
            UpdateEffectPreviews(essence);
            
            // Corruption warnings
            UpdateCorruptionWarnings(essence);
            
            // Timer setup
            SetupTimer();
        }

        private void UpdateEssenceInfo(SoulEssence essence, EssenceDisplayInfo displayInfo)
        {
            // Essence type and amount
            if (_essenceTypeText != null)
            {
                _essenceTypeText.text = displayInfo.typeName;
            }

            if (_essenceAmountText != null)
            {
                _essenceAmountText.text = $"Amount: {essence.Quantity:F1}";
            }

            if (_essenceDescriptionText != null)
            {
                _essenceDescriptionText.text = displayInfo.effectPreview;
            }

            // Icon color
            if (_essenceIcon != null)
            {
                _essenceIcon.color = displayInfo.typeColor;
            }
        }

        private void UpdateEffectPreviews(SoulEssence essence)
        {
            // Immediate consumption effects
            if (_immediateEffectText != null)
            {
                var immediateEffects = CalculateImmediateEffects(essence);
                _immediateEffectText.text = $"Immediate Effects:\n{immediateEffects}";
            }

            // Banking effects (90% efficiency)
            if (_bankingEffectText != null)
            {
                float bankingAmount = essence.Quantity * 0.9f; // Banking efficiency
                _bankingEffectText.text = $"Bank for Later:\n{bankingAmount:F1} {essence.Type} essence\n(90% efficiency)";
            }
        }

        private void UpdateCorruptionWarnings(SoulEssence essence)
        {
            bool hasCorruptionRisk = essence.TotalCorruptionRisk > 0.1f;

            if (_corruptionWarningPanel != null)
            {
                _corruptionWarningPanel.SetActive(hasCorruptionRisk);
            }

            if (_corruptionWarningText != null && hasCorruptionRisk)
            {
                _corruptionWarningText.text = $"⚠️ Corruption Risk: {essence.TotalCorruptionRisk:P0}\nProceed with caution!";
            }

            // Update overall dialog color based on danger level
            UpdateDialogDangerLevel(hasCorruptionRisk);
        }

        private void UpdateDialogDangerLevel(bool isDangerous)
        {
            Color targetColor = isDangerous ? _dangerousEssenceColor : _safeEssenceColor;

            if (_timerFillImage != null)
            {
                _timerFillImage.color = targetColor;
            }

            // Could also update border colors, background tints, etc.
        }

        private void SetupTimer()
        {
            if (_timeRemainingSlider != null)
            {
                _timeRemainingSlider.maxValue = _autoCloseTime;
                _timeRemainingSlider.value = _timeRemaining;
            }

            UpdateTimerDisplay();
        }

        private void UpdateTimer()
        {
            _timeRemaining -= Time.unscaledDeltaTime; // Use unscaled time in case game is paused

            if (_timeRemainingSlider != null)
            {
                _timeRemainingSlider.value = _timeRemaining;
            }

            UpdateTimerDisplay();

            // Auto-close dialog when time runs out
            if (_timeRemaining <= 0f)
            {
                AutoChoice();
            }
        }

        private void UpdateTimerDisplay()
        {
            if (_timeRemainingText != null)
            {
                _timeRemainingText.text = $"Time: {_timeRemaining:F1}s";
            }
        }

        #endregion

        #region Choice Handling

        private void MakeChoice(bool consumeImmediately)
        {
            if (_currentOpportunity == null) return;

            // Invoke callbacks
            _onChoiceMade?.Invoke(consumeImmediately);
            OnChoiceMade?.Invoke(_currentOpportunity, consumeImmediately);

            // Play choice sound
            if (_audioManager != null)
            {
                string soundName = consumeImmediately ? "essence_consume" : "essence_bank";
                // _audioManager.PlaySFX(soundName); // When audio clips are added
            }

            HideDialog();

            // Restore normal time scale
            // Time.timeScale = 1f;
        }

        private void CancelChoice()
        {
            // For now, canceling defaults to banking (safer choice)
            MakeChoice(false);
        }

        private void AutoChoice()
        {
            // Default to banking when time runs out (safer choice)
            MakeChoice(false);
        }

        #endregion

        #region Effect Calculations

        private string CalculateImmediateEffects(SoulEssence essence)
        {
            var immediateEffects = essence.GetImmediateEffects();
            string effects = "";

            if (immediateEffects.healthRestore > 0)
                effects += $"+{immediateEffects.healthRestore:F1} Health\n";
            
            if (immediateEffects.manaRestore > 0)
                effects += $"+{immediateEffects.manaRestore:F1} Mana\n";
            
            if (immediateEffects.experienceGain > 0)
                effects += $"+{immediateEffects.experienceGain:F1} Experience\n";
            
            if (immediateEffects.corruption > 0)
                effects += $"⚠️ +{immediateEffects.corruption:F1} Corruption";

            return effects.Trim();
        }

        #endregion

        #region Public Utilities

        public bool IsDialogActive => _isDialogActive;
        public AbsorptionOpportunity CurrentOpportunity => _currentOpportunity;

        /// <summary>
        /// Force close the dialog (useful for scene transitions)
        /// </summary>
        public void ForceClose()
        {
            if (_isDialogActive)
            {
                CancelChoice();
            }
        }

        #endregion

        #region Debug and Context Menu

        [ContextMenu("Test Safe Essence Dialog")]
        private void TestSafeEssenceDialog()
        {
            var testEssence = new SoulEssence(EssenceType.Vitality, 25f, 1.0f);
            var testOpportunity = new AbsorptionOpportunity
            {
                Essence = testEssence,
                Position = Vector3.zero,
                TimeRemaining = 3f,
                IsActive = true
            };

            ShowChoiceDialog(testOpportunity, (choice) => 
            {
                Debug.Log($"Test choice made: {(choice ? "Immediate" : "Bank")}");
            });
        }

        [ContextMenu("Test Forbidden Essence Dialog")]
        private void TestForbiddenEssenceDialog()
        {
            var testEssence = new SoulEssence(EssenceType.Forbidden, 15f, 1.5f);
            var testOpportunity = new AbsorptionOpportunity
            {
                Essence = testEssence,
                Position = Vector3.zero,
                TimeRemaining = 3f,
                IsActive = true
            };

            ShowChoiceDialog(testOpportunity, (choice) => 
            {
                Debug.Log($"Test forbidden choice made: {(choice ? "Immediate" : "Bank")}");
            });
        }

        #endregion
    }
} 