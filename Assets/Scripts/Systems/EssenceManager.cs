using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulBound.Core;

namespace SoulBound.Systems
{
    /// <summary>
    /// Central manager for the soul essence collection and banking system
    /// 
    /// IMPORTANT ARCHITECTURE NOTE:
    /// This system handles ESSENCE collection (immediate consumables for health/mana restoration).
    /// This is SEPARATE from the SOUL system which will handle enemy-specific souls for skill tree progression.
    /// 
    /// ESSENCE SYSTEM (this implementation):
    /// - Immediate consumables dropped by enemies
    /// - Can be consumed immediately for health/mana/XP restoration
    /// - Can be banked for later use with 90% efficiency
    /// - Four types: Vitality, Strength, Arcane, Forbidden
    /// 
    /// SOUL SYSTEM (future implementation):
    /// - Enemy-specific souls for skill tree progression
    /// - Each enemy type drops specific soul types
    /// - Used with XP to unlock abilities in type-specific skill trees
    /// - Some abilities require souls from multiple trees for combination skills
    /// 
    /// Coordinates with PlayerController for immediate consumption vs banking decisions
    /// Handles timing windows, visual effects, and essence inventory management
    /// </summary>
    public class EssenceManager : BaseManager
    {
        [Header("Absorption Settings")]
        [SerializeField] private float _absorptionTimeWindow = 3.0f;
        // [SerializeField] private float _baseAbsorptionRate = 1.0f; // TODO: Implement absorption rate modifiers
        [SerializeField] private bool _requirePlayerInput = true;
        [SerializeField] private float _autoAbsorptionDelay = 1.5f;

        [Header("Banking System")]
        [SerializeField] private float _maxAbsorptionRange = 5.0f;
        [SerializeField] private float _maxBankCapacityPerType = 100f;
        [SerializeField] private float _bankingEfficiency = 0.9f;
        // [SerializeField] private bool _enableEssenceDecay = false; // TODO: Implement essence decay system
        // [SerializeField] private float _essenceDecayRate = 0.01f; // TODO: Implement essence decay rate

        [Header("Visual Effects")]
        [SerializeField] private GameObject _essenceParticlePrefab;
        [SerializeField] private AudioClip _absorptionSound;
        [SerializeField] private AudioClip _bankingSound;
        [SerializeField] private AudioClip _corruptionWarningSound;

        [Header("Debug Settings")]
        [SerializeField] private bool _showAbsorptionGizmos = true;

        // Essence tracking
        private Dictionary<EssenceType, float> _bankedEssence = new();
        private Dictionary<EssenceType, List<SoulEssence>> _pendingEssence = new();

        // Active absorption opportunities
        private List<AbsorptionOpportunity> _activeOpportunities = new();

        // Component references - Using lazy initialization to avoid dependency issues
        private PlayerController _playerController;
        private AudioManager _audioManager;

        // Events for UI and other systems
        public static event Action<SoulEssence> OnEssenceDropped;
        public static event Action<SoulEssence, bool> OnEssenceAbsorbed; // essence, isImmediate
        public static event Action<EssenceType, float> OnEssenceBanked;
        public static event Action<Dictionary<EssenceType, float>> OnBankedEssenceChanged;
        public static event Action<AbsorptionOpportunity> OnAbsorptionOpportunityAvailable;
        public static event Action<AbsorptionOpportunity> OnAbsorptionOpportunityExpired;

        // Public Properties
        public Dictionary<EssenceType, float> BankedEssence => new(_bankedEssence);
        public float AbsorptionTimeWindow => _absorptionTimeWindow;
        public bool RequiresPlayerInput => _requirePlayerInput;
        public int ActiveOpportunityCount => _activeOpportunities.Count;

        protected override void OnInitialize()
        {
            // Initialize banked essence dictionary
            foreach (EssenceType type in Enum.GetValues(typeof(EssenceType)))
            {
                _bankedEssence[type] = 0f;
                _pendingEssence[type] = new List<SoulEssence>();
            }

            // Don't get component references here - use lazy initialization instead
            // This prevents dependency issues since PlayerController registers after managers initialize

            if (_enableDebugLogging)
            {
                LogInfo($"Initialized with absorption window: {_absorptionTimeWindow}s");
                LogInfo("Using lazy initialization for PlayerController and AudioManager references");
                LogInfo("ESSENCE SYSTEM: Handling immediate consumables (health/mana restoration)");
                LogInfo("NOTE: This is separate from the future SOUL SYSTEM for skill tree progression");
            }
        }

        /// <summary>
        /// Lazy initialization for PlayerController reference
        /// Gets reference when first needed to avoid initialization order issues
        /// </summary>
        private PlayerController GetPlayerController()
        {
            if (_playerController == null)
            {
                _playerController = ServiceLocator.Get<PlayerController>();
                if (_playerController == null)
                {
                    // Fallback to finding in scene if not in ServiceLocator yet
                    _playerController = FindFirstObjectByType<PlayerController>();
                }
                
                if (_playerController != null && _enableDebugLogging)
                {
                    LogInfo("PlayerController reference acquired");
                }
            }
            return _playerController;
        }

        /// <summary>
        /// Lazy initialization for AudioManager reference
        /// Gets reference when first needed to avoid initialization order issues
        /// </summary>
        private AudioManager GetAudioManager()
        {
            if (_audioManager == null)
            {
                _audioManager = ServiceLocator.Get<AudioManager>();
                if (_audioManager != null && _enableDebugLogging)
                {
                    LogInfo("AudioManager reference acquired");
                }
            }
            return _audioManager;
        }

        protected override void OnCleanup()
        {
            // Clear all pending opportunities
            foreach (var opportunity in _activeOpportunities)
            {
                StopCoroutine(opportunity.ExpirationCoroutine);
            }
            _activeOpportunities.Clear();
        }

        #region Essence Drop and Collection

        /// <summary>
        /// Called when an enemy is defeated or essence source is triggered
        /// Creates an absorption opportunity with timing window
        /// </summary>
        public void OnEnemyDefeated(Vector3 position, int enemyLevel, EssenceType essenceType, float qualityMultiplier = 1.0f)
        {
            var essence = SoulEssence.CreateFromSource(essenceType, enemyLevel, qualityMultiplier);
            var scaledEssence = essence.ScaleToPlayerLevel(GetPlayerController().CurrentLevel, GetPlayerAbsorptionBonus());
            
            CreateAbsorptionOpportunity(scaledEssence, position);
            
            if (_enableDebugLogging)
            {
                Debug.Log($"[EssenceManager] Enemy defeated - {scaledEssence} at {position}");
            }
        }

        /// <summary>
        /// Create multiple essence drops from a powerful enemy
        /// </summary>
        public void OnEliteEnemyDefeated(Vector3 position, int enemyLevel, EssenceType[] essenceTypes, float qualityMultiplier = 1.5f)
        {
            foreach (var essenceType in essenceTypes)
            {
                OnEnemyDefeated(position + UnityEngine.Random.insideUnitSphere * 0.5f, enemyLevel, essenceType, qualityMultiplier);
            }
        }

        /// <summary>
        /// Create a timed absorption opportunity for the player
        /// </summary>
        private void CreateAbsorptionOpportunity(SoulEssence essence, Vector3 position)
        {
            var opportunity = new AbsorptionOpportunity
            {
                Essence = essence,
                Position = position,
                TimeRemaining = _absorptionTimeWindow,
                IsActive = true
            };

            // Start expiration coroutine
            opportunity.ExpirationCoroutine = StartCoroutine(AbsorptionOpportunityCoroutine(opportunity));
            _activeOpportunities.Add(opportunity);

            // Spawn visual effects
            SpawnEssenceVisual(essence, position);
            
            // Notify systems
            OnEssenceDropped?.Invoke(essence);
            OnAbsorptionOpportunityAvailable?.Invoke(opportunity);

            if (_enableDebugLogging)
            {
                Debug.Log($"[EssenceManager] Absorption opportunity created: {essence} - {_absorptionTimeWindow}s window");
            }
        }

        /// <summary>
        /// Coroutine that manages the timing window for essence absorption
        /// </summary>
        private IEnumerator AbsorptionOpportunityCoroutine(AbsorptionOpportunity opportunity)
        {
            float timeElapsed = 0f;
            
            while (timeElapsed < _absorptionTimeWindow && opportunity.IsActive)
            {
                timeElapsed += Time.deltaTime;
                opportunity.TimeRemaining = _absorptionTimeWindow - timeElapsed;
                
                // Auto-absorption check (if not requiring player input or after delay)
                if (!_requirePlayerInput && timeElapsed >= _autoAbsorptionDelay)
                {
                    AbsorbEssence(opportunity, true); // Auto-consume immediately
                    yield break;
                }
                
                yield return null;
            }

            // Opportunity expired
            if (opportunity.IsActive)
            {
                ExpireAbsorptionOpportunity(opportunity);
            }
        }

        #endregion

        #region Essence Absorption

        /// <summary>
        /// Player-triggered essence absorption with choice of immediate consumption or banking
        /// </summary>
        public bool TryAbsorbEssence(Vector3 playerPosition, bool consumeImmediately)
        {
            var nearestOpportunity = GetNearestAbsorptionOpportunity(playerPosition);
            if (nearestOpportunity != null)
            {
                return AbsorbEssence(nearestOpportunity, consumeImmediately);
            }
            
            return false;
        }

        /// <summary>
        /// Absorb a specific essence opportunity
        /// </summary>
        public bool AbsorbEssence(AbsorptionOpportunity opportunity, bool consumeImmediately)
        {
            if (!opportunity.IsActive)
                return false;

            var essence = opportunity.Essence;
            
            // Check if player can absorb souls
            if (!GetPlayerController().CanAbsorbSouls)
            {
                if (_enableDebugLogging)
                {
                    Debug.LogWarning("[EssenceManager] Player cannot absorb souls in current state");
                }
                return false;
            }

            // Apply absorption
            if (consumeImmediately)
            {
                ConsumeEssenceImmediately(essence);
            }
            else
            {
                BankEssence(essence);
            }

            // Complete the opportunity
            CompleteAbsorptionOpportunity(opportunity);
            
            // Notify systems
            OnEssenceAbsorbed?.Invoke(essence, consumeImmediately);
            
            // Play audio feedback
            PlayAbsorptionFeedback(essence, consumeImmediately);

            if (_enableDebugLogging)
            {
                Debug.Log($"[EssenceManager] Absorbed {essence} - Immediate: {consumeImmediately}");
            }

            return true;
        }

        /// <summary>
        /// Immediately consume essence for instant effects
        /// </summary>
        private void ConsumeEssenceImmediately(SoulEssence essence)
        {
            var effects = essence.GetImmediateEffects();
            
            // Apply effects through PlayerController
            if (effects.healthRestore > 0)
                GetPlayerController().Heal(effects.healthRestore);
            
            if (effects.manaRestore > 0)
                GetPlayerController().RestoreMana(effects.manaRestore);
            
            if (effects.experienceGain > 0)
                GetPlayerController().AddExperience(effects.experienceGain);
            
            if (effects.corruption > 0)
                GetPlayerController().AddCorruption(effects.corruption);

            // Also call the PlayerController's AbsorbSoul method for compatibility
            GetPlayerController().AbsorbSoul(essence.EffectiveValue, essence.TotalCorruptionRisk, true);
        }

        /// <summary>
        /// Bank essence for later use
        /// </summary>
        private void BankEssence(SoulEssence essence)
        {
            float bankingValue = essence.GetBankingValue() * _bankingEfficiency;
            float currentBanked = _bankedEssence[essence.Type];
            
            // Check capacity
            if (currentBanked + bankingValue > _maxBankCapacityPerType)
            {
                bankingValue = _maxBankCapacityPerType - currentBanked;
                if (bankingValue <= 0)
                {
                    if (_enableDebugLogging)
                    {
                        Debug.LogWarning($"[EssenceManager] Banking capacity full for {essence.Type}");
                    }
                    return;
                }
            }

            // Bank the essence
            _bankedEssence[essence.Type] += bankingValue;
            
            // Update PlayerController's soul energy (for compatibility)
            GetPlayerController().AbsorbSoul(bankingValue * 0.5f, essence.TotalCorruptionRisk * 0.3f, false);
            
            // Notify systems
            OnEssenceBanked?.Invoke(essence.Type, bankingValue);
            OnBankedEssenceChanged?.Invoke(new Dictionary<EssenceType, float>(_bankedEssence));

            if (_enableDebugLogging)
            {
                Debug.Log($"[EssenceManager] Banked {bankingValue:F1} {essence.Type} essence (Total: {_bankedEssence[essence.Type]:F1})");
            }
        }

        #endregion

        #region Banking System

        /// <summary>
        /// Use banked essence for various purposes
        /// </summary>
        public bool UseBankedEssence(EssenceType type, float amount)
        {
            if (_bankedEssence[type] >= amount)
            {
                _bankedEssence[type] -= amount;
                OnBankedEssenceChanged?.Invoke(new Dictionary<EssenceType, float>(_bankedEssence));
                
                if (_enableDebugLogging)
                {
                    Debug.Log($"[EssenceManager] Used {amount:F1} banked {type} essence (Remaining: {_bankedEssence[type]:F1})");
                }
                
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Get the amount of banked essence for a specific type
        /// </summary>
        public float GetBankedEssence(EssenceType type)
        {
            return _bankedEssence[type];
        }

        /// <summary>
        /// Convert banked essence to player benefits
        /// </summary>
        public bool ConvertBankedEssence(EssenceType type, float amount, EssenceConversionType conversionType)
        {
            if (!UseBankedEssence(type, amount))
                return false;

            // Apply conversion effects based on type and conversion method
            float conversionEfficiency = GetConversionEfficiency(type, conversionType);
            float effectValue = amount * conversionEfficiency;

            switch (conversionType)
            {
                case EssenceConversionType.Health:
                    GetPlayerController().Heal(effectValue);
                    break;
                case EssenceConversionType.Mana:
                    GetPlayerController().RestoreMana(effectValue);
                    break;
                case EssenceConversionType.Experience:
                    GetPlayerController().AddExperience(effectValue);
                    break;
                case EssenceConversionType.SoulEnergy:
                    GetPlayerController().AbsorbSoul(effectValue, 0f, false);
                    break;
            }

            if (_enableDebugLogging)
            {
                Debug.Log($"[EssenceManager] Converted {amount:F1} {type} to {conversionType} (Effect: {effectValue:F1})");
            }

            return true;
        }

        /// <summary>
        /// Get essence data for saving to PlayerStats
        /// </summary>
        public EssenceData GetEssenceData()
        {
            return EssenceData.FromDictionary(_bankedEssence);
        }

        /// <summary>
        /// Set essence data from loaded PlayerStats
        /// </summary>
        public void SetEssenceData(EssenceData essenceData)
        {
            var newBankedEssence = essenceData.ToDictionary();
            
            // Update banked essence amounts
            foreach (var kvp in newBankedEssence)
            {
                _bankedEssence[kvp.Key] = kvp.Value;
            }

            // Notify UI systems of the change
            OnBankedEssenceChanged?.Invoke(new Dictionary<EssenceType, float>(_bankedEssence));

            if (_enableDebugLogging)
            {
                LogInfo($"Loaded essence data - Total: {essenceData.GetTotalEssence():F1}");
                foreach (var kvp in _bankedEssence)
                {
                    if (kvp.Value > 0)
                    {
                        LogInfo($"  {kvp.Key}: {kvp.Value:F1}");
                    }
                }
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Find the nearest absorption opportunity to the player
        /// </summary>
        private AbsorptionOpportunity GetNearestAbsorptionOpportunity(Vector3 playerPosition)
        {
            AbsorptionOpportunity nearest = null;
            float nearestDistance = float.MaxValue;

            foreach (var opportunity in _activeOpportunities)
            {
                if (!opportunity.IsActive) continue;
                
                float distance = Vector3.Distance(playerPosition, opportunity.Position);
                if (distance < nearestDistance && distance <= _maxAbsorptionRange)
                {
                    nearestDistance = distance;
                    nearest = opportunity;
                }
            }

            return nearest;
        }

        /// <summary>
        /// Complete an absorption opportunity and clean up
        /// </summary>
        private void CompleteAbsorptionOpportunity(AbsorptionOpportunity opportunity)
        {
            opportunity.IsActive = false;
            
            if (opportunity.ExpirationCoroutine != null)
            {
                StopCoroutine(opportunity.ExpirationCoroutine);
            }
            
            _activeOpportunities.Remove(opportunity);
            
            // Remove visual effects
            DestroyEssenceVisual(opportunity);
        }

        /// <summary>
        /// Expire an absorption opportunity that wasn't collected in time
        /// </summary>
        private void ExpireAbsorptionOpportunity(AbsorptionOpportunity opportunity)
        {
            if (_enableDebugLogging)
            {
                Debug.Log($"[EssenceManager] Absorption opportunity expired: {opportunity.Essence}");
            }
            
            OnAbsorptionOpportunityExpired?.Invoke(opportunity);
            CompleteAbsorptionOpportunity(opportunity);
        }

        /// <summary>
        /// Get player's current absorption bonus based on level and upgrades
        /// </summary>
        private float GetPlayerAbsorptionBonus()
        {
            // Base bonus from player level
            float levelBonus = (GetPlayerController().CurrentLevel - 1) * 0.02f;
            
            // Additional bonuses could be added here (equipment, skills, etc.)
            return levelBonus;
        }

        /// <summary>
        /// Get conversion efficiency for different essence types and conversion methods
        /// </summary>
        private float GetConversionEfficiency(EssenceType essenceType, EssenceConversionType conversionType)
        {
            // Base efficiency table
            return (essenceType, conversionType) switch
            {
                (EssenceType.Vitality, EssenceConversionType.Health) => 1.2f,
                (EssenceType.Vitality, EssenceConversionType.Mana) => 0.6f,
                (EssenceType.Strength, EssenceConversionType.Experience) => 1.0f,
                (EssenceType.Arcane, EssenceConversionType.Mana) => 1.3f,
                (EssenceType.Arcane, EssenceConversionType.SoulEnergy) => 1.1f,
                (EssenceType.Forbidden, _) => 1.5f, // Powerful but risky
                _ => 0.8f // Default efficiency
            };
        }

        #endregion

        #region Visual and Audio Effects

        /// <summary>
        /// Spawn visual effects for essence drop
        /// </summary>
        private void SpawnEssenceVisual(SoulEssence essence, Vector3 position)
        {
            if (_essenceParticlePrefab != null)
            {
                var visual = Instantiate(_essenceParticlePrefab, position, Quaternion.identity);
                var displayInfo = essence.GetDisplayInfo();
                
                // Configure visual based on essence type
                var particleSystem = visual.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    var main = particleSystem.main;
                    main.startColor = displayInfo.typeColor;
                }
                
                // Store reference for cleanup
                if (visual.TryGetComponent<EssenceVisual>(out var essenceVisual))
                {
                    essenceVisual.Initialize(essence, _absorptionTimeWindow);
                }
            }
        }

        /// <summary>
        /// Remove visual effects when opportunity is completed
        /// </summary>
        private void DestroyEssenceVisual(AbsorptionOpportunity opportunity)
        {
            // Find and destroy visual at opportunity position
            var visuals = FindObjectsByType<EssenceVisual>(FindObjectsSortMode.None);
            foreach (var visual in visuals)
            {
                if (Vector3.Distance(visual.transform.position, opportunity.Position) < 0.1f)
                {
                    visual.PlayAbsorptionEffect();
                    break;
                }
            }
        }

        /// <summary>
        /// Play audio feedback for essence absorption
        /// </summary>
        private void PlayAbsorptionFeedback(SoulEssence essence, bool isImmediate)
        {
            var audioManager = GetAudioManager();
            if (audioManager == null) return;

            AudioClip clipToPlay = isImmediate ? _absorptionSound : _bankingSound;
            
            if (essence.TotalCorruptionRisk > 0.1f && _corruptionWarningSound != null)
            {
                audioManager.PlaySFX(_corruptionWarningSound);
            }
            
            if (clipToPlay != null)
            {
                audioManager.PlaySFX(clipToPlay);
            }
        }

        #endregion

        #region Debug and Testing

        /// <summary>
        /// Get debug information about current essence state
        /// </summary>
        public override string GetDebugInfo()
        {
            var info = $"Active Opportunities: {_activeOpportunities.Count}\n";
            info += "Banked Essence:\n";
            
            foreach (var kvp in _bankedEssence)
            {
                if (kvp.Value > 0)
                {
                    info += $"  {kvp.Key}: {kvp.Value:F1}\n";
                }
            }
            
            return info;
        }

        [ContextMenu("Simulate Vitality Essence Drop")]
        private void TestVitalityDrop()
        {
            OnEnemyDefeated(transform.position + Vector3.forward * 2f, 1, EssenceType.Vitality);
        }

        [ContextMenu("Simulate Forbidden Essence Drop")]
        private void TestForbiddenDrop()
        {
            OnEnemyDefeated(transform.position + Vector3.forward * 2f, 3, EssenceType.Forbidden, 1.5f);
        }

        [ContextMenu("Print Debug Info")]
        private void PrintDebugInfo()
        {
            Debug.Log($"[EssenceManager] {GetDebugInfo()}");
        }

        private void OnDrawGizmos()
        {
            if (!_showAbsorptionGizmos || !Application.isPlaying) return;

            // Draw absorption opportunities
            foreach (var opportunity in _activeOpportunities)
            {
                if (opportunity.IsActive)
                {
                    Gizmos.color = opportunity.Essence.GetDisplayInfo().typeColor;
                    Gizmos.DrawWireSphere(opportunity.Position, 0.5f);
                    
                    // Draw timer indicator
                    float timerHeight = (opportunity.TimeRemaining / _absorptionTimeWindow) * 2f;
                    Gizmos.DrawLine(opportunity.Position + Vector3.up, opportunity.Position + Vector3.up * (1 + timerHeight));
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Represents an active essence absorption opportunity with timing
    /// </summary>
    [System.Serializable]
    public class AbsorptionOpportunity
    {
        public SoulEssence Essence;
        public Vector3 Position;
        public float TimeRemaining;
        public bool IsActive;
        public Coroutine ExpirationCoroutine;
    }

    /// <summary>
    /// Types of essence conversion for banking system
    /// </summary>
    public enum EssenceConversionType
    {
        Health,     // Convert to health restoration
        Mana,       // Convert to mana restoration  
        Experience, // Convert to experience points
        SoulEnergy  // Convert to raw soul energy
    }
} 